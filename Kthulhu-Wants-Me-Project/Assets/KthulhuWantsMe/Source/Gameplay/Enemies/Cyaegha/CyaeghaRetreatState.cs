using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha
{
    public class CyaeghaRetreatState : RetreatState
    {
        protected override void OnRetreated()
        {
            base.OnRetreat();
            Destroy(gameObject);
        }
        
        protected override void OnRetreatedDefeated()
        {
            base.OnRetreat();
            Destroy(gameObject);
        }
    }
}