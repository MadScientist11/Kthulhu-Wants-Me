using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem.BuffsDebuffs;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.Spells
{
    public class BasicAttackSpell : ITentacleSpell
    {
        public bool Active { get; private set; }
        public bool InCooldown { get; private set; }

        
        private GameObject _spellInstance;

        private CancellationTokenSource _spellCastToken;

        private readonly PlayerFacade _player;
        private readonly ThePlayer _playerModel;
        private readonly TentacleSpellCastingAbility _spellCastingAbility;
        private readonly SpellConfiguration _spellConfiguration;
        private readonly IBuffDebuffFactory _buffDebuffFactory;
        private readonly IBuffDebuffService _buffDebuffService;

        public BasicAttackSpell(TentacleSpellCastingAbility spellCastingAbility, SpellConfiguration spellConfiguration,
            PlayerFacade player, ThePlayer playerModel, IBuffDebuffFactory buffDebuffFactory, IBuffDebuffService buffDebuffService)
        {
            _buffDebuffService = buffDebuffService;
            _buffDebuffFactory = buffDebuffFactory;
            _spellConfiguration = spellConfiguration;
            _spellCastingAbility = spellCastingAbility;
            _playerModel = playerModel;
            _player = player;
        }

        public async UniTask Cast()
        {
            _spellCastToken = new();
            _spellCastToken.RegisterRaiseCancelOnDestroy(_spellCastingAbility.gameObject);
            InCooldown = true;
            
            _spellCastingAbility.CastingSpell = true;
            Vector3 spellCastPosition = _player.transform.position.AddY(.05f);
            _spellInstance = Object.Instantiate(_spellConfiguration.Prefab, spellCastPosition, _spellConfiguration.Prefab.transform.rotation);
            Active = true;
            await UniTask.Delay(Mathf.FloorToInt(_spellConfiguration.ActivationTime * 1000),false , PlayerLoopTiming.Update, _spellCastToken.Token);
            _spellCastingAbility.CastingSpell = false;
            
            if (Vector3.Distance(spellCastPosition, _player.transform.position) < _spellConfiguration.EffectiveRange)
            {
                EnemyConfiguration enemyConfiguration = _spellCastingAbility.Stats.Config;
                if (enemyConfiguration.EnemyType == EnemyType.PoisonousTentacle)
                {
                    if (_player.TryGetComponent(out EntityBuffDebuffContainer container))
                    {
                        TentacleConfiguration tentacleConfig = enemyConfiguration as TentacleConfiguration;
                        PoisonDebuff poisonDebuff = _buffDebuffFactory
                            .CreateEffect<PoisonDebuff>()
                            .Init(tentacleConfig.PoisonDamagePerSecond, tentacleConfig.PoisonEffectDuration, tentacleConfig.PoisonEffectPrefab);
                       // _playerModel.TakeDamage(poisonDebuff);
                       Debug.Log("Cast?");
                        _buffDebuffService.ApplyEffect(poisonDebuff, container);
                    }
                }
                else
                {
                    _playerModel.TakeDamage(new Damage(15f));
                }
            }

            _spellCastingAbility.CancelSpell(TentacleSpell.BasicAttackSpell).Forget();
        }

        public UniTask Undo()
        {
            Object.Destroy(_spellInstance);
            StartCooldown().Forget();
            Active = false;
            return UniTask.CompletedTask;
        }

        private async UniTaskVoid StartCooldown()
        {
            await UniTask.Delay(Mathf.FloorToInt(_spellConfiguration.Cooldown * 1000));
            InCooldown = false;
        }
    }
}