using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces.AutoInteractables;
using KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities;
using KthulhuWantsMe.Source.Gameplay.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{

    public abstract class BuffItem : MonoBehaviour, IAutoInteractable
    {
        protected IBuffDebuffFactory EffectFactory;

        [Inject]
        public void Construct(IBuffDebuffFactory effectFactory) 
            => EffectFactory = effectFactory;

        protected abstract IBuffDebuff ProvideBuff();
       
        public void RespondTo(PlayerInteractionAbility ability)
        {
            if(ProvideBuff() == null)
                return;
            
            ability.ApplyEffectToPlayer(ProvideBuff());
            Destroy(gameObject);
        }
    }
}