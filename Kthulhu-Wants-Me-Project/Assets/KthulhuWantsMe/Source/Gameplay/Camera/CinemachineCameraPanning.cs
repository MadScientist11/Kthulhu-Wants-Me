using Freya;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Camera
{
    public class CinemachineCameraPanning : MonoBehaviour
    {
        public bool DisablePanning { get; set; }

        [SerializeField] private Vector2 PanRange = new(2, 2);
        [SerializeField] private AnimationCurve TestCurve;
        
        private CinemachineCameraOffset _cinemachineCameraOffset;
        
        private const float SmoothTime = .5f;

        private Vector3 _targetOffset;
        private Vector3 _velocity = Vector3.zero;

        private void Awake() => 
            _cinemachineCameraOffset = GetComponent<CinemachineCameraOffset>();

        private void Update()
        {
            if(DisablePanning)
                return;
            
            Vector3 screenCenter = (new Vector3(Screen.width, Screen.height) / 2);
            Vector2 centeredMousePosition = Input.mousePosition - screenCenter;

            Vector2 vectorToMouse = centeredMousePosition;

            Vector2 panRange = Mathfs.Remap(-new Vector2(screenCenter.x, screenCenter.y), new Vector2(screenCenter.x, screenCenter.y),
                -PanRange, PanRange, vectorToMouse);

            Vector2 t = Mathfs.Remap(-PanRange, PanRange, -Vector2.one, Vector2.one, panRange);

            float f = TestCurve.Evaluate(t.x);
            float f1 = TestCurve.Evaluate(t.y);
            Debug.Log(f);
            Vector2 lerped = Mathfs.Lerp(-PanRange, PanRange, new Vector2(f,f1));

            _targetOffset += lerped.XYtoXYZ();
            _targetOffset = Mathfs.Clamp(_targetOffset, -PanRange.XYtoXYZ(),
                PanRange.XYtoXYZ());
                
            _cinemachineCameraOffset.m_Offset = Vector3.SmoothDamp(_cinemachineCameraOffset.m_Offset, _targetOffset, ref _velocity, SmoothTime);
        }
    }
}
