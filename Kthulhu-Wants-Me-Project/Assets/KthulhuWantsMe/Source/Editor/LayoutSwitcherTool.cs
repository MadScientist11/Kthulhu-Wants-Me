
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEditorInternal;
using UnityEngine;

namespace KthulhuWantsMe.Source.Editor
{
    public static class LayoutSwitcherTool
    {
        [Shortcut("Tools/Layouts/Default", KeyCode.Alpha1, ShortcutModifiers.Alt)]
        public static void DefaultLayoutMenuItem()
        {
            OpenLayout("NDefault");
        }
        
        [Shortcut("Tools/Layouts/Animations", KeyCode.Alpha2, ShortcutModifiers.Alt)]
        public static void AnimationsLayoutMenuItem()
        {
            OpenLayout("Animations");
        }
        [Shortcut("Tools/Layouts/2 by 3", KeyCode.Alpha3, ShortcutModifiers.Alt)]
        public static void TwoByThreeLayoutMenuItem()
        {
            OpenLayout("2 by 3");
        }
        
        
        private static string GetWindowLayoutPath(string name)
        {

            string layoutsPreferencesPath = Path.Combine(InternalEditorUtility.unityPreferencesFolder, "Layouts");
            string layoutsModePreferencesPath = Path.Combine(layoutsPreferencesPath, ModeService.currentId);

            if (Directory.Exists(layoutsModePreferencesPath))
            {
                string[] layoutPaths = Directory.GetFiles(layoutsModePreferencesPath)
                    .Where(path => path.EndsWith(".wlt")).ToArray();

                foreach (string layoutPath in layoutPaths)
                {
                    if (String.CompareOrdinal(name, Path.GetFileNameWithoutExtension(layoutPath)) == 0)
                    {
                        return layoutPath;
                    }
                }
                
                
                
            }
            
            return null;
        }

        private static bool OpenLayout(string name)
        {
            string path = GetWindowLayoutPath(name);
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            Type windowLayoutType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.WindowLayout");

            if (windowLayoutType != null)
            {
                MethodInfo tryLoadWindowLayoutMethod = windowLayoutType.GetMethod("LoadWindowLayout", 
                    BindingFlags.Public | BindingFlags.Static,
                    null,
                    new Type[] { typeof(string), typeof(bool) },
                    null);

                if (tryLoadWindowLayoutMethod != null)
                {
                    object[] args = new object[] { path, false };
                    bool result = (bool)tryLoadWindowLayoutMethod.Invoke(null, args);
                    return result;
                }
            }

            return false;
        }
    }
}