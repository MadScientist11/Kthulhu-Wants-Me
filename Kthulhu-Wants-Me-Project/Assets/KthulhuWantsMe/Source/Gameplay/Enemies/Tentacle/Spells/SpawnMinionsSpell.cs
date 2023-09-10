using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Spell;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.Spells
{
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
}