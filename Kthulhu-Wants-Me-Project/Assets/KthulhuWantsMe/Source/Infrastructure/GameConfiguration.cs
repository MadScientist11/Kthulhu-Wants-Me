﻿using UnityEngine;

namespace KthulhuWantsMe.Source.Infrastructure.Scopes
{
    [CreateAssetMenu(menuName = "Create GameConfiguration", fileName = "GameConfiguration", order = 0)]
    public class GameConfiguration : ScriptableObject
    {
        public string UIScene;
        public string MainScene;
    }
}