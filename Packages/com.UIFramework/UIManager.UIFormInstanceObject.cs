//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework.UI
{
    public sealed partial class UIManager : GameFrameworkModule, IUIManager
    {
        /// <summary>
        /// 界面实例对象。
        /// </summary>
        private sealed class UIFormInstanceObject : ObjectBase
        {
            private object m_UIFormAsset;
            private IUIFormHelper m_UIFormHelper;

            public UIFormInstanceObject()
            {
                m_UIFormAsset = null;
                m_UIFormHelper = null;
            }

            public static UIFormInstanceObject Create(string name, object uiFormAsset, object uiFormInstance, IUIFormHelper uiFormHelper)
            {
                if (uiFormAsset == null)
                {
                    throw new GameFrameworkException("UI form asset is invalid.");
                }

                if (uiFormHelper == null)
                {
                    throw new GameFrameworkException("UI form helper is invalid.");
                }

                UIFormInstanceObject uiFormInstanceObject = ReferencePool.Acquire<UIFormInstanceObject>();
                uiFormInstanceObject.Initialize(name, uiFormInstance);
                uiFormInstanceObject.m_UIFormAsset = uiFormAsset;
                uiFormInstanceObject.m_UIFormHelper = uiFormHelper;
                return uiFormInstanceObject;
            }

            public override void Clear()
            {
                base.Clear();
                m_UIFormAsset = null;
                m_UIFormHelper = null;
            }

            protected internal override void Release(bool isShutdown)
            {
                m_UIFormHelper.ReleaseUIForm(m_UIFormAsset, Target);
            }
        }
    }
}
