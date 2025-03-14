//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System.Collections.Generic;
using System;
using UnityEngine;
using ZeroFramework.Debugger;
using ZeroFramework.Config;
using ZeroFramework.Download;
using ZeroFramework.Entity;
using ZeroFramework.FileSystem;
using ZeroFramework.Localization;
using ZeroFramework.Network;
using ZeroFramework.Resource;
using ZeroFramework.Scenes;
using ZeroFramework.Setting;
using ZeroFramework.Sound;
using ZeroFramework.UI;
using ZeroFramework.WebRequest;
using Object = UnityEngine.Object;

namespace ZeroFramework
{
    /// <summary>
    /// 静态脚本，Zero框架的主入口
    /// </summary>
    public static class Zero
    {
        private const int DefaultDpi = 96; // default windows dpi

        private static float _gameSpeedBeforePause = 1f;
        private static int _frameRate;
        private static float _gameSpeed;
        private static bool _runInBackground;
        private static bool _neverSleep;
        private static GameObject _root;

        /// <summary>
        /// 获取或设置游戏帧率。
        /// </summary>
        public static int FrameRate
        {
            get => _frameRate;
            set => Application.targetFrameRate = _frameRate = value;
        }

        /// <summary>
        /// 获取或设置游戏速度。
        /// </summary>
        public static float GameSpeed
        {
            get => _gameSpeed;
            set => Time.timeScale = _gameSpeed = value >= 0f ? value : 0f;
        }

        /// <summary>
        /// 获取或设置是否允许后台运行。
        /// </summary>
        public static bool RunInBackground
        {
            get => _runInBackground;
            set => Application.runInBackground = _runInBackground = value;
        }

        /// <summary>
        /// 获取或设置是否禁止休眠。
        /// </summary>
        public static bool NeverSleep
        {
            get => _neverSleep;
            set
            {
                _neverSleep = value;
                Screen.sleepTimeout = value ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
            }
        }

        /// <summary>
        /// 暂停游戏。
        /// </summary>
        public static void PauseGame()
        {
            if (IsGamePaused)
                return;

            _gameSpeedBeforePause = GameSpeed;
            GameSpeed = 0f;
        }

        /// <summary>
        /// 获取游戏是否暂停。
        /// </summary>
        public static bool IsGamePaused => _gameSpeed <= 0f;

        /// <summary>
        /// 获取是否正常游戏速度。
        /// </summary>
        public static bool IsNormalGameSpeed => _gameSpeed == 1f;

        /// <summary>
        /// 框架物体根节点
        /// </summary>
        public static GameObject Root => _root;

        /// <summary>
        /// 恢复游戏。
        /// </summary>
        public static void ResumeGame()
        {
            if (!IsGamePaused)
                return;

            GameSpeed = _gameSpeedBeforePause;
        }

        /// <summary>
        /// 重置为正常游戏速度。
        /// </summary>
        public static void ResetNormalGameSpeed()
        {
            if (IsNormalGameSpeed)
                return;

            GameSpeed = 1f;
        }

        #region Lift Function

        public static void Init()
        {
            if (_root == null)
            {
                _root = new GameObject("[ZERO]Framework");
                _root.AddComponent<ZeroFrameworkComponent>();
            }

            FrameRate = GameFrameworkConfig.Instance.frameRate;
            GameSpeed = GameFrameworkConfig.Instance.gameSpeed;
            RunInBackground = GameFrameworkConfig.Instance.runInBackground;
            NeverSleep = GameFrameworkConfig.Instance.neverSleep;

            //引用池设置
            var enableStrictCheck = GameFrameworkConfig.Instance.enableStrictCheck;
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

            var debuggerType = GameFrameworkConfig.Instance.activeWindow;
            if (debuggerType != DebuggerActiveWindowType.AlwaysClose)
            {
                _root.GetOrAddComponent<DebuggerComponent>();
            }

#if UNITY_5_3_OR_NEWER || UNITY_5_3
            Utility.Converter.ScreenDpi = Screen.dpi;
            if (Utility.Converter.ScreenDpi <= 0)
            {
                Utility.Converter.ScreenDpi = DefaultDpi;
            }
#else
            Log.Error("Game Framework only applies with Unity 5.3 and above, but current Unity version is {0}.", Application.unityVersion);
			Application.Quit();
#endif

#if UNITY_5_6_OR_NEWER
            Application.lowMemory += OnLowMemory;
#endif

            Log.Info("Zero Framework Version: {0}", Version.GameFrameworkVersion);
            Log.Info("Unity Version: {0}", Application.unityVersion);
            Log.Info("Game Version: {0} ({1})", Version.GameVersion, Version.InternalGameVersion);
            Log.Info("Game Resources Version: {0}", Version.GameResVersion);
            Log.Info("Zero Framework Launch Succeed.");
        }

        public static void Update(float deltaTime, float unscaledDeltaTime)
        {
            for (var current = _frameworkModules.First; current != null; current = current.Next)
            {
                current.Value.Update(deltaTime, unscaledDeltaTime);
            }
        }

        public static void OnDestroy()
        {
            Log.Info("Zero Framework is destroyed.");
            
            for (var current = _frameworkModules.Last; current != null; current = current.Previous)
            {
                current.Value.Shutdown();
            }

            _frameworkModules.Clear();
            ReferencePool.ClearAll();
            Utility.Marshal.FreeCachedHGlobal();
            GameFrameworkLog.SetLogHelper(null);
        }

        /// <summary>
        /// 关闭框架
        /// </summary>
        public static void Shutdown()
        {
            Log.Info("Quit Zero Framework And Game.");

#if UNITY_5_6_OR_NEWER
            Application.lowMemory -= OnLowMemory;
#endif
            Object.DestroyImmediate(_root);
            _root = null;

            for (var current = _frameworkModules.Last; current != null; current = current.Previous)
            {
                current.Value.Shutdown();
            }

            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private static void OnLowMemory()
        {
            Log.Info("Low memory reported...");
            objectPool.ReleaseAllUnused();

            //ResourceComponent resourceCompoent = GameEntry.GetComponent<ResourceComponent>();
            //if (resourceCompoent != null)
            //{
            //    resourceCompoent.ForceUnloadUnusedAssets(true);
            //}
        }

        #endregion

        #region Framework Module

        private readonly static GameFrameworkLinkedList<GameFrameworkModule> _frameworkModules =
            new GameFrameworkLinkedList<GameFrameworkModule>();

        private static DataNodeManager _dataNodeManager;
        private static EventManager _eventManager;
        private static FsmManager _fsmManager;
        private static ObjectPoolManager _objectPoolManager;
        private static ProcedureManager _procedureManager;
        private static TimerManager _timerManager;
        private static ConfigManager _configManager;
        private static DownloadManager _downloadManager;
        private static EntityManager _entityManager;
        private static FileSystemManager _fileSystemManager;
        private static LocalizationManager _localizationManager;
        private static NetworkManager _networkManager;
        private static ResourceManager _resourceManager;
        private static ScenesManager _scenesManager;
        private static SettingManager _settingManager;
        private static SoundManager _soundManager;
        private static UIManager _uiManager;
        private static WebRequestManager _webRequestManager;

        /// <summary> 数据结点模块 </summary>
        public static DataNodeManager dataNode
        {
            get
            {
                if (_dataNodeManager == null)
                {
                    _dataNodeManager = GetModule<DataNodeManager>();
                }

                return _dataNodeManager;
            }
        }

        /// <summary> 事件订阅模块 </summary>
        public static EventManager @event
        {
            get
            {
                if (_eventManager == null)
                {
                    _eventManager = GetModule<EventManager>();
                }

                return _eventManager;
            }
        }

        /// <summary> 状态机模块 </summary>
        public static FsmManager fsm
        {
            get
            {
                if (_fsmManager == null)
                {
                    _fsmManager = GetModule<FsmManager>();
                }

                return _fsmManager;
            }
        }

        /// <summary> 对象池模块 </summary>
        public static ObjectPoolManager objectPool
        {
            get
            {
                if (_objectPoolManager == null)
                {
                    _objectPoolManager = GetModule<ObjectPoolManager>();
                }

                return _objectPoolManager;
            }
        }

        /// <summary> 状态机模块 </summary>
        public static ProcedureManager procedure
        {
            get
            {
                if (_procedureManager == null)
                {
                    _procedureManager = GetModule<ProcedureManager>();
                }

                return _procedureManager;
            }
        }

        /// <summary> 计时器模块 </summary>
        public static TimerManager timer
        {
            get
            {
                if (_timerManager == null)
                {
                    _timerManager = GetModule<TimerManager>();
                }

                return _timerManager;
            }
        }

        /// <summary> 配置表模块 </summary>
        public static ConfigManager config
        {
            get
            {
                if (_configManager == null)
                {
                    _configManager = GetModule<ConfigManager>();
                }

                return _configManager;
            }
        }

        /// <summary> 下载模块 </summary>
        public static DownloadManager download
        {
            get
            {
                if (_downloadManager == null)
                {
                    _downloadManager = GetModule<DownloadManager>();
                }

                return _downloadManager;
            }
        }

        /// <summary> 实体模块 </summary>
        public static EntityManager entity
        {
            get
            {
                if (_entityManager == null)
                {
                    _entityManager = GetModule<EntityManager>();
                }

                return _entityManager;
            }
        }

        /// <summary> 文件系统模块 </summary>
        internal static FileSystemManager file
        {
            get
            {
                if (_fileSystemManager == null)
                {
                    _fileSystemManager = GetModule<FileSystemManager>();
                }

                return _fileSystemManager;
            }
        }

        /// <summary> 本地化模块 </summary>
        public static LocalizationManager i10n
        {
            get
            {
                if (_localizationManager == null)
                {
                    _localizationManager = GetModule<LocalizationManager>();
                }

                return _localizationManager;
            }
        }

        /// <summary> 网络模块 </summary>
        public static NetworkManager network
        {
            get
            {
                if (_networkManager == null)
                {
                    _networkManager = GetModule<NetworkManager>();
                }

                return _networkManager;
            }
        }

        /// <summary> 资源模块 </summary>
        public static ResourceManager resource
        {
            get
            {
                if (_resourceManager == null)
                {
                    _resourceManager = GetModule<ResourceManager>();
                }

                return _resourceManager;
            }
        }

        /// <summary> 场景模块 </summary>
        public static ScenesManager scene
        {
            get
            {
                if (_scenesManager == null)
                {
                    _scenesManager = GetModule<ScenesManager>();
                }

                return _scenesManager;
            }
        }

        /// <summary> 存档模块 </summary>
        public static SettingManager setting
        {
            get
            {
                if (_settingManager == null)
                {
                    _settingManager = GetModule<SettingManager>();
                }

                return _settingManager;
            }
        }

        /// <summary> 音频模块 </summary>
        public static SoundManager audio
        {
            get
            {
                if (_soundManager == null)
                {
                    _soundManager = GetModule<SoundManager>();
                }

                return _soundManager;
            }
        }

        /// <summary> UI框架模块 </summary>
        public static UIManager ui
        {
            get
            {
                if (_uiManager == null)
                {
                    _uiManager = GetModule<UIManager>();
                }

                return _uiManager;
            }
        }

        /// <summary> webRequest框架模块 </summary>
        public static WebRequestManager web
        {
            get
            {
                if (_webRequestManager == null)
                {
                    _webRequestManager = GetModule<WebRequestManager>();
                }

                return _webRequestManager;
            }
        }

        /// <summary>
        /// 获取脚本中的模块，如果不存在则会新创建
        /// </summary>
        public static T GetModule<T>() where T : GameFrameworkModule
        {
            Type @class = typeof(T);
            string fullName = $"{@class.Namespace}.{@class.Name}";
            Type moduleType = Utility.Assembly.GetType(fullName);
            if (moduleType == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Can not find Game Framework module type '{0}'.",
                    fullName));
            }

            return GetModule(moduleType) as T;
        }

        public static bool HasModule<T>() where T : GameFrameworkModule
        {
            Type @class = typeof(T);
            string fullName = $"{@class.Namespace}.{@class.Name}";
            Type moduleType = Utility.Assembly.GetType(fullName);

            foreach (GameFrameworkModule module in _frameworkModules)
            {
                if (module.GetType() == moduleType)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 优先从缓存链表中拿取实例，没有则创建
        /// </summary>
        private static GameFrameworkModule GetModule(Type moduleType)
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
        private static GameFrameworkModule CreateModule(Type moduleType)
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

        private static void InitTextHelper()
        {
            var name = GameFrameworkConfig.Instance.textHelperTypeName;
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

        private static void InitVersionHelper()
        {
            var name = GameFrameworkConfig.Instance.versionHelperTypeName;
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

        private static void InitLogHelper()
        {
            var name = GameFrameworkConfig.Instance.logHelperTypeName;
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

        private static void InitCompressionHelper()
        {
            var name = GameFrameworkConfig.Instance.compressionHelperTypeName;
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

        private static void InitJsonHelper()
        {
            var name = GameFrameworkConfig.Instance.jsonHelperTypeName;
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
    }
}