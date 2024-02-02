using UnityEngine;

namespace LG
{

    /// <summary>
    /// 模块资源组件
    /// </summary>
    public class Module_ResourceComp : ModelCompBase
    {
        #region 构架
        public override void LGLoad(ModuleBase module, params object[] agrs)
        {
            base.LGLoad(module, agrs);
            base.LoadEnd();
        }
        public override void Close()
        {
            if (ResourceModule.Instance != null)
                ResourceModule.Instance.UnloadModel(Module.ModuleName);
            base.Close();
        }
        #endregion

        public AssetBundle LoadAssetBundle(string BundleName)
        {
            string ModelName = Module.ModuleName;
            if (AppConfig.AppResModel == AppResModel.release)
            {
                ModelName = ModelName.ToLower();
                BundleName = BundleName.ToLower();
            }
            if (ResourceModule.Instance != null)
            {
                return ResourceModule.Instance.LoadAssetBundle(Module.ModuleName, BundleName);
            }
            else
            {
                Debug.LogError("ResourceModel No Load");
                return null;
            }
        }

        public T LoadAsset<T>(string BundleName, string AssetName) where T : UnityEngine.Object
        {
            string ModelName = Module.ModuleName;
            if (AppConfig.AppResModel == AppResModel.release)
            {
                ModelName = ModelName.ToLower();
                BundleName = BundleName.ToLower();
                if (AssetName != null)
                    AssetName = AssetName.ToLower();
            }
            if (ResourceModule.Instance != null)
            {
                return ResourceModule.Instance.LoadAsset<T>(ModelName, BundleName, AssetName);
            }
            else
            {
                Debug.LogError("ResourceModel No Load");
                return null;
            }
        }
        public T[] LoadAllAsset<T>(string BundleName, string AssetName) where T : UnityEngine.Object
        {
            string ModelName = Module.ModuleName;
            if (AppConfig.AppResModel == AppResModel.release)
            {
                ModelName = ModelName.ToLower();
                BundleName = BundleName.ToLower();
                if (AssetName != null)
                    AssetName = AssetName.ToLower();
            }
            if (ResourceModule.Instance != null)
            {
                return ResourceModule.Instance.LoadAllAsset<T>(ModelName, BundleName, AssetName);
            }
            else
            {
                Debug.LogError("ResourceModel No Load");
                return null;
            }
        }

        public byte[] LoadByteFile(string BundleName, string AssetName)
        {
            string ModelName = Module.ModuleName;
            if (AppConfig.AppResModel == AppResModel.release)
            {
                ModelName = ModelName.ToLower();
                BundleName = BundleName.ToLower();
                if (AssetName != null)
                    AssetName = AssetName.ToLower();
            }
            if (ResourceModule.Instance != null)
            {
                return ResourceModule.Instance.LoadByteFile(ModelName, BundleName, AssetName);
            }
            else
            {
                Debug.LogError("ResourceModel No Load");
                return null;
            }
        }


        public byte[] LoadLuaFile(string BundleName, string AssetName)
        {
            string ModelName = Module.ModuleName;
            if (AppConfig.AppResModel == AppResModel.release)
            {
                ModelName = ModelName.ToLower();
                BundleName = BundleName.ToLower();
                if (AssetName != null)
                    AssetName = AssetName.ToLower();
            }
            if (ResourceModule.Instance != null)
            {
                return ResourceModule.Instance.LoadLuaFile(ModelName, BundleName, AssetName);
            }
            else
            {
                Debug.LogError("ResourceModel No Load");
                return null;
            }
        }

        public byte[] LoadProtoFile(string BundleName, string AssetName)
        {
            string ModelName = Module.ModuleName;
            if (AppConfig.AppResModel == AppResModel.release)
            {
                ModelName = ModelName.ToLower();
                BundleName = BundleName.ToLower();
                if (AssetName != null)
                    AssetName = AssetName.ToLower();
            }
            if (ResourceModule.Instance != null)
            {
                return ResourceModule.Instance.LoadProtoFile(ModelName, BundleName, AssetName);
            }
            else
            {
                Debug.LogError("ResourceModel No Load");
                return null;
            }
        }

        public void UnloadAsset(string BundleName, string AssetName)
        {
            string ModelName = Module.ModuleName;
            if (AppConfig.AppResModel == AppResModel.release)
            {
                ModelName = ModelName.ToLower();
                BundleName = BundleName.ToLower();
                AssetName = AssetName.ToLower();
            }
            ResourceModule.Instance.UnloadAsset(Module.ModuleName, BundleName, AssetName);
        }

        public void UnloadBundle(string BundleName)
        {
            string ModelName = Module.ModuleName;
            if (AppConfig.AppResModel == AppResModel.release)
            {
                ModelName = ModelName.ToLower();
                BundleName = BundleName.ToLower();
            }
            ResourceModule.Instance.UnloadBundle(Module.ModuleName, BundleName);
        }

    }
}