//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using UnityEngine;

namespace ZeroFramework
{
    public abstract class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
    {
        protected static bool onApplicationQuit = false;
        protected static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null && !onApplicationQuit)
                {
                    instance = SingletonCreator.CreateMonoSingleton<T>();
                }

                return instance;
            }
        }

        public static bool IsApplicationQuit => onApplicationQuit;

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

                instance = null;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnApplicationQuit()
        {
            onApplicationQuit = true;
            if (instance == null)
            {
                return;
            }

            Destroy(instance.gameObject);
            instance = null;
        }

        protected virtual void OnDestroy()
        {
            instance = null;
        }
    }
}