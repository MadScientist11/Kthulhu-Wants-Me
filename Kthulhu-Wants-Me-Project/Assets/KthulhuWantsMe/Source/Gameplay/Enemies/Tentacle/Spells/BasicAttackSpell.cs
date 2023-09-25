using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.Spells
{
    public class BasicAttackSpell : ITentacleSpell
    {
        public bool Active { get; private set; }
        
        private const string TentacleBasicAttackSpell = "TentacleBasicAttackSpell";
        
        private GameObject _spellInstance;
        private GameObject _spellPrefab;


        private float _spellEffectiveRadius = 3f;
        
        private readonly PlayerFacade _player;
        private readonly ThePlayer _playerModel;
        private TentacleSpellCastingAbility _spellCastingAbility;

        public BasicAttackSpell(TentacleSpellCastingAbility spellCastingAbility,PlayerFacade player, ThePlayer playerModel)
        {
            _spellCastingAbility = spellCastingAbility;
            _playerModel = playerModel;
            _player = player;
        }

        public async void Init()
        {
            _spellPrefab = (GameObject)await Resources.LoadAsync(TentacleBasicAttackSpell);

        }
       
        public async UniTask Cast()
        {
            _spellCastingAbility.CastingSpell = true;
            Vector3 spellCastPosition = _player.transform.position;
            _spellInstance = Object.Instantiate(_spellPrefab, spellCastPosition, Quaternion.identity);
            Active = true;
            await UniTask.Delay(2000);
            _spellCastingAbility.CastingSpell = false;
            
            if (Vector3.Distance(spellCastPosition, _player.transform.position) < _spellEffectiveRadius)
            {
                _playerModel.TakeDamage(new Damage(15f));
            }

            _spellCastingAbility.CancelSpell(TentacleSpell.BasicAttackSpell).Forget();

        }

        public UniTask Undo()
        {
            Object.Destroy(_spellInstance);
            Active = false;
            return UniTask.CompletedTask;
        }
    }
}