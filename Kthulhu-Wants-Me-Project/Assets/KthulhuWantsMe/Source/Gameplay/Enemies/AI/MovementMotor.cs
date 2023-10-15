﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementMotor : MonoBehaviour
    {
        public float MoveSpeed
        {
            get => _navMeshAgent.speed;
            set => _navMeshAgent.speed = value;
        }

        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Rigidbody _rigidbody;

        private float _defaultAngularSpeed;

        private void Start()
        {
            _defaultAngularSpeed = _navMeshAgent.angularSpeed;
        }

        private void OnValidate()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void MoveTo(Vector3 destination)
        {
            if (_navMeshAgent.enabled)
            {
                _navMeshAgent.SetDestination(destination);
            }
        }

        public void AddVelocity(Vector3 velocity, float resetAfter)
        {
            StartCoroutine(DoVelocityChange(velocity, resetAfter));
        }

        private IEnumerator DoVelocityChange(Vector3 velocity, float resetAfter)
        {
            _navMeshAgent.velocity = velocity;
            _navMeshAgent.angularSpeed = 0;
            
            yield return new WaitForSeconds(resetAfter);
            
            _navMeshAgent.angularSpeed = _defaultAngularSpeed;
            _navMeshAgent.velocity = Vector3.zero;
        }

        public void HaltMovement()
        {
            if (_navMeshAgent.enabled)
                _navMeshAgent.isStopped = true;
        }

        public void ResumeMovement()
        {
            if (_navMeshAgent.enabled)
                _navMeshAgent.isStopped = false;
        }

        public void Disable() => _navMeshAgent.SwitchOff();

        public void Enable() => _navMeshAgent.SwitchOn();
    }
}