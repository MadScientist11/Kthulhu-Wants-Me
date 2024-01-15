using Cinemachine;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Utilities.Extensions;
using UnityEngine;
using VContainer;
using Vertx.Debugging;

namespace KthulhuWantsMe.Source.Gameplay.Camera
{
    public class CameraController : MonoBehaviour
    {
        public ICinemachineCamera ActiveVCam
        {
            get
            {
                return _cinemachineBrain.ActiveVirtualCamera;
            }
        }

        [SerializeField] private CinemachineBrain _cinemachineBrain;

        private IGameFactory _gameFactory;
        [SerializeField] private float _offset = 1f;
        [SerializeField] private float _radius = 1f;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        private void Start() =>
            Cursor.lockState = CursorLockMode.Confined;


        private void Update()
        {
            FadeObjectsInView();
        }

        private void FadeObjectsInView()
        {
            Vector3 dir = _gameFactory.Player.transform.position.AddY(2) - (transform.position - transform.up * _offset);
            Ray ray = new Ray(transform.position - transform.up * _offset, dir.normalized);
            int playerLayer = LayerMask.NameToLayer(GameConstants.Layers.Player);
            RaycastHit[] hits = new RaycastHit[12];
            int hitCount = DrawPhysics.SphereCastNonAlloc(ray, _radius, hits, dir.magnitude, playerLayer);

            if (hitCount > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit hit = hits[i];
                    if (hit.transform != null)
                    {
                        if (hit.transform.TryGetComponent(out FadeableObject fadeableObject))
                        {
                            fadeableObject.RequestFade();
                        }
                    }
                }
            }
        }
    }
}