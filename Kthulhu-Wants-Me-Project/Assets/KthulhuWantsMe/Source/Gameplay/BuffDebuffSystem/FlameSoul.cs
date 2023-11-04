using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem.BuffsDebuffs;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem
{
    public class FlameSoul : BuffItem
    {
        [SerializeField] private float _flameBuffDuration = 10;
        [SerializeField] private float _waitBeforeDestroy = 15;
        [SerializeField] private float _shrinkTime = 3;
        [SerializeField] private Ease _shrinkEasing = Ease.InBounce;
        
        private async void Start()
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_waitBeforeDestroy), false, PlayerLoopTiming.Update, destroyCancellationToken);
                await transform
                    .DOScale(0, _shrinkTime)
                    .SetEase(_shrinkEasing)
                    .AwaitForComplete(TweenCancelBehaviour.Kill, destroyCancellationToken);
                Destroy(gameObject);
            }
            catch (OperationCanceledException e)
            {
                Debug.Log("Cancellation occurred, so this task block is executed");
            }
         
        }

        protected override IBuffDebuff ProvideBuff()
        {
            FireBuff effect = EffectFactory.CreateEffect<FireBuff>().Init(_flameBuffDuration);
            return effect;
        }
    }
}