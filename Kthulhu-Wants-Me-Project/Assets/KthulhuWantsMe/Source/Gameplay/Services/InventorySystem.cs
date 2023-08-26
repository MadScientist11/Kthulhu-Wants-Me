using System;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Items;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public interface IInventorySystem
    {
        event Action<ItemBase> OnItemAdded;
        event Action<ItemBase> OnItemRemoved;
        event Action<ItemBase, ItemBase> OnItemSwitched;
        void AddItem(ItemBase item);
    }

    public class InventorySystem : IInventorySystem, IInitializableService, IDisposable
    {
        public bool IsInitialized { get; set; }
        public ItemBase[] EquippedItems => _items;
        
        public event Action<ItemBase> OnItemAdded;
        public event Action<ItemBase> OnItemRemoved;
        public event Action<ItemBase, ItemBase> OnItemSwitched;

        private readonly ItemBase[] _items = new ItemBase[5];
        private int _currentIndex;

        private readonly IInputService _inputService;


        public InventorySystem(IInputService inputService)
        {
            _inputService = inputService;
        }

        public UniTask Initialize()
        {
            _inputService.GameplayScenario.SwitchItem += SwitchItem;
            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            _inputService.GameplayScenario.SwitchItem -= SwitchItem;
        }

        public void AddItem(ItemBase item)
        {
            Debug.Log($"Adding item {item}");
            if (_items[_currentIndex] == null)
            {
                Debug.Log($"Was no item so added {item}");

                _items[_currentIndex] = item;
                OnItemAdded?.Invoke(item);
            }
            else
            {
                ItemBase removedItem = _items[_currentIndex];
                _items[_currentIndex] = null;
                OnItemRemoved?.Invoke(removedItem);
                AddItem(item);
            }
        }

        private void SwitchItem(int index)
        {
            int previous = _currentIndex;
            _currentIndex = index;
            OnItemSwitched?.Invoke(_items[_currentIndex], _items[previous]);
        }
    }
}