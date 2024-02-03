using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LG
{

    public class LGTools : EditorWindow
    {
        [MenuItem("Tools/LGTools")]
        public static void CreateView()
        {
            LGTools tools = GetWindowWithRect<LGTools>(new Rect(100, 100, 600, 400), false, "LGTools");
            tools.Init();
        }

        public void Init()
        {

        }
    }
}