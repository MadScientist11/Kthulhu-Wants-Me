using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerMovementController : ICharacterController
    {
        private readonly KinematicCharacterMotor _motor;
        private readonly PlayerConfiguration _playerConfiguration;

        private Vector3 _moveInputVector;
        private Vector3 _lookInputVector;
        private Vector3 _internalVelocityAdd;
        private bool _killVelocity;

        public Vector3 CurrentVelocity => _motor.Velocity;
        public Vector3 InternalVelocityAdd => _internalVelocityAdd;
        public bool IsGrounded => _motor.GroundingStatus.IsStableOnGround;

        public PlayerMovementController(KinematicCharacterMotor motor, PlayerConfiguration playerConfiguration)
        {
            _playerConfiguration = playerConfiguration;
            _motor = motor;
            motor.CharacterController = this;

            MaxQueueSize = Mathf.CeilToInt(1f / _historicalPositionInterval * _historicalPositionDuration);
            _historicalVelocities = new Queue<Vector3>(MaxQueueSize);
        }

        public void SetInputs(Vector2 moveInput, Vector3 lookDirection)
        {
            Vector3 moveInputVector = Vector3.ClampMagnitude(new Vector3(moveInput.x, 0f, moveInput.y), 1f);
            Vector3 cameraPlanarDirection = lookDirection;
            _moveInputVector = moveInputVector;
            _lookInputVector = cameraPlanarDirection;
        }

        public void ResetInputs() =>
            _moveInputVector = Vector3.zero;

        public void KillVelocity() =>
            _killVelocity = true;

        public void ToggleMotor(bool value)
        {
            if (_motor.enabled == value)
                return;

            _motor.enabled = value;
        }

        public void EnableCollisionDetection()
        {
            _motor.SetCapsuleCollisionsActivation(true);
            _motor.SetMovementCollisionsSolvingActivation(true);
        }
        
        public void DisableCollisionDetection()
        {
            _motor.SetCapsuleCollisionsActivation(false);
            _motor.SetMovementCollisionsSolvingActivation(false);
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (_lookInputVector != Vector3.zero && _playerConfiguration.OrientationSharpness > 0f)
            {
                // Smoothly interpolate from current to target look direction
                Vector3 smoothedLookInputDirection = Vector3.Slerp(_motor.CharacterForward, _lookInputVector,
                    1 - Mathf.Exp(-_playerConfiguration.OrientationSharpness * deltaTime)).normalized;

                // Set the current rotation (which will be used by the KinematicCharacterMotor)
                currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, _motor.CharacterUp);
            }
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (_killVelocity)
            {
                currentVelocity = Vector3.zero;
                _killVelocity = false;
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
                targetMovementVelocity = reorientedInput * _playerConfiguration.MoveSpeed;

                // Smooth movement Velocity
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity,
                    1 - Mathf.Exp(-_playerConfiguration.StableMovementSharpness * deltaTime));
            }
            else
            {
                // Add move input
                if (_moveInputVector.sqrMagnitude > 0f)
                {
                    targetMovementVelocity = _moveInputVector * _playerConfiguration.MaxAirMoveSpeed;

                    // Prevent climbing on un-stable slopes with air movement
                    if (_motor.GroundingStatus.FoundAnyGround)
                    {
                        Vector3 perpenticularObstructionNormal = Vector3
                            .Cross(Vector3.Cross(_motor.CharacterUp, _motor.GroundingStatus.GroundNormal),
                                _motor.CharacterUp).normalized;
                        targetMovementVelocity =
                            Vector3.ProjectOnPlane(targetMovementVelocity, perpenticularObstructionNormal);
                    }

                    Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity,
                        _playerConfiguration.Gravity);
                    currentVelocity += velocityDiff * _playerConfiguration.AirAccelerationSpeed * deltaTime;
                }

                // Gravity
                currentVelocity += _playerConfiguration.Gravity * deltaTime;

                // Drag
                currentVelocity *= (1f / (1f + (_playerConfiguration.Drag * deltaTime)));
            }

            // Take into account additive velocity
            if (_internalVelocityAdd.sqrMagnitude > 0f)
            {
                currentVelocity += _internalVelocityAdd;
                _internalVelocityAdd = Vector3.zero;
            }
        }

        private float _historicalPositionDuration = 1f;
        private float _historicalPositionInterval = 0.1f;

        public Vector3 AverageVelocity
        {
            get
            {
                Vector3 average = Vector3.zero;
                foreach (Vector3 historicalVelocity in _historicalVelocities)
                {
                    average += historicalVelocity;
                }

                average.y = 0;

                return average / _historicalVelocities.Count;
            }
        }

        private Queue<Vector3> _historicalVelocities;
        private float _lastPositionTime;
        private int MaxQueueSize;

        public void AfterCharacterUpdate(float deltaTime)
        {
            if (_lastPositionTime + _historicalPositionInterval <= Time.time)
            {
                if(_historicalVelocities.Count == MaxQueueSize)
                {
                    _historicalVelocities.Dequeue();
                }
                
                _historicalVelocities.Enqueue(CurrentVelocity);
                _lastPositionTime = Time.time;
            }
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
        }

        public void PostGroundingUpdate(float deltaTime)
        {
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }

        public void AddVelocity(Vector3 velocity)
        {
            _internalVelocityAdd += velocity;
        }
    }
}