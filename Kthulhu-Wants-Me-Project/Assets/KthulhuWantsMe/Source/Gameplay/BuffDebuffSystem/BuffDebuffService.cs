﻿using System;
using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem.BuffsDebuffs;
using UnityEngine;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem
{
    public interface IBuffDebuffService
    {
        event Action<IEffectReceiver, IBuffDebuff> EffectRegistered;
        event Action<IEffectReceiver, IBuffDebuff> EffectCanceled;
        
        Dictionary<IEffectReceiver, List<IBuffDebuff>> ActiveEffectReceivers { get; }
        void ApplyEffect(IBuffDebuff effect, IEffectReceiver to);
        void CancelEffect(IBuffDebuff effect, IEffectReceiver onEffectReceiver);
        void CancelAllEffectsOnEntity(IEffectReceiver entity);
       
    }


    public class BuffDebuffService : IBuffDebuffService, ITickable
    {
        public Dictionary<IEffectReceiver, List<IBuffDebuff>> ActiveEffectReceivers { get; } = new();

        public event Action<IEffectReceiver, IBuffDebuff> EffectRegistered;
        public event Action<IEffectReceiver, IBuffDebuff> EffectCanceled;

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
                ApplyEffectInternal(effect, to);
            }
        }


        public void Tick()
        {
            if (Time.timeScale == 0)
            {
                return;
            }

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
            if (ActiveEffectReceivers.TryGetValue(entity, out List<IBuffDebuff> effects))
            {
                foreach (IBuffDebuff effect in effects)
                    CancelEffect(effect, entity);
            }
        }

        private void ApplyEffectInternal(IBuffDebuff effect, IEffectReceiver to)
        {
            effect.ApplyEffect(to);
            
            if (IsInstaEffect(effect))
            {
                return;
            }

            AddEffect(effect, to);
        }

        private void CancelEffectInternal(IBuffDebuff effect, IEffectReceiver onEffectReceiver)
        {
            if (effect is IUpdatableEffect updatableEffect)
                updatableEffect.CancelEffect(onEffectReceiver);

            if (onEffectReceiver == null)
            {
                return;
            }

            ActiveEffectReceivers[onEffectReceiver].Remove(effect);

            if (ActiveEffectReceivers[onEffectReceiver] == null || ActiveEffectReceivers[onEffectReceiver].Count == 0)
                ActiveEffectReceivers.Remove(onEffectReceiver);
            
            EffectCanceled?.Invoke(onEffectReceiver, effect);
        }

        private void AddEffect(IBuffDebuff effect, IEffectReceiver to)
        {
            if (!ActiveEffectReceivers.ContainsKey(to))
                ActiveEffectReceivers.Add(to, new List<IBuffDebuff>() { effect });
            else
                ActiveEffectReceivers[to].Add(effect);
            
            EffectRegistered?.Invoke(to, effect);
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