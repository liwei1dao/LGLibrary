using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LG
{
    //打包选项
    public enum BuildSwitchType
    {
        OnlyApp,                                        //仅编译App
        OnlyModule,                                     //仅编译模块
        AppAndModule,                                   //APP和模块一起编译
    }
    public enum ResourceBuildNodelType
    {
        UselessNodel,            //无用文件
        ModelNodel,              //模块节点
        FolderNodel,             //文件夹节点
        ResourcesItemNodel,      //资源文件节点
    }
    [System.Serializable]
    public class ResourceCatalog
    {
        [HideInInspector]
        public string Name;
        [HideInInspector]
        public string Path;
    }
    [System.Serializable]
    public class ResourceModelConfig : ResourceItemConfig
    {
        [HideInInspector]
        public bool IsSelection = true;
        [HideInInspector]
        public bool IsUpdataModule = false;
        public ResourceModelConfig(string _RootName, string _Path)
            : base(_RootName, null, _Path)
        {
            if (NodelType == ResourceBuildNodelType.FolderNodel)
            {
                NodelType = ResourceBuildNodelType.ModelNodel;
                AssetBundleName = Name;
                ModelName = Name;
                IsMergeBuild = false;
                RefreshChildViewGUI();
            }
        }

        public void MergeConfig(ResourceModelConfig other)
        {
            for (int i = 0; i < other.ChildBuildItem.Count; i++)
            {
                ResourceItemConfig item1 = other.ChildBuildItem[i];
                bool IsKeep = false;
                for (int j = 0; j < ChildBuildItem.Count; j++)
                {
                    ResourceItemConfig item2 = ChildBuildItem[j];
                    if (item1.Name == item2.Name)
                    {
                        if (item2.NodelType == ResourceBuildNodelType.FolderNodel)
                        {
                            item2.MergeConfig(item1);
                        }
                        else
                        {
                            ChildBuildItem[j] = item1;
                        }
                        IsKeep = true;
                        break;
                    }
                }
                if (!IsKeep)
                {
                    ChildBuildItem.Add(item1);
                }
            }
        }

        public override void OnGUI()
        {
#if UNITY_EDITOR
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            EditorGUI.indentLevel = Layer;
            IsSelection = EditorGUILayout.Toggle(IsSelection, GUILayout.Width(10));
            Foldout = EditorGUILayout.Foldout(Foldout, Name);
            EditorGUILayout.Space();
            IsUpdataModule = GUILayout.Toggle(IsUpdataModule, "IsUpData");
            EditorGUILayout.EndHorizontal();
            if (Foldout)
            {
                foreach (var item in ChildBuildItem)
                {
                    item.OnGUI();
                }
            }
#endif
        }
    }
    [System.Serializable]
    public class ResourceItemConfig
    {
        [HideInInspector]
        public string Name;
        [HideInInspector]
        public string Path;
        [HideInInspector]
        public string RootName;
        [HideInInspector]
        public string ModelName;
        [HideInInspector]
        public string AssetBundleName;
        [HideInInspector]
        public int Layer;
        [HideInInspector]
        public ResourceBuildNodelType NodelType;
        [HideInInspector]
        public bool Foldout = false;
        [HideInInspector]
        public bool IsMergeBuild = true;
        [HideInInspector]
        public bool IsShowBuildToogle = true;
        [HideInInspector]
        public List<ResourceItemConfig> ChildBuildItem;

        public ResourceItemConfig(string _RootName, ResourceItemConfig _Parent, string _Path)
        {
            RootName = _RootName;
            Path = _Path;
            Name = StringExtend.GetPathFolderName(Path);
            ModelName = _Parent == null ? Name : _Parent.ModelName;
            Layer = _Parent == null ? 0 : _Parent.Layer + 1;
            ChildBuildItem = new List<ResourceItemConfig>();
            if (StringExtend.IsDirectory(Application.dataPath + Path))
            {
                string[] fileList = Directory.GetFileSystemEntries(Application.dataPath + Path);
                foreach (string file in fileList)
                {
                    string _file = file.Substring(Application.dataPath.Length, file.Length - Application.dataPath.Length);
                    ResourceItemConfig Item = new ResourceItemConfig(RootName, this, _file);
                    if (Item.NodelType != ResourceBuildNodelType.UselessNodel)
                    {
                        ChildBuildItem.Add(Item);
                    }
                }
                if (ChildBuildItem.Count > 0)
                {
                    NodelType = ResourceBuildNodelType.FolderNodel;
                }
                else
                {
                    NodelType = ResourceBuildNodelType.UselessNodel;
                }
            }
            else
            {
                if (StringExtend.CheckSuffix(Path, ToolsConfig.CanBuildFileTypes))
                {
                    Name = Name.Substring(0, Name.IndexOf("."));
                    NodelType = ResourceBuildNodelType.ResourcesItemNodel;
                }
                else
                {
                    NodelType = ResourceBuildNodelType.UselessNodel;
                }
            }
        }

        /// <param name="_Other"></param>
        public void MergeConfig(ResourceItemConfig other)
        {
            for (int i = 0; i < other.ChildBuildItem.Count; i++)
            {
                ResourceItemConfig item1 = other.ChildBuildItem[i];
                bool IsKeep = false;
                for (int j = 0; j < ChildBuildItem.Count; j++)
                {
                    ResourceItemConfig item2 = ChildBuildItem[j];
                    if (item1.Name == item2.Name)
                    {
                        if (item2.NodelType == ResourceBuildNodelType.FolderNodel)
                        {
                            item2.MergeConfig(item1);
                        }
                        else
                        {
                            ChildBuildItem[j] = item1;      //覆盖
                        }
                        IsKeep = true;
                        break;
                    }
                }
                if (!IsKeep)
                {
                    ChildBuildItem.Add(item1);
                }
            }
        }

        /// <summary>
        /// 拆分资源列表
        /// </summary>
        /// <param name="RootName"></param>
        public void SplitConfig(string RootName)
        {
            List<ResourceItemConfig> Tmp = new List<ResourceItemConfig>();
            foreach (var item in ChildBuildItem)
            {
                if (item.NodelType == ResourceBuildNodelType.ResourcesItemNodel && item.RootName == RootName)
                {
                    Tmp.Add(item);
                }
                else
                {
                    if (item.NodelType == ResourceBuildNodelType.FolderNodel || item.NodelType == ResourceBuildNodelType.ModelNodel)
                    {
                        item.SplitConfig(RootName);
                        if (item.NodelType == ResourceBuildNodelType.UselessNodel)
                        {
                            Tmp.Add(item);
                        }
                    }

                }
            }
            foreach (var item in Tmp)
            {
                ChildBuildItem.Remove(item);
            }
            if (ChildBuildItem.Count == 0)
            {
                NodelType = ResourceBuildNodelType.UselessNodel;
            }
        }

        public void RefreshViewGUI(ResourceItemConfig Parent)
        {
            if (Parent.IsShowBuildToogle && !Parent.IsMergeBuild)
            {
                IsShowBuildToogle = true;
                IsMergeBuild = true;
                AssetBundleName = Parent.AssetBundleName + "/" + Name;
            }
            else
            {
                IsShowBuildToogle = false;
                AssetBundleName = Parent.AssetBundleName;
            }
            RefreshChildViewGUI();
        }

        public void RefreshChildViewGUI()
        {
            foreach (var item in ChildBuildItem)
            {
                item.RefreshViewGUI(this);
            }
        }

        public virtual void OnGUI()
        {
#if UNITY_EDITOR
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            EditorGUI.indentLevel = Layer+1;
            if (NodelType == ResourceBuildNodelType.FolderNodel)
            {
                Foldout = EditorGUILayout.Foldout(Foldout, Name);
                EditorGUILayout.Space();
                if (IsShowBuildToogle)
                {
                    bool mIsMergeBuild = GUILayout.Toggle(IsMergeBuild, "合并");

                    if (mIsMergeBuild != IsMergeBuild)
                    {
                        IsMergeBuild = mIsMergeBuild;
                        RefreshChildViewGUI();
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField(Name);
                EditorGUILayout.Space();
                if (IsShowBuildToogle)
                {
                    IsMergeBuild = GUILayout.Toggle(IsMergeBuild, "打包");
                }
            }
            EditorGUILayout.EndHorizontal();
            if (Foldout)
            {
                foreach (var item in ChildBuildItem)
                {
                    item.OnGUI();
                }
            }
#endif
        }

        //获取编译输出文件
        public virtual string GetBuildOutFile()
        {
            if (!Path.EndsWith("lua") && !Path.EndsWith("proto") && !Path.EndsWith("bytes"))
            {
                return (Application.dataPath + Path).Substring(AppConfig.PlatformRoot.Length + 1);
            }
            else
            {
                if (Path.EndsWith("lua"))
                {
                    string luabuildfile = (Application.dataPath + Path).Replace(".lua", ".txt");
                    FilesExtend.CopyFile(Application.dataPath + Path, luabuildfile);
                    string outstr = luabuildfile.Substring(AppConfig.PlatformRoot.Length + 1);
                    return outstr;
                }
                else if (Path.EndsWith("proto"))
                {
                    string protobuildfile = (Application.dataPath + Path).Replace(".proto", ".txt");
                    FilesExtend.CopyFile(Application.dataPath + Path, protobuildfile);
                    string outstr = protobuildfile.Substring(AppConfig.PlatformRoot.Length + 1);
                    return outstr;
                }
                else if (Path.EndsWith("bytes"))
                {
                    string protobuildfile = (Application.dataPath + Path).Replace(".bytes", ".txt");
                    FilesExtend.CopyFile(Application.dataPath + Path, protobuildfile);
                    string outstr = protobuildfile.Substring(AppConfig.PlatformRoot.Length + 1);
                    return outstr;
                }
            }
            return "";
        }
        /// <summary>
        /// 清理编译输出文件
        /// </summary>
        /// <returns></returns>
        public virtual void ClearBuildOutFile()
        {
            if (Path.EndsWith("lua"))
            {
                string luabuildfile = (Application.dataPath + Path).Replace(".lua", ".txt");
                if (File.Exists(luabuildfile))
                {
                    File.Delete(luabuildfile);
                }
            }
            else if (Path.EndsWith("proto"))
            {
                string protobuildfile = (Application.dataPath + Path).Replace(".proto", ".txt");
                if (File.Exists(protobuildfile))
                {
                    File.Delete(protobuildfile);
                }
            }
            else if (Path.EndsWith("bytes"))
            {
                string protobuildfile = (Application.dataPath + Path).Replace(".bytes", ".txt");
                if (File.Exists(protobuildfile))
                {
                    File.Delete(protobuildfile);
                }
            }
        }

    }
    [System.Serializable]
    [SirenixEditorConfig]
    public class PackingConfig : GlobalConfig<PackingConfig>
    {
        [EnumToggleButtons, LabelText("目标平台"), BoxGroup("Build Settings")]
        public AppPlatform BuildPlatform;
        [EnumToggleButtons, LabelText("编译策略"), BoxGroup("Build Settings")]
        public BuildSwitchType BuildTarget;
        [BoxGroup("Build Settings"),LabelText("大版本")]
        public float ProVersion;
        [BoxGroup("Build Settings"), LabelText("小版本")]
        public float ResVersion;
        [BoxGroup("Build Settings"), LabelText("是否压缩")]
        public bool IsCompress;
        [BoxGroup("Build Settings"),ShowInInspector, LabelText("压缩密码"),ShowIf("IsCompress")]
        public string CompressPassword => AppConfig.ResZipPassword;
        [BoxGroup("Build Settings"), LabelText("输出路径"),ReadOnly]
        public string ResourceOutPath = Application.streamingAssetsPath;
        [HorizontalGroup(0.2f)]
        [OnValueChanged("OnListCatalogChanged")]
        [LabelText("目录"),ListDrawerSettings(CustomAddFunction = "CatalogAddFunction",  OnBeginListElementGUI = "OnBeginCatalogGUI")]
        public List<ResourceCatalog> ResourceCatalog;
        [LabelText("模块"), ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, OnBeginListElementGUI = "OnBeginModelGUI")]
        [HorizontalGroup(0.8f)]
        public List<ResourceModelConfig> ModelBuildConfig;

        private void OnBeginCatalogGUI(int index)
        {
            GUILayout.TextField(ResourceCatalog[index].Name);
        }

        private void OnBeginModelGUI(int index)
        {
            ModelBuildConfig[index].OnGUI();
        }
#if UNITY_EDITOR
        private ResourceCatalog CatalogAddFunction()
        {
            string NewPath = EditorUtility.OpenFolderPanel("选择资源目录", Application.dataPath, "");
            ResourceCatalog Catalog = AddNewResourceCatalog(NewPath);
            return Catalog;
        }
#endif
        private void OnListCatalogChanged()
        {
            ModelBuildConfig.Clear();
            foreach (var Item in ResourceCatalog)
            {
                RetrievalModelResourceDirectory(Item, Item.Path);
            }
        }
        public ResourceCatalog AddNewResourceCatalog(string _Path)
        {
            _Path = _Path.Substring(Application.dataPath.Length, _Path.Length - Application.dataPath.Length);
            ResourceCatalog mCatalog = new ResourceCatalog
            {
                Name = StringExtend.GetPathFolderName(_Path),
                Path = _Path
            };
            //RetrievalModelResourceDirectory(mCatalog, mCatalog.Path);
            return mCatalog;
        }

        /// <summary>
        /// 检索模块资源目录
        /// </summary>
        /// <param name="_Path"></param>
        private void RetrievalModelResourceDirectory(ResourceCatalog _Catalog, string _Path)
        {
            string[] fileList = Directory.GetFileSystemEntries(Application.dataPath + _Path);
            foreach (string file in fileList)
            {
                if (Directory.Exists(file))
                {
                    if (file.EndsWith("Module"))
                    {
                        string _file = file.Substring(Application.dataPath.Length, file.Length - Application.dataPath.Length);
                        ResourceModelConfig ModelConfig = new ResourceModelConfig(_Catalog.Path, _file);
                        if (ModelConfig.NodelType != ResourceBuildNodelType.UselessNodel)
                        {
                            AddResourceModelBuildConfig(ModelConfig);
                        }
                    }
                    else
                    {
                        string _file = file.Substring(Application.dataPath.Length, file.Length - Application.dataPath.Length);
                        RetrievalModelResourceDirectory(_Catalog, _file);
                    }
                }
            }
        }

        /// <summary>
        /// 添加模块资源编译配置
        /// </summary>
        /// <param name="_Config"></param>
        private void AddResourceModelBuildConfig(ResourceModelConfig _Config)
        {
            for (int i = 0; i < ModelBuildConfig.Count; i++)
            {
                if (ModelBuildConfig[i].Name == _Config.Name)
                {
                    ModelBuildConfig[i].MergeConfig(_Config);
                    return;
                }
            }
            ModelBuildConfig.Add(_Config);
        }
    }

   
}