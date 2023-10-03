using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
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
        public bool CastingSpell { get; set; }

        public Transform SpellSpawnPoint;

        private Dictionary<TentacleSpell, ITentacleSpell> _tentacleSpells;

        private List<TentacleSpell> _activeSpells = new();


        private IGameFactory _gameFactory;
        private ThePlayer _playerModel;
        private IBuffDebuffFactory _buffDebuffFactory;

        [Inject]
        public void Construct(IDataProvider dataProvider, IGameFactory gameFactory, ThePlayer playerModel,
            IBuffDebuffFactory buffDebuffFactory)
        {
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
            Debug.Log("Cancel Spell");
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
            AllSpells allSpells = (AllSpells)await Resources.LoadAsync<AllSpells>("Enemies/Spells/AllSpells");

            ProhibitHealthItemsUsageSpell prohibitHealthItemsUsageSpell
                = new ProhibitHealthItemsUsageSpell(_gameFactory.Player, this);

            SpawnMinionsSpell spawnMinionsSpell
                = new SpawnMinionsSpell(_gameFactory, this);

            BasicAttackSpell basicAttackSpell
                = new BasicAttackSpell(this, allSpells[TentacleSpell.BasicAttackSpell], _gameFactory.Player,
                    _playerModel);

            BuffSpell buffSpell
                = new BuffSpell(this, allSpells[TentacleSpell.BasicAttackSpell], _buffDebuffFactory);


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