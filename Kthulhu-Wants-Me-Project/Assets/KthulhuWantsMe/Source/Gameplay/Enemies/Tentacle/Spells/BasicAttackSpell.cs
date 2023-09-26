using Cysharp.Threading.Tasks;
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


        private float _spellEffectiveRadius = 3f;
        private float _spellCastCooldown = 3f;

        
        private readonly PlayerFacade _player;
        private readonly ThePlayer _playerModel;
        private readonly TentacleSpellCastingAbility _spellCastingAbility;
        private SpellConfiguration _spellConfiguration;

        public BasicAttackSpell(TentacleSpellCastingAbility spellCastingAbility, SpellConfiguration spellConfiguration,
            PlayerFacade player, ThePlayer playerModel)
        {
            _spellConfiguration = spellConfiguration;
            _spellCastingAbility = spellCastingAbility;
            _playerModel = playerModel;
            _player = player;
        }

        public async UniTask Cast()
        {
            InCooldown = true;
            
            _spellCastingAbility.CastingSpell = true;
            Vector3 spellCastPosition = _player.transform.position;
            _spellInstance = Object.Instantiate(_spellConfiguration.Prefab, spellCastPosition, Quaternion.identity);
            Active = true;
            await UniTask.Delay(2000);
            _spellCastingAbility.CastingSpell = false;
            
            if (Vector3.Distance(spellCastPosition, _player.transform.position) < _spellConfiguration.EffectiveRange)
            {
                _playerModel.TakeDamage(new Damage(15f));
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