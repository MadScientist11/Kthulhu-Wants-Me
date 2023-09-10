namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem
{
    public interface IBuffDebuffService
    {
        void ApplyEffect(IBuffDebuff effect, IEffectReceiver to);
    }

    public class BuffDebuffService : IBuffDebuffService
    {
        public BuffDebuffService()
        {
        }


        public void ApplyEffect(IBuffDebuff effect, IEffectReceiver to)
        {
            effect.ApplyEffect(to);
        }
    }
}