using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public class PlayerInventory 
    {
        public IReadOnlyList<IPickable> EquippedItems => _items;
        public IPickable CurrentItem => _items[_currentIndex];

        public event Action<IPickable> OnItemAdded;
        public event Action<IPickable> OnItemRemoved;
        public event Action<IPickable, IPickable> OnItemSwitched;
        public event Action<IPickable> OnCurrentItemChanged;

        private readonly IPickable[] _items = new IPickable[5];
        private int _currentIndex;

     
        public void ReplaceItem(IPickable item,Action<IPickable> onItemAdded, Action<IPickable> onItemRemoved)
        {
            if (_items[_currentIndex] == null)
            {
                _items[_currentIndex] = item;
                OnCurrentItemChanged?.Invoke(item);
                OnItemAdded?.Invoke(item);
                onItemAdded?.Invoke(item);
            }
            else
            {
                IPickable removedItem = _items[_currentIndex];
                RemoveItem(removedItem);
                onItemRemoved?.Invoke(removedItem);
                ReplaceItem(item, onItemAdded, onItemRemoved);
            }
        }


        public void RemoveItem(IPickable item)
        {
            int index = Array.IndexOf(_items, item);
            _items[index] = null;
            OnCurrentItemChanged?.Invoke(null);
            OnItemRemoved?.Invoke(item);
        }
        
        public void RemoveItemWithoutNotify(IPickable item)
        {
            int index = Array.IndexOf(_items, item);
            _items[index] = null;
        }

        public void SwitchItem(int index, Action<IPickable, IPickable> onItemSwitched)
        {
            int previous = _currentIndex;
            _currentIndex = index;
            onItemSwitched?.Invoke(_items[_currentIndex], _items[previous]);
            OnItemSwitched?.Invoke(_items[_currentIndex], _items[previous]);
            OnCurrentItemChanged?.Invoke(_items[_currentIndex]);

        }
    }
}