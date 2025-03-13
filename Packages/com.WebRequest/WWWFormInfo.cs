//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using UnityEngine;

namespace ZeroFramework.WebRequest
{
    internal sealed class WWWFormInfo : IReference
    {
        private WWWForm _wwwForm;
        private object _userData;

        public WWWFormInfo()
        {
            _wwwForm = null;
            _userData = null;
        }

        public WWWForm WWWForm => _wwwForm;

        public object UserData => _userData;

        public static WWWFormInfo Create(WWWForm wwwForm, object userData)
        {
            WWWFormInfo wwwFormInfo = ReferencePool.Acquire<WWWFormInfo>();
            wwwFormInfo._wwwForm = wwwForm;
            wwwFormInfo._userData = userData;
            return wwwFormInfo;
        }

        public void Clear()
        {
            _wwwForm = null;
            _userData = null;
        }
    }
}
