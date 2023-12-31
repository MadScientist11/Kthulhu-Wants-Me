﻿namespace KthulhuWantsMe.Source
{
    public static class GameConstants
    {
        public const float SpawnItemsElevation = 1f;
        
        public const string MenuPath = "KthulhuWantsMe/";
        public static class Layers
        {
            public const string PortalSpawnObstacle = "PortalSpawnObstacle";
            public const string Player = "Player";
            public const string PlayerRoll = "PlayerRoll";
            public const string Enemy = "Enemy";
            public const string Ground = "Ground";
            public const string FadeableObject = "FadeableObject";
        }
        public static class Scenes
        {
            public static string GameSceneName = "MainGame";
            
            public static string StartUpPath = "Assets/KthulhuWantsMe/Scenes/StartUp.unity";
            public static string GamePath = "Assets/KthulhuWantsMe/Scenes/Game.unity";
        }
    }
}