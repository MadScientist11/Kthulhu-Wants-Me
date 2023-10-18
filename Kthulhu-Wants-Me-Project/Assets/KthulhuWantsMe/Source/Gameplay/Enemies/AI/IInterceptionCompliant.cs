using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.AI
{
    public interface IInterceptionCompliant
    {
        Vector3 AverageVelocity { get; }
    }
}