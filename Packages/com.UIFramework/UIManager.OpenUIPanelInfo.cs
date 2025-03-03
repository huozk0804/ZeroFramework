//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework.UI
{
    public sealed partial class UIManager : GameFrameworkModule, IUIManager
    {
        private sealed class OpenUIPanelInfo : IReference
        {
            private int _serialId;
            private UIGroup _uiGroup;
            private bool _pauseCoveredUIForm;
            private object _userData;

            public OpenUIPanelInfo()
            {
                _serialId = 0;
                _uiGroup = null;
                _pauseCoveredUIForm = false;
                _userData = null;
            }

            public int SerialId => _serialId;

            public UIGroup UIGroup => _uiGroup;

            public bool PauseCoveredUIForm => _pauseCoveredUIForm;

            public object UserData => _userData;

            public static OpenUIPanelInfo Create(int serialId, UIGroup uiGroup, bool pauseCoveredUIForm, object userData)
            {
                OpenUIPanelInfo openUIFormInfo = ReferencePool.Acquire<OpenUIPanelInfo>();
                openUIFormInfo._serialId = serialId;
                openUIFormInfo._uiGroup = uiGroup;
                openUIFormInfo._pauseCoveredUIForm = pauseCoveredUIForm;
                openUIFormInfo._userData = userData;
                return openUIFormInfo;
            }

            public void Clear()
            {
                _serialId = 0;
                _uiGroup = null;
                _pauseCoveredUIForm = false;
                _userData = null;
            }
        }
    }
}
