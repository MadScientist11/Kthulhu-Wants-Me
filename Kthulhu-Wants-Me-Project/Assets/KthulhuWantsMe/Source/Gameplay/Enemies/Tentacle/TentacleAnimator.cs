﻿using System;
using KthulhuWantsMe.Source.Gameplay.AnimatorHelpers;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.StaterResetter;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleAnimator : MonoBehaviour
    {
        [SerializeField] private Renderer _tentacleRenderer;
        [SerializeField] private Animator _tentacleAnimator;
        [SerializeField] private Rig _tentacleRig;
        [SerializeField] private Vector3 _tentacleGrabOffset;
        [SerializeField] private float _tentacleGrabRadius;
        [SerializeField] private float _twirlStrength;

        [SerializeField] private Transform _grabTarget;
        [SerializeField] private Transform _chainTarget;
        
        private Material _tentacleMaterial;
        private bool _grabPlayerAnimationActive;

        private static readonly int GrabPlayerParam = Shader.PropertyToID("_GrabPlayer");
        private static readonly int Radius = Shader.PropertyToID("_Radius");
        private static readonly int InteractPos = Shader.PropertyToID("_InteractPos");
        private static readonly int TwirlStrength = Shader.PropertyToID("_TwirlStrength");
        
        
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Aggro = Animator.StringToHash("Aggro");
        private static readonly int Impact = Animator.StringToHash("Impact");
        private static readonly int GrabPlayer = Animator.StringToHash("GrabPlayer");
        private static readonly int Emerge = Animator.StringToHash("Emerged");
        private static readonly int Retreat = Animator.StringToHash("Retreat");

        
        private static readonly int _idleStateHash = Animator.StringToHash("Idle");
        private static readonly int _attackStateHash = Animator.StringToHash("Attack");
        
        private PlayerFacade _player;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _player = gameFactory.Player;
        }

        private void Awake()
        {
            _tentacleMaterial = _tentacleRenderer.material;
        }

        private void Update()
        {
            _tentacleMaterial.SetFloat(Radius, _tentacleGrabRadius);
            _tentacleMaterial.SetFloat(TwirlStrength, _twirlStrength);
            _tentacleMaterial.SetVector(InteractPos, _grabTarget.localPosition + _tentacleGrabOffset);
        }
        
        public void PlayGrabPlayerAnimation(Transform playerFollowTarget)
        {
            //_playerFollowTarget = playerFollowTarget;
        }

        public void PlayGrabPlayerAttack()
        {
            _tentacleMaterial.SetInt(GrabPlayerParam, 1);
            _tentacleAnimator.SetBool(GrabPlayer, true);
            
            _tentacleRig.weight = 0;
        }

        public void CancelGrab()
        {
            _tentacleMaterial.SetInt(GrabPlayerParam, 0);
            _tentacleAnimator.SetBool(GrabPlayer, false);
            _tentacleRig.weight = 1;
        }

        public void ResetAnimator()
        {
            _tentacleAnimator.Rebind();
            _tentacleAnimator.Update(0f);
        }

        public void PlayIdle()
        { }

        public void PlayAttack()
        {
            _chainTarget.position = _player.transform.position;
            _tentacleAnimator.SetTrigger(Attack);
        }

        public void CancelAttack()
        {
            _tentacleAnimator.SetBool(Attack, false);
        }

        public void PlayAggroIdle()
        {
            _tentacleAnimator.SetBool(Aggro, true);
        }

        public void PlayImpact()
        {
            _tentacleAnimator.SetTrigger(Impact);
        }

        public void PlayEmerge()
        {
            _tentacleRig.weight = 0;
            _tentacleAnimator.SetBool(Emerge, false);
        }

        public void SetEmerged()
        {
            _tentacleRig.weight = 1;
            _tentacleAnimator.SetBool(Emerge, true);
        }

        public void PlayRetreat()
        {
            _tentacleAnimator.SetBool(Retreat, true);
            _tentacleRig.weight = 0;
        }

    }
}