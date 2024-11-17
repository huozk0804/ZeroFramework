using UnityEngine;

namespace ZeroFramework
{
    public abstract class ScriptableObjectSingleton<T> : ScriptableObject, ISingleton where T : ScriptableObjectSingleton<T>
    {
        protected static bool _onApplicationQuit = false;
        protected static T _instance;

        public static T Instance
        {
            get
            {
                if (!_instance && !_onApplicationQuit)
                {
                    _instance = UnityEngine.Resources.Load<T>(typeof(T).Name);
                    if (!_instance)
                    {
                        _instance = CreateInstance<T>();
                    }
                }

                return _instance;
            }
        }

        public static bool IsApplicationQuit => _onApplicationQuit;
        public void OnDispose()
        {
            if (SingletonCreator.IsUnitTestMode)
            {
                var curTrans = this;
                do
                {
                    DestroyImmediate(this);
                } while (curTrans);
                _instance = null;
            }
            else
            {
                Destroy(this);
            }
        }
    }
}