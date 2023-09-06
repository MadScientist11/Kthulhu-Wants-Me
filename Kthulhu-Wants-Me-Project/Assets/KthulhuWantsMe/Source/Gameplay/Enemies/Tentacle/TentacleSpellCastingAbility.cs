using KthulhuWantsMe.Source.Gameplay.Spell;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleSpellCastingAbility : MonoBehaviour
    {
        public bool MinionsSpawnSpellActive { get; private set; }
        
        [SerializeField] private Transform _spellSpawnPoint;
        
        private MinionsSpawnSpell _minionsSpawnSpell;
        
        
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

        public void CastMinionsSpawnSpell()
        {
            if(!_tentacleSettings.AllowSpellCasting)
                return;
            
            _minionsSpawnSpell = _gameFactory.CreateMinionsSpawnSpell(_spellSpawnPoint.position, Quaternion.identity);
            _minionsSpawnSpell.Activate();
            MinionsSpawnSpellActive = true;
        }

        public void CancelSpell()
        {
            if (_minionsSpawnSpell != null)
            {
                Destroy(_minionsSpawnSpell.gameObject);
            }
        }
    }
}