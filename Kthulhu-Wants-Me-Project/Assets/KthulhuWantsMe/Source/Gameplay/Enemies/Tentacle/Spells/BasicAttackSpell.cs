using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem.BuffsDebuffs;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Utilities.Extensions;
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
        private IGameFactory _gameFactory;

        public BasicAttackSpell(TentacleSpellCastingAbility spellCastingAbility, SpellConfiguration spellConfiguration,
            IGameFactory gameFactory, ThePlayer playerModel, IBuffDebuffFactory buffDebuffFactory, IBuffDebuffService buffDebuffService)
        {
            _gameFactory = gameFactory;
            _buffDebuffService = buffDebuffService;
            _buffDebuffFactory = buffDebuffFactory;
            _spellConfiguration = spellConfiguration;
            _spellCastingAbility = spellCastingAbility;
            _playerModel = playerModel;
            _player = gameFactory.Player;
            _gameFactory = gameFactory;
        }

        public async UniTask Cast()
        {
            _spellCastToken = new();
            _spellCastToken.RegisterRaiseCancelOnDestroy(_spellCastingAbility.gameObject);
            _spellCastToken.Token.Register(CancelSpell);
            InCooldown = true;
            
            _spellCastingAbility.CastingSpell = true;
            Vector3 spellCastPosition = _player.transform.position.AddY(.05f);
            _spellInstance = _gameFactory.CreateInjected(_spellConfiguration.Prefab, spellCastPosition, _spellConfiguration.Prefab.transform.rotation);
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
                        _buffDebuffService.ApplyEffect(poisonDebuff, container);
                    }
                }
                else
                {
                    _playerModel.TakeDamage(new Damage(15f));
                }
            }

            CancelSpell();
        }

        private void CancelSpell()
        {
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