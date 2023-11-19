using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem.BuffsDebuffs;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public interface IBuffDebuffFactory
    {
        T CreateEffect<T>() where T : IBuffDebuff, new();
    }

    public class BuffDebuffFactory : IBuffDebuffFactory
    {
        private Dictionary<Type, IBuffDebuff> _cache = new();
        private IObjectResolver _instantiator;

        public BuffDebuffFactory(IObjectResolver instantiator)
        {
            _instantiator = instantiator;
        }

        public TEffect CreateEffect<TEffect>() where TEffect : IBuffDebuff, new()
        {
            TEffect effect = new TEffect();
            _instantiator.Inject(effect);
            return effect;
        }
    }
}