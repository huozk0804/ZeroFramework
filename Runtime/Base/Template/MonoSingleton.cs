using UnityEngine;

namespace ZeroFramework
{
    public abstract class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
    {
        protected static bool _onApplicationQuit = false;
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null && !_onApplicationQuit)
                {
                    _instance = SingletonCreator.CreateMonoSingleton<T>();
                }

                return _instance;
            }
        }

        public static bool IsApplicationQuit => _onApplicationQuit;

        public virtual void OnDispose()
        {
            if (SingletonCreator.IsUnitTestMode)
            {
                var curTrans = transform;
                do
                {
                    var parent = curTrans.parent;
                    DestroyImmediate(curTrans.gameObject);
                    curTrans = parent;
                } while (curTrans != null);

                _instance = null;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnApplicationQuit()
        {
            _onApplicationQuit = true;
            if (_instance == null)
            {
                return;
            }

            Destroy(_instance.gameObject);
            _instance = null;
        }

        protected virtual void OnDestory()
        {
            _instance = null;
        }
    }
}