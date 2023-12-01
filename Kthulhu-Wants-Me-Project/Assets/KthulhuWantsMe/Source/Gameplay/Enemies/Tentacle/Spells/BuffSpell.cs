using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem.BuffsDebuffs;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.Spells
{
    public class BuffSpell : ITentacleSpell
    {
       public bool Active { get; private set; }
        public bool InCooldown { get; private set; }

        
        private GameObject _spellInstance;


        private float _spellEffectiveRadius = 3f;
        private float _spellCastCooldown = 3f;

        
        private readonly PlayerFacade _player;
        private readonly ThePlayer _playerModel;
        private readonly TentacleSpellCastingAbility _spellCastingAbility;
        private readonly SpellConfiguration _spellConfiguration;
        private IBuffDebuffFactory _buffDebuffFactory;

        public BuffSpell(TentacleSpellCastingAbility spellCastingAbility, SpellConfiguration spellConfiguration, IBuffDebuffFactory buffDebuffFactory)
        {
            _buffDebuffFactory = buffDebuffFactory;
            _spellConfiguration = spellConfiguration;
            _spellCastingAbility = spellCastingAbility;
        }

        public async UniTask Cast()
        {
            InCooldown = true;
            
            _spellCastingAbility.CastingSpell = true;
            Vector3 spellCastPosition = _spellCastingAbility.transform.position;
            _spellInstance = Object.Instantiate(_spellConfiguration.Prefab, spellCastPosition, Quaternion.identity);
            Active = true;
            await UniTask.Delay(Mathf.FloorToInt(_spellConfiguration.ActivationTime * 1000));
            _spellCastingAbility.CastingSpell = false;

            if (PhysicsUtility.HitMany(spellCastPosition, _spellEffectiveRadius,
                    LayerMasks.EnemyMask, out List<EnemyStatsContainer> enemies))
            {
                foreach (EnemyStatsContainer enemy in enemies)
                {

                    if (enemy.TryGetComponent(out EntityBuffDebuffContainer container))
                    {
                        InstaHealBuff instaHealBuff = _buffDebuffFactory.CreateEffect<InstaHealBuff>().Init(16);
                        instaHealBuff.ApplyEffect(container);
                    }
                }
            }

            _spellCastingAbility.CancelSpell(TentacleSpell.Buff).Forget();
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