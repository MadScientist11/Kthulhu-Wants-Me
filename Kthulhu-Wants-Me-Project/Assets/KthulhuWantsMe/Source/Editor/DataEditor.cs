using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace KthulhuWantsMe.Source.Editor
{
    public class DataEditor : OdinMenuEditorWindow
    {
        [MenuItem("Tools/Data Editor")]
        private static void OpenWindow()
        {
            GetWindow<DataEditor>().Show();
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree menuTree = new OdinMenuTree();
            menuTree.AddAllAssetsAtPath("Data", "Assets/KthulhuWantsMe/Data", true);
            return menuTree;
        }
    }
}