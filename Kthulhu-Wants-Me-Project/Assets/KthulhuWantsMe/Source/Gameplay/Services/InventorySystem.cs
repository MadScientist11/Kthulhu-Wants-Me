using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public interface IInventorySystem
    {
        event Action<IPickable> OnItemAdded;
        event Action<IPickable> OnItemRemoved;
        event Action<IPickable, IPickable> OnItemSwitched;
        IPickable CurrentItem { get; }
        void ReplaceItem(IPickable item,Action<IPickable> onItemAdded, Action<IPickable> onItemRemoved);
        void SwitchItem(int index, Action<IPickable, IPickable> onItemSwitched);
        void RemoveItem(IPickable item);
    }

    public class InventorySystem : IInventorySystem
    {
        public bool IsInitialized { get; set; }
        public IReadOnlyList<IPickable> EquippedItems => _items;
        public IPickable CurrentItem => _items[_currentIndex];

        public event Action<IPickable> OnItemAdded;
        public event Action<IPickable> OnItemRemoved;
        public event Action<IPickable, IPickable> OnItemSwitched;

        private readonly IPickable[] _items = new IPickable[5];
        private int _currentIndex;

     
        public void ReplaceItem(IPickable item,Action<IPickable> onItemAdded, Action<IPickable> onItemRemoved)
        {
            if (_items[_currentIndex] == null)
            {
                _items[_currentIndex] = item;
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
        }
    }
}