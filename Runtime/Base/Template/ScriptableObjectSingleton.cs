using UnityEngine;

namespace ZeroFramework
{
    public abstract class ScriptableObjectSingleton<T> : ScriptableObject, ISingleton where T : ScriptableObjectSingleton<T>
    {
        protected static bool onApplicationQuit = false;
        protected static T instance;

        public static bool IsApplicationQuit => onApplicationQuit;
        public static T Instance
        {
            get
            {
                if (!instance && !onApplicationQuit)
                {
                    instance = UnityEngine.Resources.Load<T>(typeof(T).Name);
                    if (!instance)
                    {
                        instance = CreateInstance<T>();
                    }
                }

                return instance;
            }
        }
        
        public void OnDispose()
        {
            if (SingletonCreator.IsUnitTestMode)
            {
                var curTrans = this;
                do
                {
                    DestroyImmediate(this);
                } while (curTrans);
                instance = null;
            }
            else
            {
                Destroy(this);
            }
        }
    }
}