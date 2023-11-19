namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem
{
    public enum EffectId
    {
        Unknown = -1,
        FireEffect = 0,
        PoisonEffect = 50,
    }
    public interface IBuffDebuff
    {
        EffectId EffectId { get; }
        void ApplyEffect(IEffectReceiver effectReceiver);
    }
}