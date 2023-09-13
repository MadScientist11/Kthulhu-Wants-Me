using System;
using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem.BuffsDebuffs;
using UnityEngine;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem
{
    public interface IBuffDebuffService
    {
        Dictionary<IEffectReceiver, List<IBuffDebuff>> ActiveEffectReceivers { get; }
        void ApplyEffect(IBuffDebuff effect, IEffectReceiver to);
        void CancelEffect(IBuffDebuff effect, IEffectReceiver onEffectReceiver);
        void CancelAllEffectsOnEntity(IEffectReceiver entity);
    }


    public class BuffDebuffService : IBuffDebuffService, ITickable
    {
        public Dictionary<IEffectReceiver, List<IBuffDebuff>> ActiveEffectReceivers { get; } = new();

        private readonly List<Action> _queuedCancellations = new();

        public void ApplyEffect(IBuffDebuff effect, IEffectReceiver to)
        {
            if (ActiveEffectReceivers.TryGetValue(to, out List<IBuffDebuff> effects))
            {
                if (effects.FirstOrDefault(e => e.GetType() == effect.GetType()) == null)
                {
                    effects.Add(effect);
                    effect.ApplyEffect(to);
                }
            }
            else
            {
                ActiveEffectReceivers.Add(to, new List<IBuffDebuff>() { effect });
                effect.ApplyEffect(to);
            }
        }


        public void Tick()
        {
            foreach (KeyValuePair<IEffectReceiver, List<IBuffDebuff>> valuePair in ActiveEffectReceivers)
            {
                foreach (IBuffDebuff buffDebuff in valuePair.Value)
                {
                    if (buffDebuff is IUpdatableEffect updatableEffect)
                        updatableEffect.UpdateEffect();
                }
            }

            foreach (Action queuedCancellation in _queuedCancellations)
            {
                queuedCancellation?.Invoke();
            }

            _queuedCancellations.Clear();
        }

        public void CancelEffect(IBuffDebuff effect, IEffectReceiver onEffectReceiver)
        {
            _queuedCancellations.Add(() => CancelEffectInternal(effect, onEffectReceiver));
        }

        public void CancelAllEffectsOnEntity(IEffectReceiver entity)
        {
            foreach (IBuffDebuff effect in ActiveEffectReceivers[entity])
            {
                _queuedCancellations.Add(() => CancelEffectInternal(effect, entity));
            }
        }

        private void CancelEffectInternal(IBuffDebuff effect, IEffectReceiver onEffectReceiver)
        {
            if (effect is IUpdatableEffect updatableEffect)
            {
                updatableEffect.CancelEffect(onEffectReceiver);
                ActiveEffectReceivers[onEffectReceiver].Remove(effect);
            }
        }
    }
}