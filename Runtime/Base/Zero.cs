using System.Collections.Generic;
using System;
using UnityEngine;
using ZeroFramework.Download;
using ZeroFramework.FileSystem;
using ZeroFramework.Network;
using ZeroFramework.Resource;
using ZeroFramework.Runtime;
using ZeroFramework.WebRequest;

namespace ZeroFramework
{
    [DisallowMultipleComponent]
    [MonoSingletonPath("ZeroFramework")]
    public class Zero : MonoSingleton<Zero>
    {
        private const int DefaultDpi = 96; // default windows dpi

        private float m_GameSpeedBeforePause = 1f;
        private bool isInit;
        private int m_FrameRate;
        private float m_GameSpeed;
        private bool m_RunInBackground;
        private bool m_NeverSleep;

        public bool IsInit => isInit;

        /// <summary>
        /// 获取或设置游戏帧率。
        /// </summary>
        public int FrameRate
        {
            get => m_FrameRate;
            set => Application.targetFrameRate = m_FrameRate = value;
        }

        /// <summary>
        /// 获取或设置游戏速度。
        /// </summary>
        public float GameSpeed
        {
            get => m_GameSpeed;
            set => Time.timeScale = m_GameSpeed = value >= 0f ? value : 0f;
        }

        /// <summary>
        /// 获取或设置是否允许后台运行。
        /// </summary>
        public bool RunInBackground
        {
            get => m_RunInBackground;
            set => Application.runInBackground = m_RunInBackground = value;
        }

        /// <summary>
        /// 获取或设置是否禁止休眠。
        /// </summary>
        public bool NeverSleep
        {
            get => m_NeverSleep;
            set
            {
                m_NeverSleep = value;
                Screen.sleepTimeout = value ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
            }
        }

        #region Unity Message

        private void Awake()
        {
#if UNITY_5_3_OR_NEWER || UNITY_5_3
            Utility.Converter.ScreenDpi = Screen.dpi;
            if (Utility.Converter.ScreenDpi <= 0)
            {
                Utility.Converter.ScreenDpi = DefaultDpi;
            }
#else
            Log.Error("Game Framework only applies with Unity 5.3 and above, but current Unity version is {0}.", Application.unityVersion);
            GameEntry.Shutdown(ShutdownType.Quit);
#endif

#if UNITY_5_6_OR_NEWER
            Application.lowMemory += OnLowMemory;
#endif
            isInit = true;

            FrameRate = GameFrameworkConfig.Instance.m_FrameRate;
            GameSpeed = GameFrameworkConfig.Instance.m_GameSpeed;
            RunInBackground = GameFrameworkConfig.Instance.m_RunInBackground;
            NeverSleep = GameFrameworkConfig.Instance.m_NeverSleep;

            //引用池设置处理
            var enableStrictCheck = GameFrameworkConfig.Instance.m_EnableStrictCheck;
            switch (enableStrictCheck)
            {
                case ReferenceStrictCheckType.AlwaysEnable:
                    ReferencePool.EnableStrictCheck = true;
                    break;
                case ReferenceStrictCheckType.OnlyEnableWhenDevelopment:
                    ReferencePool.EnableStrictCheck = Debug.isDebugBuild;
                    break;
                case ReferenceStrictCheckType.OnlyEnableInEditor:
                    ReferencePool.EnableStrictCheck = Application.isEditor;
                    break;
                default:
                    ReferencePool.EnableStrictCheck = false;
                    break;
            }

            InitTextHelper();
            InitLogHelper();
            InitVersionHelper();
            InitJsonHelper();
            InitCompressionHelper();
        }

        private void Start()
        {
        }

        private void FixedUpdate()
        {
        }

        private void Update()
        {
            foreach (var module in _frameworkModules)
            {
                module.Update(Time.deltaTime, Time.unscaledDeltaTime);
            }
        }

        protected override void OnApplicationQuit()
        {
#if UNITY_5_6_OR_NEWER
            Application.lowMemory -= OnLowMemory;
#endif
            StopAllCoroutines();
            base.OnApplicationQuit();
        }

        protected override void OnDestory()
        {
            for (var current = _frameworkModules.Last; current != null; current = current.Previous)
            {
                current.Value.Shutdown();
            }

            _frameworkModules.Clear();
            ReferencePool.ClearAll();
            Utility.Marshal.FreeCachedHGlobal();
            GameFrameworkLog.SetLogHelper(null);
            base.OnDestory();
            isInit = false;
        }

        #endregion

        #region Framework Module

        private readonly GameFrameworkLinkedList<GameFrameworkModule> _frameworkModules =
            new GameFrameworkLinkedList<GameFrameworkModule>();
        
        public IObjectPoolManager ObjectPool => GetModule<IObjectPoolManager>();
        public IProcedureManager Procedure => GetModule<IProcedureManager>();
        public IEventManager Event => GetModule<IEventManager>();
        public IFsmManager Fsm => GetModule<IFsmManager>();
        public IDataNodeManager DataNode => GetModule<IDataNodeManager>();
        public IDataTableManager DataTable => GetModule<IDataTableManager>();
        public IDownloadManager Download => GetModule<IDownloadManager>();
        public IFileSystemManager FileSystem => GetModule<IFileSystemManager>();
        public INetworkManager Network => GetModule<INetworkManager>();
        public IResourceManager Resource => GetModule<IResourceManager>();
        public IWebRequestManager WebRequest => GetModule<IWebRequestManager>();

        /// <summary>
        /// 获取脚本中的模块，如果不存在则会新创建
        /// </summary>
        public T GetModule<T>() where T : class
        {
            if (!isInit)
            {
                throw new GameFrameworkException("Get module before must be initialized.");
            }

            Type interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
            {
                throw new GameFrameworkException(
                    Utility.Text.Format("You must get module by interface, but '{0}' is not.", interfaceType.FullName));
            }

            if (!interfaceType.FullName.StartsWith("ZeroFramework.", StringComparison.Ordinal))
            {
                throw new GameFrameworkException(
                    Utility.Text.Format("You must get a Game Framework module, but '{0}' is not.",
                        interfaceType.FullName));
            }

            string moduleName =
                Utility.Text.Format("{0}.{1}", interfaceType.Namespace, interfaceType.Name.Substring(1));
            Type moduleType = Type.GetType(moduleName);
            if (moduleType == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Can not find Game Framework module type '{0}'.",
                    moduleName));
            }

            return GetModule(moduleType) as T;
        }

        /// <summary>
        /// 优先从缓存链表中拿取实例，没有则创建
        /// </summary>
        private GameFrameworkModule GetModule(Type moduleType)
        {
            foreach (GameFrameworkModule module in _frameworkModules)
            {
                if (module.GetType() == moduleType)
                {
                    return module;
                }
            }

            return CreateModule(moduleType);
        }

        /// <summary>
        /// 创建模块的实例
        /// </summary>
        private GameFrameworkModule CreateModule(Type moduleType)
        {
            GameFrameworkModule module = (GameFrameworkModule)Activator.CreateInstance(moduleType);
            if (module == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Can not create module '{0}'.",
                    moduleType.FullName));
            }

            LinkedListNode<GameFrameworkModule> current = _frameworkModules.First;
            while (current != null)
            {
                if (module.Priority > current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                _frameworkModules.AddBefore(current, module);
            }
            else
            {
                _frameworkModules.AddLast(module);
            }

            return module;
        }

        #endregion

        #region Base Helper

        private void InitTextHelper()
        {
            var name = GameFrameworkConfig.Instance.m_TextHelperTypeName;
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            Type textHelperType = Utility.Assembly.GetType(name);
            if (textHelperType == null)
            {
                Log.Error("Can not find text helper type '{0}'.", name);
                return;
            }

            Utility.Text.ITextHelper textHelper = (Utility.Text.ITextHelper)Activator.CreateInstance(textHelperType);
            if (textHelper == null)
            {
                Log.Error("Can not create text helper instance '{0}'.", name);
                return;
            }

            Utility.Text.SetTextHelper(textHelper);
        }

        private void InitVersionHelper()
        {
            var name = GameFrameworkConfig.Instance.m_VersionHelperTypeName;
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            Type versionHelperType = Utility.Assembly.GetType(name);
            if (versionHelperType == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Can not find version helper type '{0}'.",
                    name));
            }

           Version.IVersionHelper versionHelper = (Version.IVersionHelper)Activator.CreateInstance(versionHelperType);
            if (versionHelper == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Can not create version helper instance '{0}'.",
                    name));
            }

            Version.SetVersionHelper(versionHelper);
        }

        private void InitLogHelper()
        {
            var name = GameFrameworkConfig.Instance.m_LogHelperTypeName;
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            Type logHelperType = Utility.Assembly.GetType(name);
            if (logHelperType == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Can not find log helper type '{0}'.", name));
            }

            GameFrameworkLog.ILogHelper
                logHelper = (GameFrameworkLog.ILogHelper)Activator.CreateInstance(logHelperType);
            if (logHelper == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Can not create log helper instance '{0}'.",
                    name));
            }

            GameFrameworkLog.SetLogHelper(logHelper);
        }

        private void InitCompressionHelper()
        {
            var name = GameFrameworkConfig.Instance.m_CompressionHelperTypeName;
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            Type compressionHelperType = Utility.Assembly.GetType(name);
            if (compressionHelperType == null)
            {
                Log.Error("Can not find compression helper type '{0}'.", name);
                return;
            }

            Utility.Compression.ICompressionHelper compressionHelper =
                (Utility.Compression.ICompressionHelper)Activator.CreateInstance(compressionHelperType);
            if (compressionHelper == null)
            {
                Log.Error("Can not create compression helper instance '{0}'.", name);
                return;
            }

            Utility.Compression.SetCompressionHelper(compressionHelper);
        }

        private void InitJsonHelper()
        {
            var name = GameFrameworkConfig.Instance.m_JsonHelperTypeName;
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            Type jsonHelperType = Utility.Assembly.GetType(name);
            if (jsonHelperType == null)
            {
                Log.Error("Can not find JSON helper type '{0}'.", name);
                return;
            }

            Utility.Json.IJsonHelper jsonHelper = (Utility.Json.IJsonHelper)Activator.CreateInstance(jsonHelperType);
            if (jsonHelper == null)
            {
                Log.Error("Can not create JSON helper instance '{0}'.", name);
                return;
            }

            Utility.Json.SetJsonHelper(jsonHelper);
        }

        #endregion

        private void OnLowMemory()
        {
            Log.Info("Low memory reported...");
            ObjectPool.ReleaseAllUnused();

            //ObjectPoolComponent objectPoolComponent = GameEntry.GetComponent<ObjectPoolComponent>();
            //if (objectPoolComponent != null)
            //{
            //    objectPoolComponent.ReleaseAllUnused();
            //}

            //ResourceComponent resourceCompoent = GameEntry.GetComponent<ResourceComponent>();
            //if (resourceCompoent != null)
            //{
            //    resourceCompoent.ForceUnloadUnusedAssets(true);
            //}
        }

        /// <summary>
        /// 关闭框架
        /// </summary>
        public void Shutdown(ShutdownType shutdownType)
        {
            Log.Info("Shutdown Game Framework ({0})...", shutdownType);
            if (shutdownType == ShutdownType.None)
            {
                return;
            }

            if (shutdownType == ShutdownType.Restart)
            {
                //SceneManager.LoadScene(GameFrameworkSceneId);
                return;
            }

            if (shutdownType == ShutdownType.Quit)
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                return;
            }
        }
    }
}