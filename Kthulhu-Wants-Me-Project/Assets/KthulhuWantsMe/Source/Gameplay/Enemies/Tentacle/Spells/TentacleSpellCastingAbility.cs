using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.Spells
{
    public class TentacleSpellCastingAbility : MonoBehaviour, IAbility
    {
        public bool CastingSpell { get; private set; }

        public Transform SpellSpawnPoint;

        private Dictionary<TentacleSpell, ITentacleSpell> _tentacleSpells;
        
        private List<TentacleSpell> _activeSpells = new();


        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IDataProvider dataProvider, IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            
        }

        private void Start() =>
            CreateSpells();

        public async UniTaskVoid CastSpell(TentacleSpell spell)
        {
            //In collection, cancel if 
            CastingSpell = true;
            await _tentacleSpells[spell].Cast();
            CastingSpell = false;
            _activeSpells.Add(spell);
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

        public bool CanCastSpell(TentacleSpell spell) => !CastingSpell && !IsActive(spell) && 
            !_gameFactory.Player.TentacleSpellResponse.IsActiveDebuff(spell);

        public bool IsActive(TentacleSpell spell)
            => _tentacleSpells[spell].Active;

        public ITentacleSpell Get(TentacleSpell spell) => 
            _tentacleSpells[spell];

        private void CreateSpells()
        {
            ProhibitHealthItemsUsageSpell prohibitHealthItemsUsageSpell
                = new ProhibitHealthItemsUsageSpell(_gameFactory.Player, this);

            SpawnMinionsSpell spawnMinionsSpell
                = new SpawnMinionsSpell(_gameFactory, this);

            _tentacleSpells = new()
            {
                { TentacleSpell.PlayerCantUseHealthItems, prohibitHealthItemsUsageSpell },
                { TentacleSpell.MinionsSpawnSpell, spawnMinionsSpell },
            };
        }
    }
}