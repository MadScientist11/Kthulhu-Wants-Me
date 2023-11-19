using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.DamageSystem
{
 
    public interface IDamageProvider
    {
        Transform DamageDealer { get; }
        float ProvideDamage();
    }
}