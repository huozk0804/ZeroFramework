﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

namespace ZeroFramework.UI
{
    public sealed partial class UIManager : GameFrameworkModule, IUIManager
    {
        private sealed class OpenUIFormInfo : IReference
        {
            private int m_SerialId;
            private UIGroup m_UIGroup;
            private bool m_PauseCoveredUIForm;
            private object m_UserData;

            public OpenUIFormInfo()
            {
                m_SerialId = 0;
                m_UIGroup = null;
                m_PauseCoveredUIForm = false;
                m_UserData = null;
            }

            public int SerialId => m_SerialId;

            public UIGroup UIGroup => m_UIGroup;

            public bool PauseCoveredUIForm => m_PauseCoveredUIForm;

            public object UserData => m_UserData;

            public static OpenUIFormInfo Create(int serialId, UIGroup uiGroup, bool pauseCoveredUIForm, object userData)
            {
                OpenUIFormInfo openUIFormInfo = ReferencePool.Acquire<OpenUIFormInfo>();
                openUIFormInfo.m_SerialId = serialId;
                openUIFormInfo.m_UIGroup = uiGroup;
                openUIFormInfo.m_PauseCoveredUIForm = pauseCoveredUIForm;
                openUIFormInfo.m_UserData = userData;
                return openUIFormInfo;
            }

            public void Clear()
            {
                m_SerialId = 0;
                m_UIGroup = null;
                m_PauseCoveredUIForm = false;
                m_UserData = null;
            }
        }
    }
}