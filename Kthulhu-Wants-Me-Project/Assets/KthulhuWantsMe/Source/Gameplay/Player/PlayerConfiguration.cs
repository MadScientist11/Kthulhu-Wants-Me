using UnityEngine;

namespace KthulhuWantsMe.Source.Player
{
    [CreateAssetMenu(menuName = "KhtulhuWantsMe/PlayerConfiguration", fileName = "PlayerConfiguration", order = 0)]
    public class PlayerConfiguration : ScriptableObject
    {
        public GameObject PlayerPrefab;
    }
}