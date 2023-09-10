using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem
{
    public class EntityBuffDebuffContainer : MonoBehaviour, IEffectReceiver
    {
        public Transform Transform => transform;
        public IReadOnlyList<IBuffDebuff> Effects  => _buffDebuffService.ActiveEffectReceivers[this];

        private IBuffDebuffService _buffDebuffService;
        
        [Inject]
        public void Construct(IBuffDebuffService buffDebuffService) => 
            _buffDebuffService = buffDebuffService;
    }
}