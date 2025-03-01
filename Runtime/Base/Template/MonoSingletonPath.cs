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