using System.Collections.Generic;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public interface IDamageSource
    {
        Transform DamageSourceObject { get; }
    }
}