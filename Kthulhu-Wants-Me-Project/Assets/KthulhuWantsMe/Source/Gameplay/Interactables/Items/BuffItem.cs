using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces.AutoInteractables;
using KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public class BuffItem : MonoBehaviour, IBuff, IAutoInteractable
    {
        [SerializeField] private BuffData _buffData;
        
        public BuffTarget BuffTarget => _buffData.BuffTarget;
        public BuffType BuffType => _buffData.BuffType;
        public float Value => _buffData.Value;
        

        public void Init(BuffData buffData)
        {
            _buffData = buffData;
        }

        public void RespondTo(PlayerInteractionAbility ability)
        {
            Destroy(gameObject);
        }
    }
}