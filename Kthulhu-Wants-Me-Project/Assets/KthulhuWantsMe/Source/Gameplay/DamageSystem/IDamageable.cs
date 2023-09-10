namespace KthulhuWantsMe.Source.Gameplay.DamageSystem
{
    public interface IDamageable
    {
        void TakeDamage(float damage);
        void TakeDamage(float damage, IDamageProvider damageProvider)
            => TakeDamage(damage);
    }
}