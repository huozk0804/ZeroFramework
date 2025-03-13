//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;

namespace ZeroFramework
{
    [AttributeUsage(AttributeTargets.Class)] //这个特性只能标记在Class上
    public class MonoSingletonPath : Attribute
    {
        public MonoSingletonPath(string pathInHierarchy)
        {
            PathInHierarchy = pathInHierarchy;
        }

        public string PathInHierarchy { get; }
    }
}