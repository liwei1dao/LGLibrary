using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;

namespace LG.Editor
{
    [InitializeOnLoad]
    [SirenixEditorConfig]
    public class LGToolsWindow_Hose : GlobalConfig<LGToolsWindow_Hose>
    {
        [LabelText("项目名称")]
        public string ProjectName;
    }
}
