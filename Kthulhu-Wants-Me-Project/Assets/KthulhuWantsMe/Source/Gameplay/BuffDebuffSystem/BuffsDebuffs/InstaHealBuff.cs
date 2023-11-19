using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem.BuffsDebuffs
{
    public class InstaHealBuff : IBuffDebuff
    {
        public EffectId EffectId => EffectId.Unknown;
        
        private float _healAmount;

        private const string InstaHealVFXPath = "Effects/InstaHealVFX";

        private ParticleSystem _vfxInstance;
        
        private IResourceManager _resourceManager;
        private ParticleSystem _vfxPrefab;

        [Inject]
        public void Construct(IResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public InstaHealBuff Init(int healAmount)
        {
            _healAmount = healAmount;
            _vfxPrefab = Resources.Load<ParticleSystem>(InstaHealVFXPath);
            return this;
        }


        public void ApplyEffect(IEffectReceiver effectReceiver)
        {
            if (effectReceiver.Transform.TryGetComponent(out IHealable healable))
            {
                healable.Heal(_healAmount);
                
                _vfxInstance = Object.Instantiate(_vfxPrefab, effectReceiver.Transform);
                _vfxInstance.transform.position += Vector3.up;
            }
        }
    }
}