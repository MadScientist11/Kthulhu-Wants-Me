using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.AI
{
    [CreateAssetMenu(menuName = "Create GlobalAIConfiguration", fileName = "GlobalAIConfiguration", order = 0)]
    public class GlobalAIConfiguration : ScriptableObject
    {
        public int MaxChasingEnemies = 15;
        public double EnemiesFallBehindDistance = 10;
        public float AIBehaviourUpdateRate = 2f;
    }
}