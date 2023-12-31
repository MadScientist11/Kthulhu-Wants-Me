﻿using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Freya;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem.BuffsDebuffs;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem
{
    public class HealthSoul : BuffItem
    {
        [SerializeField] private int _healAmount;

        private Vector3 _velocity;

        private PlayerFacade _player;
        private ThePlayer _thePlayer;

        [Inject]
        public void Construct(IGameFactory gameFactory, ThePlayer thePlayer)
        {
            _thePlayer = thePlayer;
            _player = gameFactory.Player;
        }

        private async void Start()
        {
            try
            {
                await transform.DOMove(transform.position + Random.insideUnitCircle.XZtoXYZ() * 3, .5f)
                    .SetEase(Ease.InSine)
                    .AwaitForComplete(TweenCancelBehaviour.Kill, destroyCancellationToken);
                
                await UniTask.Delay(TimeSpan.FromSeconds(7), false, PlayerLoopTiming.Update, destroyCancellationToken);
                
                await transform.DOScale(0, 3f)
                    .SetEase(Ease.InBounce)
                    .AwaitForComplete(TweenCancelBehaviour.Kill, destroyCancellationToken);;
                Destroy(gameObject);
            }
            catch (OperationCanceledException e)
            {
                Debug.Log($"Cancellation occurred, so this task block is executed {e}");
            }
        }

        private void Update()
        {
            if (_thePlayer.IsFullHp)
                return;

            if (InRange())
                FollowPlayer();
        }

        protected override IBuffDebuff ProvideBuff()
        {
            if (_thePlayer.IsFullHp)
            {
                return null;
            }

            InstaHealBuff effect = EffectFactory.CreateEffect<InstaHealBuff>();
            effect.Init(_healAmount);
            return effect;
        }

        private bool InRange() =>
            Vector3.Distance(transform.position, _player.transform.position) < 3;

        private void FollowPlayer()
        {
            transform.position =
                Vector3.SmoothDamp(transform.position, _player.transform.position.AddY(.5f), ref _velocity, 0.2f);
        }
    }
}