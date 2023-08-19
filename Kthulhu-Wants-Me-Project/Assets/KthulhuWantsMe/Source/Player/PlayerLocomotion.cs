using KinematicCharacterController;
using KthulhuWantsMe.Source.Utilities;
using UnityEngine;

namespace KthulhuWantsMe.Source.Player
{
    public class PlayerLocomotion : MonoBehaviour
    {
        public KinematicCharacterMotor KinematicCharacterMotor;
        private KinematicLocomotion _locomotion;
        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";

        private void Awake()
        {
            _locomotion = new KinematicLocomotion(KinematicCharacterMotor);
        }

        public void SetGravity(float gravity)
        {
            _locomotion.KillVelocity = true;
            _locomotion.Gravity = new Vector3(0, gravity, 0);
        }

        private void Update()
        {
            Ray ray = MousePointer.GetWorldRay(Camera.main);
            var vectorToHit = Vector3.zero;
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);

                Vector3 hitPointXZ = new Vector3(hitPoint.x, transform.position.y, hitPoint.z);
                vectorToHit = (hitPointXZ - transform.position);
                //_playerTransform.forward = _vectorToHit.normalized;
            }


            Debug.DrawRay(Vector3.zero, vectorToHit.normalized * 5);

            _locomotion.SetInputs(new Vector2(Input.GetAxisRaw(HorizontalInput), Input.GetAxisRaw(VerticalInput)),
                vectorToHit.normalized);

            // Apply impulse
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //_locomotion.Motor.ForceUnground(0.1f);
                _locomotion.AddVelocity(transform.forward * 100f);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = transform.position + transform.forward;
                sphere.transform.localScale = Vector3.one * .25f;
                sphere.AddComponent<Rigidbody>().AddForce(transform.forward * 2000);
            }
        }
    }
}