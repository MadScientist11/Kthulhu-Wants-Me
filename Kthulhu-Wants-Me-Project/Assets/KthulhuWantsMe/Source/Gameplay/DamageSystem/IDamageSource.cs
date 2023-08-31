using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.DamageSystem
{
    public interface IDamageSource
    {
        Transform DamageSourceObject { get; }
    }
}