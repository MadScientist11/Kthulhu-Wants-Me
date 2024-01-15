using System.Collections;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Interactables.Weapons.Claymore;
using KthulhuWantsMe.Source.Gameplay.Player.AttackSystem;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.SkillTreeSystem;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using MoreMountains.Feedbacks;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities
{
    public class PlayerSpecialAttackAbility : MonoBehaviour, IAbility
    {
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerAttack _playerAttack;
        [SerializeField] private MMFeedbacks _specialAttackFeedback;

        private float _attackCooldown = 0.6f;
        
        private WeaponItem _currentWeapon;

        private IInputService _inputService;
        private ThePlayer _player;

        [Inject]
        public void Construct(IInputService inputService, ThePlayer player)
        {
            _player = player;
            _inputService = inputService;

            _inputService.GameplayScenario.SpecialAttack += PerformSpecialAttack;
        }

        private void OnDestroy()
        {
            _inputService.GameplayScenario.SpecialAttack -= PerformSpecialAttack;
        }

        private void OnSpecialAttack()
        {
            _currentWeapon.GetComponent<ISpecialAttack>().RespondTo(this);
        }
 

        private void PerformSpecialAttack()
        {
            if(GetComponent<PlayerLungeAbility>().IsInLunge 
               || !_player.PlayerStats.AcquiredSkills.Contains(SkillId.SwordSpecialAttack) 
               || _playerAnimator.CurrentState == AnimatorState.SpecialAttack
               || _playerAttack.IsAttacking)
                return;
            
            if (_player.Inventory.CurrentItem is WeaponItem weapon)
            {
                _currentWeapon = weapon;
                PlayerLocomotion playerLocomotion = GetComponent<PlayerLocomotion>();
                playerLocomotion.BlockMovement(0.5f);
                playerLocomotion.StopToAttack();
                playerLocomotion.FaceMouse();
                StartCoroutine(DoDisableInput());
                _playerAnimator.PlaySpecialAttack();
                _playerAttack.ResetAttackState();
                _specialAttackFeedback?.PlayFeedbacks();
            }
        }

        private IEnumerator DoDisableInput()
        {
            _inputService.GameplayScenario.Disable();
            yield return new WaitForSeconds(.5f);
            _inputService.GameplayScenario.Enable();
        }
    }
}