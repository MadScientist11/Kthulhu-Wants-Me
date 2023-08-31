using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public interface IInventorySystem
    {
        event Action<IPickable> OnItemAdded;
        event Action<IPickable> OnItemRemoved;
        event Action<IPickable, IPickable> OnItemSwitched;
        IPickable CurrentItem { get; }
        void ReplaceItem(IPickable item);
        void RemoveItem(IPickable item);
        void RemoveItemWithoutNotify(IPickable item);
        void ReplaceItem(IPickable item,Action<IPickable> onItemAdded, Action<IPickable> onItemRemoved);
        void SwitchItem(int index, Action<IPickable, IPickable> onItemSwitched);
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

     
        public void ReplaceItem(IPickable item)
        {
            Debug.Log($"Adding item {item}");
            if (_items[_currentIndex] == null)
            {
                Debug.Log($"Was no item so added {item}");
                _items[_currentIndex] = item;
                item.Equipped = true;
                OnItemAdded?.Invoke(item);
            }
            else
            {
                IPickable removedItem = _items[_currentIndex];
                RemoveItem(removedItem);
                ReplaceItem(item);
            }
        }
        
        public void ReplaceItem(IPickable item,Action<IPickable> onItemAdded, Action<IPickable> onItemRemoved)
        {
            Debug.Log($"Adding item {item}");
            if (_items[_currentIndex] == null)
            {
                Debug.Log($"Was no item so added {item}");
                _items[_currentIndex] = item;
                item.Equipped = true;
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
            item.Equipped = false;
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