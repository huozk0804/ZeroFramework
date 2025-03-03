//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using UnityEngine;

namespace ZeroFramework.Entity
{
    internal sealed class AttachEntityInfo : IReference
    {
        private Transform _parentTransform;
        private object _userData;

        public AttachEntityInfo()
        {
            _parentTransform = null;
            _userData = null;
        }

        public Transform ParentTransform => _parentTransform;

        public object UserData => _userData;

        public static AttachEntityInfo Create(Transform parentTransform, object userData)
        {
            AttachEntityInfo attachEntityInfo = ReferencePool.Acquire<AttachEntityInfo>();
            attachEntityInfo._parentTransform = parentTransform;
            attachEntityInfo._userData = userData;
            return attachEntityInfo;
        }

        public void Clear()
        {
            _parentTransform = null;
            _userData = null;
        }
    }
}
