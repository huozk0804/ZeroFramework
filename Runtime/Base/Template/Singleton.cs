namespace ZeroFramework
{
    public abstract class Singleton<T> : ISingleton where T : Singleton<T>
    {
        private static T _instance;

        /// <summary>
        /// 标签锁：确保当一个线程位于代码的临界区时，另一个线程不进入临界区。
        /// 如果其他线程试图进入锁定的代码，则它将一直等待（即被阻止），直到该对象被释放
        /// </summary>
        static object _lock = new object();

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = SingletonCreator.CreateSingleton<T>();
                    }
                }

                return _instance;
            }
        }

        public virtual void OnInit() { }

        public virtual void OnDispose()
        {
            _instance = null;
        }
    }
}