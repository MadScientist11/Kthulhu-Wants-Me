using KthulhuWantsMe.Source.Gameplay.Enemies;

namespace KthulhuWantsMe.Source.Gameplay
{
    public static class EnemiesExtensions
    {
        public static bool OccupiesSpawner(this EnemyType enemyType)
        {
            return enemyType.IsTentacleVariant();
        }
        
        public static bool IsTentacleVariant(this EnemyType enemyType)
        {
            return enemyType == EnemyType.Tentacle || enemyType == EnemyType.BleedTentacle ||
                   enemyType == EnemyType.PoisonousTentacle || enemyType == EnemyType.TentacleSpecial;
        }
    }
}