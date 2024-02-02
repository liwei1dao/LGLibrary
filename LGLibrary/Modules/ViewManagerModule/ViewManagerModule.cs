using UnityEngine;
using UnityEngine.UI;

namespace LG
{

    public class GUIManagerModule : ModuleBase<GUIManagerModule>
    {
        private GameObject mLowUIRoot;                //底优先级显示节点
        private GameObject mNormalUIRoot;             //中优先级显示节点
        private GameObject mHightUIRoot;              //高优先级显示节点

        private Camera mLowUICamera;                //底优先级显示相机
        private Camera mNormalUCamera;             //中优先级显示相机
        private Camera mHightUICamera;              //高优先级显示相机

        private Vector2 mViewSzie;                    //UI界面尺寸
        private List<ViewComp> mLowViewComps;         //底优先级UI面板
        private List<ViewComp> mNormalViewComps;      //底优先级UI面板
        private List<ViewComp> mHightViewComps;       //底优先级UI面板

        public GameObject LowUIRoot
        {
            get { return mLowUIRoot; }
        }

        public GameObject NormalUIRoot
        {
            get { return mNormalUIRoot; }
        }

        public GameObject HightUIRoot
        {
            get { return mHightUIRoot; }
        }

        public Camera LowUICamera
        {
            get { return mLowUICamera; }
        }

        public Camera NormalUICamera
        {
            get { return mNormalUCamera; }
        }

        public Camera HightUICamera
        {
            get { return mHightUICamera; }
        }

        public Vector2 ViewSzie
        {
            get { return mViewSzie; }
        }

        public GameObject CreateView(ViewComp View, GameObject ViewAsset)
        {
            if (ViewAsset == null)
            {
                Debug.LogError("加载的界面 UI prefab 不存在");
                return null;
            }
            GameObject UIRoot = null;
            List<ViewComp> views = null;
            switch (View.GetLevel())
            {
                case UILevel.LowUI:
                    UIRoot = LowUIRoot;
                    views = mLowViewComps;
                    break;
                case UILevel.NormalUI:
                    UIRoot = NormalUIRoot;
                    views = mNormalViewComps;
                    break;
                case UILevel.HightUI:
                    UIRoot = HightUIRoot;
                    views = mHightViewComps;
                    break;
            }
            GameObject UIGameobject = ViewAsset.CreateToParnt(UIRoot);
            RectTransform rectTrans = UIGameobject.GetComponent<RectTransform>();
            rectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            rectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            rectTrans.anchorMin = Vector2.zero;
            rectTrans.anchorMax = Vector2.one;
            Canvas canvas = UIGameobject.AddMissingComponent<Canvas>();
            UIGameobject.AddMissingComponent<GraphicRaycaster>();
            canvas.overrideSorting = true;
            View.SetCanvas(canvas);
            View.SetIndex(views.Count);
            views.Add(View);
            return UIGameobject;
        }

        public GameObject CreateEmptyView(ViewComp View, string ViewName)
        {
            GameObject UIRoot = null;
            List<ViewComp> views = null;
            switch (View.GetLevel())
            {
                case UILevel.LowUI:
                    UIRoot = LowUIRoot;
                    views = mLowViewComps;
                    break;
                case UILevel.NormalUI:
                    UIRoot = NormalUIRoot;
                    views = mNormalViewComps;
                    break;
                case UILevel.HightUI:
                    UIRoot = HightUIRoot;
                    views = mHightViewComps;
                    break;
            }

            GameObject UIGameobject = UIRoot.CreateChild(ViewName);
            RectTransform rectTrans = UIGameobject.AddMissingComponent<RectTransform>();
            rectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            rectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            rectTrans.anchorMin = Vector2.zero;
            rectTrans.anchorMax = Vector2.one;
            Canvas canvas = UIGameobject.AddMissingComponent<Canvas>();
            UIGameobject.AddMissingComponent<GraphicRaycaster>();
            canvas.overrideSorting = true;
            View.SetCanvas(canvas);
            View.SetIndex(views.Count);
            views.Add(View);
            return UIGameobject;
        }
        public GameObject FindView(ViewComp View, string ViewName)
        {
            GameObject UIRoot = null;
            List<ViewComp> views = null;
            switch (View.GetLevel())
            {
                case UILevel.LowUI:
                    UIRoot = LowUIRoot;
                    views = mLowViewComps;
                    break;
                case UILevel.NormalUI:
                    UIRoot = NormalUIRoot;
                    views = mNormalViewComps;
                    break;
                case UILevel.HightUI:
                    UIRoot = HightUIRoot;
                    views = mHightViewComps;
                    break;
            }
            GameObject UIGameobject = UIRoot.Find(ViewName);
            if (UIGameobject != null)
            {
                RectTransform rectTrans = UIGameobject.GetComponent<RectTransform>();
                rectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
                rectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
                rectTrans.anchorMin = Vector2.zero;
                rectTrans.anchorMax = Vector2.one;
                Canvas canvas = UIGameobject.AddMissingComponent<Canvas>();
                UIGameobject.AddMissingComponent<GraphicRaycaster>();
                canvas.overrideSorting = true;
                View.SetCanvas(canvas);
                View.SetIndex(views.Count);
                views.Add(View);
                return UIGameobject;
            }
            else
            {
                return null;
            }
        }
        public void SetViewToTop(ViewComp View)
        {
            List<ViewComp> views = null;
            switch (View.GetLevel())
            {
                case UILevel.LowUI:
                    views = mLowViewComps;
                    break;
                case UILevel.NormalUI:
                    views = mNormalViewComps;
                    break;
                case UILevel.HightUI:
                    views = mHightViewComps;
                    break;
            }
            if (View.GetIndex() == views.Count - 1)
            {
                return;
            }
            views.RemoveAt(View.GetIndex());
            views.Add(View);
            UpdataViews(View.GetLevel());
        }
        public void DeletView(ViewComp View)
        {
            switch (View.GetLevel())
            {
                case UILevel.LowUI:
                    mLowViewComps.RemoveAt(View.GetIndex());
                    break;
                case UILevel.NormalUI:
                    mNormalViewComps.RemoveAt(View.GetIndex());
                    break;
                case UILevel.HightUI:
                    mHightViewComps.RemoveAt(View.GetIndex());
                    break;
            }
            UpdataViews(View.GetLevel());
        }
        public void UpdataViews(UILevel Level)
        {
            List<ViewComp> views = null;
            switch (Level)
            {
                case UILevel.LowUI:
                    views = mLowViewComps;
                    break;
                case UILevel.NormalUI:
                    views = mNormalViewComps;
                    break;
                case UILevel.HightUI:
                    views = mHightViewComps;
                    break;
            }
            for (int i = 0; i < views.Count; i++)
            {
                views[i].SetIndex(i);
            }

        }
    }
}