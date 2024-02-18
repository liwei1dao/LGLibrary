using Sirenix.OdinInspector.Editor;
using System.Linq;
using UnityEngine;
using Sirenix.Utilities.Editor;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
using System.IO;
using System;
using ProtoBuf.Reflection;
using Google.Protobuf.Reflection;


namespace LG.Editor
{
    public class ProtoToolWindow : OdinEditorWindow
    {


        [MenuItem("Tools/ProtoTool")]
        public static void OpenWindow()
        {
            var window = GetWindow<ProtoToolWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
            //使用当前类初始化
            window._serializedObject = new SerializedObject(window);
            //获取当前类中可序列话的属性
            window._fileListProperty = window._serializedObject.FindProperty("_fileList");
        }

        [HideInInspector]
        public SerializedObject _serializedObject;
        [HideInInspector]
        public SerializedProperty _fileListProperty;

        [LabelText("pb文件目录地址")]
        [HorizontalGroup("PbPath")]
        public string PbPath = string.Empty;
        [HorizontalGroup("PbPath")]
        [Button("选择目标", ButtonSizes.Small)]
        private void SelectPbPath()
        {
            string NewPath = EditorUtility.OpenFolderPanel("选择资源目录", Application.dataPath, "");
            PbPath = NewPath;
            if (PbPath != string.Empty)
            {
                _fileList = Directory.GetFiles(PbPath,
                    "*.proto", SearchOption.AllDirectories);
                var _fileListInstance = new string[_fileList.Length];
                for (int i = 0; i < _fileList.Length; i++)
                    _fileListInstance[i] = MakeRelativePath(PbPath, _fileList[i]);
                if (_fileListInstance.Length > 0)
                {
                    _fileList = _fileListInstance;
                    _serializedObject.Update();
                }
            }
            return;
        }
        [LabelText("输出目录")]
        [HorizontalGroup("OutPath"), ReadOnly]
        public string OutPath = Application.dataPath + "/Plugins/LGLibrary/Modules/AppServiceModule/pb/";

        [ShowInInspector, LabelText("文件列表")]
        protected string[] _fileList = new string[0];
        public static string MakeRelativePath(string fromPath, string toPath)
        {
            if (String.IsNullOrEmpty(fromPath)) throw new ArgumentNullException(nameof(fromPath));
            if (String.IsNullOrEmpty(toPath)) throw new ArgumentNullException(nameof(toPath));
            // make sure there is a trailing '/', else Uri.MakeRelativeUri won't work as expected
            char lastChar = fromPath[fromPath.Length - 1];
            if (lastChar != Path.DirectorySeparatorChar && lastChar != Path.AltDirectorySeparatorChar)
                fromPath += Path.DirectorySeparatorChar;

            Uri fromUri = new Uri(fromPath, UriKind.RelativeOrAbsolute);
            if (!fromUri.IsAbsoluteUri)
            {
                fromUri = new Uri(Path.Combine(Directory.GetCurrentDirectory(), fromPath));
            }
            Uri toUri = new Uri(toPath, UriKind.RelativeOrAbsolute);
            if (!toUri.IsAbsoluteUri)
            {
                toUri = new Uri(Path.Combine(Directory.GetCurrentDirectory(), toPath));
            }

            if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.OrdinalIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }

        private bool IsCanBuld
        {
            get
            {
                return PbPath != string.Empty && OutPath != string.Empty && _fileList.Length > 0;
            }
        }

        [Button("编译文件", ButtonSizes.Large), ShowIf("IsCanBuld")]
        private void BuildPbFile()
        {
            Generate(PbPath, _fileList, Application.dataPath + OutPath);
        }

        private static void Generate(string inpath, string[] inprotos, string outpath)
        {
            if (!Directory.Exists(outpath))
            {
                Directory.CreateDirectory(outpath);
            }

            var set = new FileDescriptorSet();

            set.AddImportPath(inpath);
            foreach (var inproto in inprotos)
            {
                var s = inproto;
                if (!inproto.Contains(".proto"))
                {
                    s += ".proto";
                }

                set.Add(s, true);
            }

            set.Process();
            var errors = set.GetErrors();
            if (errors != null && errors.Length > 0)
            {
                foreach (var error in errors)
                {
                    Debug.LogError(error);
                }

                return;
            }
            var files = CSharpCodeGenerator.Default.Generate(set);

            foreach (var file in files)
            {
                var path = Path.Combine(outpath, file.Name);
                var absolutePath = Path.GetFullPath(path);
                var absoluteOutPath = Path.GetDirectoryName(absolutePath);
                if (!Directory.Exists(absoluteOutPath))
                {
                    Directory.CreateDirectory(absoluteOutPath);
                }
                File.WriteAllText(path, file.Text);

                Debug.Log($"Generated cs file for {file.Name.Replace(".cs", ".proto")} successfully to: {path}");
            }

            EditorUtility.DisplayDialog("Complete",
                "Proto文件已转CS，详细请看控制台输出" +
                "\n" +
                "Proto files has been convert into CS files, please go to console and view details",
                "Close window");
        }
    }
}