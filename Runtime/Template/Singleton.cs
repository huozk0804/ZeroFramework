//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework
{
    public abstract class Singleton<T> : ISingleton where T : Singleton<T>
    {
        private static T instance;

        /// <summary>
        /// 标签锁：确保当一个线程位于代码的临界区时，另一个线程不进入临界区。
        /// 如果其他线程试图进入锁定的代码，则它将一直等待（即被阻止），直到该对象被释放
        /// </summary>
        static object @lock = new object();

        public static T Instance
        {
            get
            {
                lock (@lock)
                {
                    instance ??= SingletonCreator.CreateSingleton<T>();
                }

                return instance;
            }
        }
        
        public virtual void OnDispose()
        {
            instance = null;
        }
    }
}