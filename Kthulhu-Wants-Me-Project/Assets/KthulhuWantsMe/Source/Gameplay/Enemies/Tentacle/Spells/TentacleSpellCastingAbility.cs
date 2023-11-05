using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.Spells
{
    public class TentacleSpellCastingAbility : MonoBehaviour, IAbility
    {
        public EnemyStatsContainer Stats => _enemyStatsContainer;
        public bool CastingSpell { get; set; }

        public Transform SpellSpawnPoint;
        [SerializeField] private EnemyStatsContainer _enemyStatsContainer;

        private Dictionary<TentacleSpell, ITentacleSpell> _tentacleSpells;

        private readonly List<TentacleSpell> _activeSpells = new();


        private IGameFactory _gameFactory;
        private ThePlayer _playerModel;
        private IBuffDebuffFactory _buffDebuffFactory;
        private IBuffDebuffService _buffDebuffService;

        [Inject]
        public void Construct(IDataProvider dataProvider, IGameFactory gameFactory, ThePlayer playerModel,
            IBuffDebuffFactory buffDebuffFactory, IBuffDebuffService buffDebuffService)
        {
            _buffDebuffService = buffDebuffService;
            _buffDebuffFactory = buffDebuffFactory;
            _playerModel = playerModel;
            _gameFactory = gameFactory;
        }

        private void Start() =>
            CreateSpells();

        public async UniTaskVoid CastSpell(TentacleSpell spell)
        {
            //In collection, cancel if 
            _activeSpells.Add(spell);
            await _tentacleSpells[spell].Cast();
        }

        public void CancelActiveSpells()
        {
            foreach (TentacleSpell tentacleSpell in _activeSpells)
            {
                CancelSpell(tentacleSpell).Forget();
            }

            _activeSpells.Clear();
        }

        public async UniTaskVoid CancelSpell(TentacleSpell spell)
        {
            await _tentacleSpells[spell].Undo();
        }

        public bool CanCastSpell(TentacleSpell spell) => !CastingSpell && !IsActive(spell) && IsNotOnCooldown(spell) &&
                                                         !_gameFactory.Player.TentacleSpellResponse.IsActiveDebuff(
                                                             spell);

        public bool IsActive(TentacleSpell spell)
            => _tentacleSpells[spell].Active;

        public bool IsNotOnCooldown(TentacleSpell spell)
            => !_tentacleSpells[spell].InCooldown;

        public ITentacleSpell Get(TentacleSpell spell) =>
            _tentacleSpells[spell];

        private async UniTaskVoid CreateSpells()
        {
            AllSpells allSpells = null;
            if (_enemyStatsContainer.Config.EnemyType == EnemyType.TentacleElite)
            {
                allSpells = (AllSpells)await Resources.LoadAsync<AllSpells>("Enemies/Spells/AllSpellsElite");
            }
            else if (_enemyStatsContainer.Config.EnemyType == EnemyType.PoisonousTentacle)
            {
                allSpells = (AllSpells)await Resources.LoadAsync<AllSpells>("Enemies/Spells/AllSpellsPoison");
            }
            else
            {
                allSpells = (AllSpells)await Resources.LoadAsync<AllSpells>("Enemies/Spells/AllSpells");
            }


            ProhibitHealthItemsUsageSpell prohibitHealthItemsUsageSpell
                = new ProhibitHealthItemsUsageSpell(_gameFactory.Player, this);

            SpawnMinionsSpell spawnMinionsSpell
                = new SpawnMinionsSpell(_gameFactory, this);

            BasicAttackSpell basicAttackSpell
                = new BasicAttackSpell(this, allSpells[TentacleSpell.BasicAttackSpell], _gameFactory.Player,
                    _playerModel, _buffDebuffFactory, _buffDebuffService);

            BuffSpell buffSpell
                = new BuffSpell(this, allSpells[TentacleSpell.Buff], _buffDebuffFactory);


            _tentacleSpells = new()
            {
                { TentacleSpell.PlayerCantUseHealthItems, prohibitHealthItemsUsageSpell },
                { TentacleSpell.MinionsSpawnSpell, spawnMinionsSpell },
                { TentacleSpell.BasicAttackSpell, basicAttackSpell },
                { TentacleSpell.Buff, buffSpell },
            };
        }
    }
}