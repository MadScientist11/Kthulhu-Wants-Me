using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.Spell;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public interface ITentacleSpell
    {
        bool Active { get; }
        UniTask Cast();
        UniTask Undo();
    }

    public class ProhibitHealthItemsUsageSpell : ITentacleSpell
    {
        public bool Active { get; private set; }
        private readonly PlayerFacade _player;
        private TentacleSpellCastingAbility _tentacleSpellCastingAbility;

        public ProhibitHealthItemsUsageSpell(PlayerFacade player,
            TentacleSpellCastingAbility tentacleSpellCastingAbility)
        {
            _tentacleSpellCastingAbility = tentacleSpellCastingAbility;
            _player = player;
        }


        public UniTask Cast()
        {
            _player.PlayerInteractionAbility.ApplyBuffsUsageRestriction();
            _player.TentacleSpellResponse.ActivePlayerDebuffs.Add(TentacleSpell.PlayerCantUseHealthItems, this);
            Active = true;
            return UniTask.CompletedTask;
        }

        public UniTask Undo()
        {
            _player.PlayerInteractionAbility.CancelBuffsUsageRestriction();
            _player.TentacleSpellResponse.ActivePlayerDebuffs.Remove(TentacleSpell.PlayerCantUseHealthItems);
            Active = false;
            return UniTask.CompletedTask;
        }
    }

    public class SpawnMinionsSpell : ITentacleSpell
    {
        public bool Active { get; private set; }

        private readonly IGameFactory _gameFactory;
        private readonly TentacleSpellCastingAbility _tentacleSpellCastingAbility;


        private MinionsSpawnSpell _minionsSpawnSpell;

        public SpawnMinionsSpell(IGameFactory gameFactory, TentacleSpellCastingAbility tentacleSpellCastingAbility)
        {
            _tentacleSpellCastingAbility = tentacleSpellCastingAbility;
            _gameFactory = gameFactory;
        }

   
        public async UniTask Cast()
        {
            Vector3 spawnPoint = _tentacleSpellCastingAbility.SpellSpawnPoint.transform.position;
            _minionsSpawnSpell = _gameFactory.CreateMinionsSpawnSpell(spawnPoint, Quaternion.identity);
            await _minionsSpawnSpell.Activate();
            Active = true;
        }

        public async UniTask Undo()
        {
            await _minionsSpawnSpell.Deactivate();
            Active = false;
        }
    }

    public enum TentacleSpell
    {
        PlayerCantUseHealthItems = 0,
        MinionsSpawnSpell = 1,
        Nothing = 100,
    }

    public class TentacleSpellCastingAbility : MonoBehaviour, IAbility
    {
        public bool CastingSpell { get; private set; }

        public Transform SpellSpawnPoint;

        private Dictionary<TentacleSpell, ITentacleSpell> _tentacleSpells;
        
        private List<TentacleSpell> _activeSpells = new();


        private TentacleConfiguration _tentacleConfiguration;
        private TentacleSettings _tentacleSettings;
        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IDataProvider dataProvider, IGameFactory gameFactory, IRuntimeData runtimeData)
        {
            _gameFactory = gameFactory;
            _tentacleConfiguration = dataProvider.TentacleConfig;
            _tentacleSettings = runtimeData.TentacleSettings;
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