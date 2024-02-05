using Sirenix.OdinInspector.Editor;
using System.Linq;
using UnityEngine;
using Sirenix.Utilities.Editor;
using Sirenix.Serialization;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.OdinInspector.Demos;

namespace LG
{
    public class LGToolsWindow : OdinMenuEditorWindow
    {
        [MenuItem("Tools/LGTools")]
        public static void OpenWindow()
        {
            var window = GetWindow<LGToolsWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }
        protected override OdinMenuTree BuildMenuTree() {
            OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true)
            {
                { "Project",LGToolsWindow_Hose.Instance},
                { "Build",new LGToolsWindow_Build()},
            };
            tree.SortMenuItemsByName();
            return tree;

        }
    }

}