using System;

namespace ZeroFramework
{
    [AttributeUsage(AttributeTargets.Class)] //这个特性只能标记在Class上
    public class MonoSingletonPath : Attribute
    {
        private string _pathInHierarchy;

        public MonoSingletonPath(string pathInHierarchy)
        {
            _pathInHierarchy = pathInHierarchy;
        }

        public string PathInHierarchy => _pathInHierarchy;
    }
}