using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.Player;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle.Spells
{
    public class ProhibitHealthItemsUsageSpell : ITentacleSpell
    {
        public bool Active { get; private set; }
        public bool InCooldown { get; } 
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
}