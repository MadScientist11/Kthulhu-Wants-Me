using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem
{
    [CreateAssetMenu(menuName = GameConstants.MenuPath + "Create BuffItemsContainer", fileName = "BuffItemsContainer", order = 0)]
    public class BuffItemsContainer : ScriptableObject
    {
        public List<BuffItem> BuffItems;
        
        public TBuffItem Get<TBuffItem>() where TBuffItem : BuffItem
        {
           return (TBuffItem)BuffItems.First(x => x is TBuffItem);
        }
    }
}