//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using UnityEngine;

namespace ZeroFramework.UI
{
    public sealed partial class UIManager : GameFrameworkModule, IUIManager
    {
        /// <summary>
        /// 界面实例对象。
        /// </summary>
        private sealed class UIPanelInstanceObject : ObjectBase
        {
            private object _uiFormAsset;
            private IUIPanelHelper _uiFormHelper;

            public UIPanelInstanceObject()
            {
                _uiFormAsset = null;
                _uiFormHelper = null;
            }

            public static UIPanelInstanceObject Create(string name, object uiFormAsset, object uiFormInstance, IUIPanelHelper uiFormHelper)
            {
                if (uiFormAsset == null)
                {
                    throw new GameFrameworkException("UI form asset is invalid.");
                }

                if (uiFormHelper == null)
                {
                    throw new GameFrameworkException("UI form helper is invalid.");
                }

                UIPanelInstanceObject uiFormInstanceObject = ReferencePool.Acquire<UIPanelInstanceObject>();
                uiFormInstanceObject.Initialize(name, uiFormInstance);
                uiFormInstanceObject._uiFormAsset = uiFormAsset;
                uiFormInstanceObject._uiFormHelper = uiFormHelper;
                return uiFormInstanceObject;
            }

            public override void Clear()
            {
                base.Clear();
                _uiFormAsset = null;
                _uiFormHelper = null;
            }

            protected internal override void Release(bool isShutdown)
            {
                _uiFormHelper.ReleaseUIForm(_uiFormAsset, Target);
            }
        }
    }
}
