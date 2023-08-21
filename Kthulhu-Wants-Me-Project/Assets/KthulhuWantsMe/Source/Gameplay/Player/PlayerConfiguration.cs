using KthulhuWantsMe.Source.Gameplay.Player;
using UnityEngine;

namespace KthulhuWantsMe.Source.Player
{
    [CreateAssetMenu(menuName = "KhtulhuWantsMe/PlayerConfiguration", fileName = "PlayerConfiguration", order = 0)]
    public class PlayerConfiguration : ScriptableObject
    {
        public PlayerFacade PlayerPrefab;
    }
}