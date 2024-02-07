using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LG
{

    public class GUIManagerModule : ModuleBase<GUIManagerModule>
    {
        private GameObject uiRoot;                      //底优先级显示节点
        private Camera uiCamera;                        //底优先级显示相机
        private Vector2 mViewSzie;                      //UI界面尺寸
        private float mMatch;                           //UI适配权重
        private List<ViewComp> mLowViewComps;           //底优先级UI面板
        private List<ViewComp> mNormalViewComps;        //底优先级UI面板
        private List<ViewComp> mHightViewComps;         //底优先级UI面板

        public GameObject UIRoot
        {
            get { return uiRoot; }
        }

        public Camera UICamera
        {
            get { return uiCamera; }
        }

        public Vector2 ViewSzie
        {
            get { return mViewSzie; }
        }
        public override void LGLoad(params object[] agr)
        {
            ResourceComp = AddComp<Module_ResourceComp>();
            if (agr.Length == 2)
            {
                mViewSzie = (Vector2)agr[0];
                mMatch = (float)agr[1];
                mLowViewComps = new List<ViewComp>();
                mNormalViewComps = new List<ViewComp>();
                mHightViewComps = new List<ViewComp>();
                CreateUIRoot();
                base.LGLoad(agr);
            }
            else
            {
                Debug.LogError("ViewManagerModule 启动参数错误，请检查代码");
            }
        }
        private void CreateUIRoot()
        {
            uiRoot = GameObject.Find("UIRoot");
            if (uiRoot == null)
            {
                uiRoot = CreateObj("Prefab", "UIRoot", null);
                uiRoot.name = "UIRoot";
            }
            GameObject.DontDestroyOnLoad(uiRoot);
            Camera _cm0 = GameObject.Find("Camera").GetComponent<Camera>();
            _cm0.orthographic = true;
            _cm0.clearFlags = CameraClearFlags.Depth;
            _cm0.nearClipPlane = -100;
            _cm0.farClipPlane = 100;
            _cm0.orthographicSize = 20;
            _cm0.depth = 0;
            _cm0.cullingMask = LayerMask.GetMask("UI");
            uiCamera = _cm0;
            Canvas _ca0 = uiRoot.GetComponent<Canvas>();
            _ca0.renderMode = RenderMode.ScreenSpaceOverlay;
            _ca0.worldCamera = _cm0;
            _ca0.sortingOrder = 0;
            UnityEngine.UI.CanvasScaler _cs0 = uiRoot.GetComponent<UnityEngine.UI.CanvasScaler>();
            _cs0.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _cs0.referenceResolution = mViewSzie;
            _cs0.matchWidthOrHeight = mMatch;
        }

        public GameObject CreateView(ViewComp View, GameObject ViewAsset)
        {
            if (ViewAsset == null)
            {
                Debug.LogError("加载的界面 UI prefab 不存在");
                return null;
            }
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
            int offset = 0;
            switch (Level)
            {
                case UILevel.LowUI:
                    views = mLowViewComps;
                    offset = 0;
                    break;
                case UILevel.NormalUI:
                    views = mNormalViewComps;
                    offset = 1000;
                    break;
                case UILevel.HightUI:
                    views = mHightViewComps;
                    offset = 2000;
                    break;
            }
            for (int i = 0; i < views.Count; i++)
            {
                views[i].SetIndex(i+ offset);
            }

        }
    }
}