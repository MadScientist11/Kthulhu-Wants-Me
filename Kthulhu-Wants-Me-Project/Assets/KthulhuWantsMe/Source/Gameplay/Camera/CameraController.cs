using Cinemachine;
using UnityEngine;

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

        private void Start() => 
            Cursor.lockState = CursorLockMode.Confined;

        public void PullFocusOn(ICinemachineCamera cinemachineCamera)
        {
            if(cinemachineCamera == ActiveVCam)
                return;
            
            ActiveVCam.VirtualCameraGameObject.SetActive(false);
            cinemachineCamera.VirtualCameraGameObject.SetActive(true);
        }
    }
}