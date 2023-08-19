using KinematicCharacterController;
using UnityEngine;

namespace KthulhuWantsMe.Source.Player
{
    public class KinematicLocomotion : ICharacterController
    {
        private readonly KinematicCharacterMotor _motor;
        private Vector3 _moveInputVector;
        private Vector3 _lookInputVector;
        
        [Header("Stable Movement")]
        public float MaxStableMoveSpeed = 10f;
        public float StableMovementSharpness = 15;
        public float OrientationSharpness = 10;
        [Header("Air Movement")]
        public float MaxAirMoveSpeed = 10f;
        public float AirAccelerationSpeed = 5f;
        public float Drag = 0.1f;
        
        public Vector3 Gravity = new Vector3(0, -30f, 0);
        private Vector3 _internalVelocityAdd;

        public bool KillVelocity;
        public KinematicLocomotion(KinematicCharacterMotor motor)
        {
            _motor = motor;
            motor.CharacterController = this;
        }
        
        public void SetInputs(Vector2 moveInput, Vector3 lookDirection)
        {
            Vector3 moveInputVector = Vector3.ClampMagnitude(new Vector3(moveInput.x, 0f, moveInput.y), 1f);

            Vector3 cameraPlanarDirection = lookDirection;
            //if (cameraPlanarDirection.sqrMagnitude == 0f)
            //{
            //    cameraPlanarDirection = Vector3.ProjectOnPlane(cameraRotation * Vector3.up, _motor.CharacterUp).normalized;
            //}
            //Quaternion cameraPlanarRotation = Quaternion.LookRotation(lookDirection, _motor.CharacterUp);

            _moveInputVector = moveInputVector;
            _lookInputVector = cameraPlanarDirection;
        }

        public void BeforeCharacterUpdate(float deltaTime) { }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (_lookInputVector != Vector3.zero && OrientationSharpness > 0f)
            {
                // Smoothly interpolate from current to target look direction
                Vector3 smoothedLookInputDirection = Vector3.Slerp(_motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

                // Set the current rotation (which will be used by the KinematicCharacterMotor)
                currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, _motor.CharacterUp);
            }
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (KillVelocity)
            {
                currentVelocity = Vector3.zero;
                KillVelocity = false;
            }
            Vector3 targetMovementVelocity = Vector3.zero;
            if (_motor.GroundingStatus.IsStableOnGround)
            {
                // Reorient velocity on slope
                currentVelocity =
                    _motor.GetDirectionTangentToSurface(currentVelocity, _motor.GroundingStatus.GroundNormal) *
                    currentVelocity.magnitude;

                // Calculate target velocity
                Vector3 inputRight = Vector3.Cross(_moveInputVector, _motor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(_motor.GroundingStatus.GroundNormal, inputRight).normalized *
                                          _moveInputVector.magnitude;
                targetMovementVelocity = reorientedInput * MaxStableMoveSpeed;

                // Smooth movement Velocity
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity,
                    1 - Mathf.Exp(-StableMovementSharpness * deltaTime));
            }
            else
            {
                // Add move input
                if (_moveInputVector.sqrMagnitude > 0f)
                {
                    targetMovementVelocity = _moveInputVector * MaxAirMoveSpeed;

                    // Prevent climbing on un-stable slopes with air movement
                    if (_motor.GroundingStatus.FoundAnyGround)
                    {
                        Vector3 perpenticularObstructionNormal = Vector3
                            .Cross(Vector3.Cross(_motor.CharacterUp, _motor.GroundingStatus.GroundNormal),
                                _motor.CharacterUp).normalized;
                        targetMovementVelocity =
                            Vector3.ProjectOnPlane(targetMovementVelocity, perpenticularObstructionNormal);
                    }

                    Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, Gravity);
                    currentVelocity += velocityDiff * AirAccelerationSpeed * deltaTime;
                }

                // Gravity
                currentVelocity += Gravity * deltaTime;

                // Drag
                currentVelocity *= (1f / (1f + (Drag * deltaTime)));
            }
            // Take into account additive velocity
            if (_internalVelocityAdd.sqrMagnitude > 0f)
            {
                currentVelocity += _internalVelocityAdd;
                _internalVelocityAdd = Vector3.zero;
            }
            
        }

        public void AfterCharacterUpdate(float deltaTime) { }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }

        public void PostGroundingUpdate(float deltaTime)
        {
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport) { }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) { }

        public void OnDiscreteCollisionDetected(Collider hitCollider) { }
        
        public void AddVelocity(Vector3 velocity)
        {
            _internalVelocityAdd += velocity;
        }
    }
}