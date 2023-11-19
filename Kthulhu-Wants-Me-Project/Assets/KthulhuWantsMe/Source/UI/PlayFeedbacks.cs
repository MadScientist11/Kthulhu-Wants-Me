using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace KthulhuWantsMe.Source.UI
{
    public class PlayFeedbacks : MonoBehaviour
    {
        [SerializeField] private MMFeedbacks _onEnableFeedback;
        [SerializeField] private MMFeedbacks _onDisableFeedback;

        private void OnEnable() => 
            _onEnableFeedback?.PlayFeedbacks();

        private void OnDisable() => 
            _onDisableFeedback?.PlayFeedbacks();
    }
}
