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
                if (EntityDoesntHaveTheEffectApplied(effect, effects))
                {
                    ApplyEffectInternal(effect, to);
                }
            }
            else
            {
                ActiveEffectReceivers.Add(to, new List<IBuffDebuff>() { effect });
                ApplyEffectInternal(effect, to);
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
            if (effect is not IUpdatableEffect)
            {
                CancelEffectInternal(effect, onEffectReceiver);
                return;
            }

            _queuedCancellations.Add(() => CancelEffectInternal(effect, onEffectReceiver));
        }

        public void CancelAllEffectsOnEntity(IEffectReceiver entity)
        {
            foreach (IBuffDebuff effect in ActiveEffectReceivers[entity])
                CancelEffect(effect, entity);
        }

        private void ApplyEffectInternal(IBuffDebuff effect, IEffectReceiver to)
        {
            effect.ApplyEffect(to);
            if (IsInstaEffect(effect))
                CancelEffectInternal(effect, to);
        }

        private void CancelEffectInternal(IBuffDebuff effect, IEffectReceiver onEffectReceiver)
        {
            if (effect is IUpdatableEffect updatableEffect)
                updatableEffect.CancelEffect(onEffectReceiver);

            ActiveEffectReceivers[onEffectReceiver].Remove(effect);

            if (ActiveEffectReceivers[onEffectReceiver] == null || ActiveEffectReceivers[onEffectReceiver].Count == 0)
                ActiveEffectReceivers.Remove(onEffectReceiver);
        }

        private bool EntityDoesntHaveTheEffectApplied(IBuffDebuff effect, List<IBuffDebuff> effects)
        {
            return effects.FirstOrDefault(e => e.GetType() == effect.GetType()) == null;
        }

        private bool IsInstaEffect(IBuffDebuff effect)
        {
            return effect is not IUpdatableEffect;
        }
    }
}