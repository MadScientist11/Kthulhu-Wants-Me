using System.Collections.Generic;
using Freya;
using KthulhuWantsMe.Source.Gameplay;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace KthulhuWantsMe.Source.UI.Compass
{
    public class Marker
    {
        public Vector2 Position => TrackedObject.position.XZ();
        public Transform TrackedObject;
    }
    public class CompassUI : MonoBehaviour
    {
        [SerializeField] private RawImage _compassImage;
        [SerializeField] private GameObject _markerPrefab;

        private readonly Dictionary<Marker, GameObject> _markerViews = new();
        private readonly List<Marker> _markers = new();
        
        private Transform _player;
        private float _compassUnit;
        private bool _initialized;
        
        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void Show()
        {
            gameObject.SwitchOn();
            _player = _gameFactory.Player.transform;
            _compassUnit = _compassImage.rectTransform.rect.width / 360f;
            _initialized = true;
        }

        public void Hide()
        {
            gameObject.SwitchOff();
            _initialized = false;
        }

        public void AddMarker(Marker marker)
        {
            GameObject markerView = CreateMarker();
            _markers.Add(marker);
            _markerViews[marker] = markerView;
        }
        
        public void RemoveMarker(Marker marker)
        {
            _markers.Remove(marker);
            GameObject markerView = _markerViews[marker];
            _markerViews.Remove(marker);
            Destroy(markerView);
        }

        private void Update()
        {
            if(!_initialized)
                return;
            
            _compassImage.uvRect = new Rect(_player.localEulerAngles.y / 360f, 0f, 1f, 1f);
            
            foreach (Marker marker in _markers)
            {
                _markerViews[marker].GetComponent<RectTransform>().anchoredPosition = CalculatePositionFor(marker);
            }
        }

        private Vector2 CalculatePositionFor(Marker marker)
        {
            Vector2 playerPosition = _player.position.XZ();
            Vector2 playerForward = _player.forward.XZ();

            float angle = Vector2.SignedAngle(marker.Position - playerPosition, playerForward);
            return new Vector2(_compassUnit * angle, 0f);
        }

        private GameObject CreateMarker()
        {
            GameObject markerInstance = Instantiate(_markerPrefab, _compassImage.transform);
            return markerInstance;
        }
    }
}
