using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LG
{
    [InitializeOnLoad]
    [SirenixEditorConfig]
    public class LGToolsWindow_Hose : GlobalConfig<LGToolsWindow_Hose>
    {
        [LabelText("ÏîÄ¿Ãû³Æ")]
        public string ProjectName;
    }
}
