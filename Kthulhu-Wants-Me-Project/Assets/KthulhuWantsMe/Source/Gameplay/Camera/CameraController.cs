using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineCameraPanning _cameraPanning;

        private void Start() => 
            Cursor.lockState = CursorLockMode.Confined;
    }
}