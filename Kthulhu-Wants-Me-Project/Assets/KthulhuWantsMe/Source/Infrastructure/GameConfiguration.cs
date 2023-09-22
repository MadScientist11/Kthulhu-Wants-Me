using KthulhuWantsMe.Source.Gameplay.Locations;
using NaughtyAttributes;
using UnityEngine;

namespace KthulhuWantsMe.Source.Infrastructure.Scopes
{
    [CreateAssetMenu(menuName = "Create GameConfiguration", fileName = "GameConfiguration", order = 0)]
    public class GameConfiguration : ScriptableObject
    {
        [Scene] public string MainScene;
        public LocationId LocationId;

    }
}