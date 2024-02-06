using Newtonsoft.Json;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
namespace LG {

    [InitializeOnLoad]
    public class BuildInfo
    {
        public AppModuleAssetInfo AppBuildInfo;
        public Dictionary<string, AppModuleAssetInfo> ModuleBuildInfo;

        public BuildInfo()
        {
            AppBuildInfo = new AppModuleAssetInfo();
            ModuleBuildInfo = new Dictionary<string, AppModuleAssetInfo>();
        }

        public AssetBundleBuild[] GetAssetBundleBuild()
        {

            AppBuildInfo.ClearNoKeepAB();
            List<AssetBundleBuild> Builds = new List<AssetBundleBuild>();
            List<string> AppBuilderKeys = new List<string>(AppBuildInfo.AppResInfo.Keys);
            for (int i = 0; i < AppBuilderKeys.Count; i++)
            {
                Builds.Add(new AssetBundleBuild
                {
                    assetBundleName = AppBuilderKeys[i],
                    assetNames = AppBuildInfo.AppResInfo[AppBuilderKeys[i]].Assets.ToArray()
                });
            }


            foreach (var item in ModuleBuildInfo)
            {
                item.Value.ClearNoKeepAB();
                List<string> ModuleBuilderKeys = new List<string>(item.Value.AppResInfo.Keys);
                for (int i = 0; i < ModuleBuilderKeys.Count; i++)
                {
                    Builds.Add(new AssetBundleBuild
                    {
                        assetBundleName = ModuleBuilderKeys[i],
                        assetNames = item.Value.AppResInfo[ModuleBuilderKeys[i]].Assets.ToArray()
                    });
                }
            }
            return Builds.ToArray();
        }
    }
    public class LGToolsWindow_Build
    {
        [SerializeField]
        [InlineEditor(Expanded = true)]
        private PackingConfig Config = PackingConfig.Instance;

        [Button("刷新", ButtonSizes.Large)]
        private void RefreshButton()
        {
            Config.ModelBuildConfig.Clear();
            foreach (var Item in Config.ResourceCatalog)
            {
                RetrievalModelResourceDirectory(Item, Item.Path);
            }
        }

        [Button("编译", ButtonSizes.Large)]
        private void BuildButton()
        {
            EditorApplication.delayCall += BuildResourceModel;
        }

        private ResourceCatalog CatalogAddFunction()
        {
            string NewPath = EditorUtility.OpenFolderPanel("选择资源目录", Application.dataPath, "");
            ResourceCatalog Catalog = AddNewResourceCatalog(NewPath);
            return Catalog;
        }

        public ResourceCatalog AddNewResourceCatalog(string _Path)
        {
            _Path = _Path.Substring(Application.dataPath.Length, _Path.Length - Application.dataPath.Length);
            ResourceCatalog mCatalog = new ResourceCatalog
            {
                Name = StringExtend.GetPathFolderName(_Path),
                Path = _Path
            };

            RetrievalModelResourceDirectory(mCatalog, mCatalog.Path);
            return mCatalog;
        }

        private void OnBeginCatalogGUI(int index)
        {
            GUILayout.TextField(Config.ResourceCatalog[index].Name);
        }

        private void OnBeginModelGUI(int index)
        {
            Config.ModelBuildConfig[index].OnGUI();
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
            for (int i = 0; i < Config.ModelBuildConfig.Count; i++)
            {
                if (Config.ModelBuildConfig[i].Name == _Config.Name)
                {
                    Config.ModelBuildConfig[i].MergeConfig(_Config);
                    return;
                }
            }
            Config.ModelBuildConfig.Add(_Config);
        }
        #region 编译
        /// <summary>
        /// 编译资源模块
        /// </summary>
        public void BuildResourceModel()
        {
            BuildTarget BuildTargetPlatform = UnityEditor.BuildTarget.StandaloneWindows;
            switch (Config.BuildPlatform)
            {
                case AppPlatform.Android:
                    BuildTargetPlatform = UnityEditor.BuildTarget.Android;
                    break;
                case AppPlatform.IOS:
                    BuildTargetPlatform = UnityEditor.BuildTarget.iOS;
                    break;
                case AppPlatform.Windows:
                    BuildTargetPlatform = UnityEditor.BuildTarget.StandaloneWindows;
                    break;
                default:
                    BuildTargetPlatform = UnityEditor.BuildTarget.StandaloneWindows;
                    break;
            }
            //AppModuleAssetInfo BuildInfo = new AppModuleAssetInfo();
            BuildInfo Builds = new BuildInfo();
            foreach (var item in Config.ModelBuildConfig)
            {
                if (item.IsSelection)
                {
                    if (Config.BuildTarget == BuildSwitchType.OnlyApp)
                    {
                        if (!item.IsUpdataModule)
                        {
                            RetrievalResourceBuildInfo(item, ref Builds.AppBuildInfo);
                        }
                    }
                    else if (Config.BuildTarget == BuildSwitchType.OnlyModule)
                    {
                        if (item.IsUpdataModule)
                        {
                            AppModuleAssetInfo modulebuildinfo = new AppModuleAssetInfo();
                            RetrievalResourceBuildInfo(item, ref modulebuildinfo);
                            Builds.ModuleBuildInfo[item.ModelName] = modulebuildinfo;
                        }
                    }
                    else
                    {
                        if (!item.IsUpdataModule)
                        {
                            RetrievalResourceBuildInfo(item, ref Builds.AppBuildInfo);
                        }
                        else
                        {
                            AppModuleAssetInfo modulebuildinfo = new AppModuleAssetInfo();
                            RetrievalResourceBuildInfo(item, ref modulebuildinfo);
                            Builds.ModuleBuildInfo[item.ModelName] = modulebuildinfo;
                        }
                    }
                }
            }
            AssetDatabase.Refresh();
            AssetBundleBuild[] builds = Builds.GetAssetBundleBuild();

            if (!Directory.Exists(Config.ResourceOutPath))
            {
                Directory.CreateDirectory(Config.ResourceOutPath);
            }
            else
            {
                FilesExtend.ClearDirectory(Config.ResourceOutPath);
            }
            AssetBundleManifest BundleInfo = BuildPipeline.BuildAssetBundles(Config.ResourceOutPath, builds, BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTargetPlatform);
            if (BundleInfo != null)
            {
                WriteAppBuilderInfo(BundleInfo, Builds);
                FilesExtend.ClearDirFile(Config.ResourceOutPath, new string[] { AppConfig.ResFileSuffix, ".json" });
                if (Config.IsCompress)
                {
                    CreateZip();
                }
                foreach (var item in Config.ModelBuildConfig)
                {
                    if (item.IsSelection)
                    {
                        CleraBuildResourceFile(item);
                    }
                }
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError("卧槽哦，啥逼情况啊");
            }
        }
        private void CleraBuildResourceFile(ResourceItemConfig _Item)
        {
            if (_Item.NodelType == ResourceBuildNodelType.ResourcesItemNodel)
            {
                _Item.ClearBuildOutFile();
            }
            else
            {
                foreach (var item in _Item.ChildBuildItem)
                {
                    CleraBuildResourceFile(item);
                }
            }
        }
        private void RetrievalResourceBuildInfo(ResourceItemConfig _Item, ref AppModuleAssetInfo _BuildInfo)
        {
            string AssetBundleName = _Item.AssetBundleName.ToLower() + AppConfig.ResFileSuffix;
            if (!_BuildInfo.AppResInfo.ContainsKey(AssetBundleName))
            {
                _BuildInfo.AppResInfo[AssetBundleName] = new ResBuileInfo
                {
                    Id = AssetBundleName,
                    Model = _Item.ModelName,
                    Assets = new List<string>()
                };
            }
            if (_Item.NodelType == ResourceBuildNodelType.ResourcesItemNodel)
            {
                _BuildInfo.AppResInfo[AssetBundleName].Assets.Add(_Item.GetBuildOutFile());
            }
            else
            {
                foreach (var item in _Item.ChildBuildItem)
                {
                    RetrievalResourceBuildInfo(item, ref _BuildInfo);
                }
            }
        }

        public void WriteAppBuilderInfo(AssetBundleManifest BundleManifest, BuildInfo BuildInfo)
        {
            string[] AssetBundles = BundleManifest.GetAllAssetBundles();
            for (int i = 0; i < AssetBundles.Length; i++)
            {
                if (BuildInfo.AppBuildInfo.AppResInfo.ContainsKey(AssetBundles[i]))
                {
                    ResBuileInfo BuildeInfo = BuildInfo.AppBuildInfo.AppResInfo[AssetBundles[i]];
                    FileInfo fiInput = new FileInfo(Config.ResourceOutPath + "/" + BuildeInfo.Id);
                    BuildeInfo.Size = fiInput.Length / 1024.0f;
                    BuildeInfo.Md5 = BundleManifest.GetAssetBundleHash(AssetBundles[i]).ToString();
                    string[] Dependencie = BundleManifest.GetDirectDependencies(AssetBundles[i]);
                    BuildeInfo.Dependencies = new string[Dependencie.Length];
                    for (int n = 0; n < Dependencie.Length; n++)
                    {
                        BuildeInfo.Dependencies[n] = Dependencie[n];
                    }
                }
                else
                {
                    foreach (var item in BuildInfo.ModuleBuildInfo)
                    {
                        if (item.Value.AppResInfo.ContainsKey(AssetBundles[i]))
                        {
                            ResBuileInfo BuildeInfo = item.Value.AppResInfo[AssetBundles[i]];
                            FileInfo fiInput = new FileInfo(Config.ResourceOutPath + "/" + BuildeInfo.Id);
                            BuildeInfo.Size = fiInput.Length / 1024.0f;
                            BuildeInfo.Md5 = BundleManifest.GetAssetBundleHash(AssetBundles[i]).ToString();
                            string[] Dependencie = BundleManifest.GetDirectDependencies(AssetBundles[i]);
                            BuildeInfo.Dependencies = new string[Dependencie.Length];
                            for (int n = 0; n < Dependencie.Length; n++)
                            {
                                BuildeInfo.Dependencies[n] = Dependencie[n];
                            }
                        }
                    }
                }
                //else
                //{
                //    Debug.LogError("No AssetBundles Key=" + AssetBundles[i]);
                //}
            }

            if (Config.BuildTarget == BuildSwitchType.OnlyApp || Config.BuildTarget == BuildSwitchType.AppAndModule)
            {
                string Json = JsonConvert.SerializeObject(BuildInfo.AppBuildInfo);
                FilesExtend.WriteStrToFile(Config.ResourceOutPath + "/VersionInfo.json", "{\"ProVersion\":" + Config.ProVersion + "}");
                FilesExtend.WriteStrToFile(Config.ResourceOutPath + "/AssetInfo.json", Json);
            }
            if (Config.BuildTarget == BuildSwitchType.OnlyModule || Config.BuildTarget == BuildSwitchType.AppAndModule)
            {
                foreach (var item in BuildInfo.ModuleBuildInfo)
                {
                    string Json = JsonConvert.SerializeObject(item.Value);
                    FilesExtend.WriteStrToFile(Config.ResourceOutPath + "/" + item.Key.ToLower() + "/AssetInfo.json", Json);
                }
            }
        }

        /// <summary>
        /// 压缩资源文件
        /// </summary>
        public void CreateZip()
        {
            DirectoryInfo dir = new DirectoryInfo(Config.ResourceOutPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();
            string[] Files = new string[fileinfo.Length];
            for (int i = 0; i < fileinfo.Length; i++)
            {
                Files[i] = fileinfo[i].FullName;
            }
            EditorCoroutineRunner.StartEditorCoroutine(ZipTools.Zip(Files, Config.ResourceOutPath + "/Res.zip", AppConfig.ResZipPassword, new string[] { ".meta" }, UpdataZipProgress));
        }

        public void UpdataZipProgress(string _Describe, float _Progress)
        {
            EditorUtility.DisplayProgressBar("压缩文件", _Describe, _Progress);
            if (_Progress >= 1)
            {
                EditorUtility.ClearProgressBar();
                FilesExtend.ClearDirFile(Config.ResourceOutPath, new string[] { ".zip" });
                AssetDatabase.Refresh();
            }
        }

        #endregion
    }

}

