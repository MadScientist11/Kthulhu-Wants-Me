using System;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.PortalsLogic
{
    public class Portal : MonoBehaviour, IPoolable<Portal>
    {
        public Action<Portal> Release { get; set; }
        
        public void Show() =>
            gameObject.SwitchOn();

        public void Hide() =>
            gameObject.SwitchOff();


        public void ClosePortal()
        {
            Release?.Invoke(this);
            //_spawnedTentacle.gameObject.SwitchOff();
        }
    }
}