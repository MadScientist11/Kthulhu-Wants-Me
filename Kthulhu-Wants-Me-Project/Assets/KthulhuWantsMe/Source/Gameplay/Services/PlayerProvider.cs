using KthulhuWantsMe.Source.Gameplay.Player;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public interface IPlayerProvider
    {
        PlayerFacade Player { get; }
        void Set(PlayerFacade player);
    }

    public class PlayerProvider : IPlayerProvider
    {
        public PlayerFacade Player { get; private set; }

        public void Set(PlayerFacade player)
        {
            Player = player;
        }
    }
}