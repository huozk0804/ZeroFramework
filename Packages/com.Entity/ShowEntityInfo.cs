//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;

namespace ZeroFramework.Entity
{
    internal sealed class ShowEntityInfo : IReference
    {
        private Type _entityLogicType;
        private object _userData;

        public ShowEntityInfo()
        {
            _entityLogicType = null;
            _userData = null;
        }

        public Type EntityLogicType => _entityLogicType;

        public object UserData => _userData;

        public static ShowEntityInfo Create(Type entityLogicType, object userData)
        {
            ShowEntityInfo showEntityInfo = ReferencePool.Acquire<ShowEntityInfo>();
            showEntityInfo._entityLogicType = entityLogicType;
            showEntityInfo._userData = userData;
            return showEntityInfo;
        }

        public void Clear()
        {
            _entityLogicType = null;
            _userData = null;
        }
    }
}
