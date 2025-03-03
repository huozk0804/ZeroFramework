//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework.UI
{
    public sealed partial class UIManager : GameFrameworkModule, IUIManager
    {
        private sealed partial class UIGroup : IUIGroup
        {
            /// <summary>
            /// 界面组界面信息。
            /// </summary>
            private sealed class UIPanelInfo : IReference
            {
                private IUIPanel _uiForm;
                private bool _paused;
                private bool _covered;

                public UIPanelInfo()
                {
                    _uiForm = null;
                    _paused = false;
                    _covered = false;
                }

                public IUIPanel UIForm => _uiForm;

                public bool Paused
                {
                    get => _paused;
                    set => _paused = value;
                }

                public bool Covered
                {
                    get => _covered;
                    set => _covered = value;
                }

                public static UIPanelInfo Create(IUIPanel uiForm)
                {
                    if (uiForm == null)
                    {
                        throw new GameFrameworkException("UI form is invalid.");
                    }

                    UIPanelInfo uiFormInfo = ReferencePool.Acquire<UIPanelInfo>();
                    uiFormInfo._uiForm = uiForm;
                    uiFormInfo._paused = true;
                    uiFormInfo._covered = true;
                    return uiFormInfo;
                }

                public void Clear()
                {
                    _uiForm = null;
                    _paused = false;
                    _covered = false;
                }
            }
        }
    }
}
