using System;
using KthulhuWantsMe.Source.UI;
using UnityEngine;
using UnityEngine.UI;

namespace KthulhuWantsMe.Source.Gameplay
{
    public class SkillImageUpdater : MonoBehaviour
    {
        public Sprite[] SkillImages;

        
        private void Update()
        {
            BranchView[] views = FindObjectsByType<BranchView>(FindObjectsSortMode.InstanceID);


            if (views.Length > 0)
            {
                for (var i = 0; i < views.Length; i++)
                {
                    var branchView = views[i];
                    Debug.Log(branchView.transform.GetChild(2).name, branchView.transform.GetChild(2));
                    Image skillImage = branchView.transform.GetChild(2).GetComponent<UnityEngine.UI.Image>();

                    skillImage.sprite = SkillImages[i];
                }
            }
        }
    }
}