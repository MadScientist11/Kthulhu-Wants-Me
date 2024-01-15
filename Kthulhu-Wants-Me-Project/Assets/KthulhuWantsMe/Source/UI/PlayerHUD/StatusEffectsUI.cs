using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.UI.PlayerHUD
{
    public class StatusEffectsUI : MonoBehaviour
    {
        [SerializeField] private StatusEffect _statusEffectPrefab;

        private readonly Dictionary<IBuffDebuff, StatusEffect> _effectViews = new();

        private PlayerFacade _player;
        private IBuffDebuffService _buffDebuffService;

        [Inject]
        public void Construct(IGameFactory gameFactory, IBuffDebuffService buffDebuffService)
        {
            _buffDebuffService = buffDebuffService;
            _player = gameFactory.Player;
        }

        private void Start()
        {
            _buffDebuffService.EffectRegistered += OnEffectRegistered;
            _buffDebuffService.EffectCanceled += OnEffectCanceled;
        }

        private void OnDestroy()
        {
            _buffDebuffService.EffectRegistered -= OnEffectRegistered;
            _buffDebuffService.EffectCanceled -= OnEffectCanceled;
        }

        private void OnEffectRegistered(IEffectReceiver effectReceiver, IBuffDebuff effect)
        {
            if (IsPlayerEffect(effectReceiver))
            {
                StatusEffect statusEffect = Instantiate(_statusEffectPrefab, transform);
                _effectViews.Add(effect, statusEffect);
            }
        }
        
        private void OnEffectCanceled(IEffectReceiver effectReceiver, IBuffDebuff effect)
        {
            if (IsPlayerEffect(effectReceiver))
            {
                if(_effectViews.TryGetValue(effect, out StatusEffect statusEffect))
                {
                    Destroy(statusEffect.gameObject);
                    _effectViews.Remove(effect);
                }
            }
        }
        private bool IsPlayerEffect(IEffectReceiver effectReceiver)
        {
            return ReferenceEquals(_player.GetComponent<EntityBuffDebuffContainer>(), effectReceiver);
        }
    }
}
