﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

using ZeroFramework;
using UnityEngine;

namespace ZeroFramework.WebRequest
{
    internal sealed class WWWFormInfo : IReference
    {
        private WWWForm m_WWWForm;
        private object m_UserData;

        public WWWFormInfo()
        {
            m_WWWForm = null;
            m_UserData = null;
        }

        public WWWForm WWWForm => m_WWWForm;

        public object UserData => m_UserData;

        public static WWWFormInfo Create(WWWForm wwwForm, object userData)
        {
            WWWFormInfo wwwFormInfo = ReferencePool.Acquire<WWWFormInfo>();
            wwwFormInfo.m_WWWForm = wwwForm;
            wwwFormInfo.m_UserData = userData;
            return wwwFormInfo;
        }

        public void Clear()
        {
            m_WWWForm = null;
            m_UserData = null;
        }
    }
}
