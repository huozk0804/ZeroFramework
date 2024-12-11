using UnityEngine;

namespace ZeroFramework.UI
{
    public class UIForms
    {
        public int SerialId { get; }
        public string UIFormAssetName { get; }
        public GameObject Handle { get; }
        public IUIGroup UIGroup { get; }
        public int DepthInUIGroup { get; }
        public bool PauseCoveredUIForm { get; }

        public void OnInit(int serialId, string uiFormAssetName, IUIGroup uiGroup, bool pauseCoveredUIForm,
            bool isNewInstance, object userData)
        {
            throw new System.NotImplementedException();
        }

        public void OnRecycle()
        {
            throw new System.NotImplementedException();
        }

        public void OnOpen(object userData)
        {
            throw new System.NotImplementedException();
        }

        public void OnClose(bool isShutdown, object userData)
        {
            throw new System.NotImplementedException();
        }

        public void OnPause()
        {
            throw new System.NotImplementedException();
        }

        public void OnResume()
        {
            throw new System.NotImplementedException();
        }

        public void OnCover()
        {
            throw new System.NotImplementedException();
        }

        public void OnReveal()
        {
            throw new System.NotImplementedException();
        }

        public void OnRefocus(object userData)
        {
            throw new System.NotImplementedException();
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            throw new System.NotImplementedException();
        }

        public void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            throw new System.NotImplementedException();
        }
    }
}