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
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Linq;

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
		public static int FrameRate {
			get => _frameRate;
			set => Application.targetFrameRate = _frameRate = value;
		}

		/// <summary>
		/// 获取或设置游戏速度。
		/// </summary>
		public static float GameSpeed {
			get => _gameSpeed;
			set => Time.timeScale = _gameSpeed = value >= 0f ? value : 0f;
		}

		/// <summary>
		/// 获取或设置是否允许后台运行。
		/// </summary>
		public static bool RunInBackground {
			get => _runInBackground;
			set => Application.runInBackground = _runInBackground = value;
		}

		/// <summary>
		/// 获取或设置是否禁止休眠。
		/// </summary>
		public static bool NeverSleep {
			get => _neverSleep;
			set {
				_neverSleep = value;
				Screen.sleepTimeout = value ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
			}
		}

		/// <summary>
		/// 暂停游戏。
		/// </summary>
		public static void PauseGame () {
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
		public static void ResumeGame () {
			if (!IsGamePaused)
				return;

			GameSpeed = _gameSpeedBeforePause;
		}

		/// <summary>
		/// 重置为正常游戏速度。
		/// </summary>
		public static void ResetNormalGameSpeed () {
			if (IsNormalGameSpeed)
				return;

			GameSpeed = 1f;
		}

		#region Lift Function

		public static void Init () {
			if (_root == null) {
				_root = new GameObject("[ZERO]Framework");
				_root.AddComponent<ZeroFrameworkComponent>();
			}

			FrameRate = GameFrameworkConfig.Instance.frameRate;
			GameSpeed = GameFrameworkConfig.Instance.gameSpeed;
			RunInBackground = GameFrameworkConfig.Instance.runInBackground;
			NeverSleep = GameFrameworkConfig.Instance.neverSleep;

			//引用池设置
			var enableStrictCheck = GameFrameworkConfig.Instance.enableStrictCheck;
			switch (enableStrictCheck) {
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
			if (debuggerType != DebuggerActiveWindowType.AlwaysClose) {
				_root.GetOrAddComponent<DebuggerComponent>();
			}

#if UNITY_5_3_OR_NEWER || UNITY_5_3
			Utility.Converter.ScreenDpi = Screen.dpi;
			if (Utility.Converter.ScreenDpi <= 0) {
				Utility.Converter.ScreenDpi = DefaultDpi;
			}
#else
            Log.Error("Game Framework only applies with Unity 5.3 and above, but current Unity version is {0}.", Application.unityVersion);
			Application.Quit();
#endif

#if UNITY_5_6_OR_NEWER
			Application.lowMemory += OnLowMemory;
#endif

			Log.Info("Zero Framework Version: {0}", Version.ZeroFrameworkVersion);
			Log.Info("Unity Version: {0}", Application.unityVersion);
			Log.Info("Game Name: {0}", Application.productName);
			Log.Info("Zero Framework Launch Succeed.");
		}

		public static void Update (float deltaTime, float unscaledDeltaTime) {
			if (_IsExecuteListDirty) {
				_IsExecuteListDirty = false;
				BuildExecuteList();
			}

			for (int i = 0; i < _UpdateExecuteList.Count; i++) {
				_UpdateExecuteList[i].Update(deltaTime, unscaledDeltaTime);
			}
		}

		public static void OnDestroy () {
			Log.Info("Zero Framework is destroyed.");

			ReferencePool.ClearAll();
			Utility.Marshal.FreeCachedHGlobal();
			GameFrameworkLog.SetLogHelper(null);
		}

		/// <summary>
		/// 关闭框架
		/// </summary>
		public static void Shutdown () {
			Log.Info("Quit Zero Framework And Game.");

#if UNITY_5_6_OR_NEWER
			Application.lowMemory -= OnLowMemory;
#endif
			Object.DestroyImmediate(_root);
			_root = null;

			for (var current = _GameFrameworkModules.Last; current != null; current = current.Previous) {
				current.Value.Shutdown();
			}

			_GameFrameworkModules.Clear();
			_GameFrameworkModuleMaps.Clear();
			_UpdateExecuteList.Clear();

			Application.Quit();
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif
		}

		private static void OnLowMemory () {
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

		private const int DesignModuleCount = 16;
		private const string ModuleRootNameSpace = "GameFramework.";
		private static bool _IsExecuteListDirty;

		private static readonly List<GameFrameworkModule> _UpdateExecuteList = new List<GameFrameworkModule>(DesignModuleCount);
		private static readonly Dictionary<string, GameFrameworkModule> _GameFrameworkModuleMaps = new Dictionary<string, GameFrameworkModule>(DesignModuleCount);
		private static readonly GameFrameworkLinkedList<GameFrameworkModule> _GameFrameworkModules = new GameFrameworkLinkedList<GameFrameworkModule>();

		private static IDataNodeManager _dataNodeManager;
		private static IEventManager _eventManager;
		private static IFsmManager _fsmManager;
		private static IObjectPoolManager _objectPoolManager;
		private static IProcedureManager _procedureManager;
		private static ITimerManager _timerManager;
		private static IConfigManager _configManager;
		private static IDownloadManager _downloadManager;
		private static IEntityManager _entityManager;
		private static IFileSystemManager _fileSystemManager;
		private static ILocalizationManager _localizationManager;
		private static INetworkManager _networkManager;
		private static IResourceManager _resourceManager;
		private static IScenesManager _scenesManager;
		private static ISettingManager _settingManager;
		private static ISoundManager _soundManager;
		private static IUIManager _uiManager;
		private static IWebRequestManager _webRequestManager;

		/// <summary> 数据结点模块 </summary>
		public static IDataNodeManager dataNode {
			get {
				if (_dataNodeManager == null) {
					_dataNodeManager = GetModule<IDataNodeManager>();
				}

				return _dataNodeManager;
			}

			set {
				var type = value.GetType();
				var name = $"{type.Namespace}.{type.Name}";
				_dataNodeManager = RegisterModule<IDataNodeManager>(name);
			}
		}

		/// <summary> 事件订阅模块 </summary>
		public static IEventManager @event {
			get {
				if (_eventManager == null) {
					_eventManager = GetModule<IEventManager>();
				}

				return _eventManager;
			}

			set {
				var type = value.GetType();
				var name = $"{type.Namespace}.{type.Name}";
				_eventManager = RegisterModule<IEventManager>(name);
			}
		}

		/// <summary> 状态机模块 </summary>
		public static IFsmManager fsm {
			get {
				if (_fsmManager == null) {
					_fsmManager = GetModule<IFsmManager>();
				}

				return _fsmManager;
			}

			set {
				var type = value.GetType();
				var name = $"{type.Namespace}.{type.Name}";
				_fsmManager = RegisterModule<IFsmManager>(name);
			}
		}

		/// <summary> 对象池模块 </summary>
		public static IObjectPoolManager objectPool {
			get {
				if (_objectPoolManager == null) {
					_objectPoolManager = GetModule<IObjectPoolManager>();
				}

				return _objectPoolManager;
			}

			set {
				var type = value.GetType();
				var name = $"{type.Namespace}.{type.Name}";
				_objectPoolManager = RegisterModule<IObjectPoolManager>(name);
			}
		}

		/// <summary> 状态机模块 </summary>
		public static IProcedureManager procedure {
			get {
				if (_procedureManager == null) {
					_procedureManager = GetModule<IProcedureManager>();
				}

				return _procedureManager;
			}

			set {
				var type = value.GetType();
				var name = $"{type.Namespace}.{type.Name}";
				_procedureManager = RegisterModule<IProcedureManager>(name);
			}
		}

		/// <summary> 计时器模块 </summary>
		public static ITimerManager timer {
			get {
				if (_timerManager == null) {
					_timerManager = GetModule<ITimerManager>();
				}

				return _timerManager;
			}

			set {
				var type = value.GetType();
				var name = $"{type.Namespace}.{type.Name}";
				_timerManager = RegisterModule<ITimerManager>(name);
			}
		}

		/// <summary> 配置表模块 </summary>
		public static IConfigManager config {
			get {
				if (_configManager == null) {
					_configManager = GetModule<IConfigManager>();
				}

				return _configManager;
			}

			set {
				var type = value.GetType();
				var name = $"{type.Namespace}.{type.Name}";
				_configManager = RegisterModule<IConfigManager>(name);
			}
		}

		/// <summary> 下载模块 </summary>
		public static IDownloadManager download {
			get {
				if (_downloadManager == null) {
					_downloadManager = GetModule<IDownloadManager>();
				}

				return _downloadManager;
			}

			set {
				var type = value.GetType();
				var name = $"{type.Namespace}.{type.Name}";
				_downloadManager = RegisterModule<IDownloadManager>(name);
			}
		}

		/// <summary> 实体模块 </summary>
		public static IEntityManager entity {
			get {
				if (_entityManager == null) {
					_entityManager = GetModule<IEntityManager>();
				}

				return _entityManager;
			}

			set {
				var type = value.GetType();
				var name = $"{type.Namespace}.{type.Name}";
				_entityManager = RegisterModule<IEntityManager>(name);
			}
		}

		/// <summary> 文件系统模块 </summary>
		internal static IFileSystemManager file {
			get {
				if (_fileSystemManager == null) {
					_fileSystemManager = GetModule<IFileSystemManager>();
				}

				return _fileSystemManager;
			}

			set {
				var type = value.GetType();
				var name = $"{type.Namespace}.{type.Name}";
				_fileSystemManager = RegisterModule<IFileSystemManager>(name);
			}
		}

		/// <summary> 本地化模块 </summary>
		public static ILocalizationManager i10n {
			get {
				if (_localizationManager == null) {
					_localizationManager = GetModule<ILocalizationManager>();
				}

				return _localizationManager;
			}

			set {
				var type = value.GetType();
				var name = $"{type.Namespace}.{type.Name}";
				_localizationManager = RegisterModule<ILocalizationManager>(name);
			}
		}

		/// <summary> 网络模块 </summary>
		public static INetworkManager network {
			get {
				if (_networkManager == null) {
					_networkManager = GetModule<INetworkManager>();
				}

				return _networkManager;
			}
			set {
				var type = value.GetType();
				var name = $"{type.Namespace}.{type.Name}";
				_networkManager = RegisterModule<INetworkManager>(name);
			}
		}

		/// <summary> 资源模块 </summary>
		public static IResourceManager resource {
			get {
				if (_resourceManager == null) {
					_resourceManager = GetModule<IResourceManager>();
				}

				return _resourceManager;
			}

			set {
				var type = value.GetType();
				var name = $"{type.Namespace}.{type.Name}";
				_resourceManager = RegisterModule<IResourceManager>(name);
			}
		}

		/// <summary> 场景模块 </summary>
		public static IScenesManager scene {
			get {
				if (_scenesManager == null) {
					_scenesManager = GetModule<IScenesManager>();
				}

				return _scenesManager;
			}

			set {
				var type = value.GetType();
				var name = $"{type.Namespace}.{type.Name}";
				_scenesManager = RegisterModule<IScenesManager>(name);
			}
		}

		/// <summary> 存档模块 </summary>
		public static ISettingManager setting {
			get {
				if (_settingManager == null) {
					_settingManager = GetModule<ISettingManager>();
				}

				return _settingManager;
			}

			set {
				var type = value.GetType();
				var name = $"{type.Namespace}.{type.Name}";
				_settingManager = RegisterModule<ISettingManager>(name);
			}
		}

		/// <summary> 音频模块 </summary>
		public static ISoundManager audio {
			get {
				if (_soundManager == null) {
					_soundManager = GetModule<ISoundManager>();
				}

				return _soundManager;
			}

			set {
				var type = value.GetType();
				var name = $"{type.Namespace}.{type.Name}";
				_soundManager = RegisterModule<ISoundManager>(name);
			}
		}

		/// <summary> UI框架模块 </summary>
		public static IUIManager ui {
			get {
				if (_uiManager == null) {
					_uiManager = GetModule<IUIManager>();
				}

				return _uiManager;
			}

			set {
				var type = value.GetType();
				var name = $"{type.Namespace}.{type.Name}";
				_uiManager = RegisterModule<IUIManager>(name);
			}
		}

		/// <summary> webRequest框架模块 </summary>
		public static IWebRequestManager web {
			get {
				if (_webRequestManager == null) {
					_webRequestManager = GetModule<IWebRequestManager>();
				}

				return _webRequestManager;
			}
			set {
				var type = value.GetType();
				var name = $"{type.Namespace}.{type.Name}";
				_webRequestManager = RegisterModule<IWebRequestManager>(name);
			}
		}

		/// <summary>
		/// 注册新的Module
		/// </summary>
		/// <typeparam name="T">接口类型</typeparam>
		/// <param name="classFullName">类名称(命名空间,类名)</param>
		/// <returns></returns>
		public static T RegisterModule<T> (string classFullName) where T : class {
			Type @class = Type.GetType(classFullName);
			Type @inter = typeof(T);

			if (@class == null) {
				throw new GameFrameworkException(Utility.Text.Format("Can not find module type '{0}'.", classFullName));
			}

			if (@class.BaseType != typeof(GameFrameworkModule)) {
				throw new GameFrameworkException("Module must inherit from GameFrameworkModule");
			}

			var ins = @class.GetInterfaces()?.ToList();
			if (ins == null || !ins.Contains(@inter)) {
				throw new GameFrameworkException($"Module must implement {@inter.Name} interface");
			}

			if (_GameFrameworkModuleMaps.TryGetValue(@inter.Name, out var oldModule)) {
				oldModule.Shutdown();
				_GameFrameworkModules.Remove(oldModule);
			}

			return CreateModule(@class, @inter.Name) as T;
		}

		/// <summary>
		/// 获取游戏框架内置模块。
		/// </summary>
		/// <typeparam name="T">要获取的游戏框架模块类型</typeparam>
		/// <returns>要获取的游戏框架模块</returns>
		/// <remarks>如果要获取的游戏框架模块不存在，则自动创建该游戏框架模块</remarks>
		internal static T GetModule<T> () where T : class {
			Type @class = typeof(T);
			string moduleName = Utility.Text.Format("{0}.{1}", @class.Namespace, @class.Name.Substring(1));
			Type moduleType = Type.GetType(moduleName);
			if (moduleType == null) {
				throw new GameFrameworkException(Utility.Text.Format("Can not find module type '{0}'.", moduleName));
			}

			if (_GameFrameworkModuleMaps.TryGetValue(@class.Name, out GameFrameworkModule module)) {
				return module as T;
			} else {
				return CreateModule(moduleType, @class.Name) as T;
			}
		}

		/// <summary>
		/// 创建游戏框架模块
		/// </summary>
		/// <param name="moduleType">要创建的游戏框架模块类型</param>
		/// <param name="interName">接口的名称</param>
		/// <returns>要创建的游戏框架模块。</returns>
		private static GameFrameworkModule CreateModule (Type moduleType, string interName) {
			GameFrameworkModule module = (GameFrameworkModule)Activator.CreateInstance(moduleType);
			if (module == null) {
				throw new GameFrameworkException(Utility.Text.Format("Can not create module '{0}'.", moduleType.FullName));
			}

			_GameFrameworkModuleMaps[interName] = module;
			LinkedListNode<GameFrameworkModule> current = _GameFrameworkModules.First;
			while (current != null) {
				if (module.Priority > current.Value.Priority) {
					break;
				}

				current = current.Next;
			}

			if (current != null) {
				_GameFrameworkModules.AddBefore(current, module);
			} else {
				_GameFrameworkModules.AddLast(module);
			}

			_IsExecuteListDirty = true;
			return module;
		}

		private static void BuildExecuteList () {
			_UpdateExecuteList.Clear();
			foreach (var module in _GameFrameworkModules) {
				_UpdateExecuteList.Add(module);
			}
		}

		#endregion

		#region Base Helper

		private static void InitTextHelper () {
			var name = GameFrameworkConfig.Instance.textHelperTypeName;
			if (string.IsNullOrEmpty(name)) {
				return;
			}

			Type textHelperType = Utility.Assembly.GetType(name);
			if (textHelperType == null) {
				Log.Error("Can not find text helper type '{0}'.", name);
				return;
			}

			Utility.Text.ITextHelper textHelper = (Utility.Text.ITextHelper)Activator.CreateInstance(textHelperType);
			if (textHelper == null) {
				Log.Error("Can not create text helper instance '{0}'.", name);
				return;
			}

			Utility.Text.SetTextHelper(textHelper);
		}

		private static void InitVersionHelper () {
			var name = GameFrameworkConfig.Instance.versionHelperTypeName;
			if (string.IsNullOrEmpty(name)) {
				return;
			}

			Type versionHelperType = Utility.Assembly.GetType(name);
			if (versionHelperType == null) {
				throw new GameFrameworkException(Utility.Text.Format("Can not find version helper type '{0}'.",
					name));
			}

			Version.IVersionHelper versionHelper = (Version.IVersionHelper)Activator.CreateInstance(versionHelperType);
			if (versionHelper == null) {
				throw new GameFrameworkException(Utility.Text.Format("Can not create version helper instance '{0}'.",
					name));
			}

			Version.SetVersionHelper(versionHelper);
		}

		private static void InitLogHelper () {
			var name = GameFrameworkConfig.Instance.logHelperTypeName;
			if (string.IsNullOrEmpty(name)) {
				return;
			}

			Type logHelperType = Utility.Assembly.GetType(name);
			if (logHelperType == null) {
				throw new GameFrameworkException(Utility.Text.Format("Can not find log helper type '{0}'.", name));
			}

			GameFrameworkLog.ILogHelper
				logHelper = (GameFrameworkLog.ILogHelper)Activator.CreateInstance(logHelperType);
			if (logHelper == null) {
				throw new GameFrameworkException(Utility.Text.Format("Can not create log helper instance '{0}'.",
					name));
			}

			GameFrameworkLog.SetLogHelper(logHelper);
		}

		private static void InitCompressionHelper () {
			var name = GameFrameworkConfig.Instance.compressionHelperTypeName;
			if (string.IsNullOrEmpty(name)) {
				return;
			}

			Type compressionHelperType = Utility.Assembly.GetType(name);
			if (compressionHelperType == null) {
				Log.Error("Can not find compression helper type '{0}'.", name);
				return;
			}

			Utility.Compression.ICompressionHelper compressionHelper =
				(Utility.Compression.ICompressionHelper)Activator.CreateInstance(compressionHelperType);
			if (compressionHelper == null) {
				Log.Error("Can not create compression helper instance '{0}'.", name);
				return;
			}

			Utility.Compression.SetCompressionHelper(compressionHelper);
		}

		private static void InitJsonHelper () {
			var name = GameFrameworkConfig.Instance.jsonHelperTypeName;
			if (string.IsNullOrEmpty(name)) {
				return;
			}

			Type jsonHelperType = Utility.Assembly.GetType(name);
			if (jsonHelperType == null) {
				Log.Error("Can not find JSON helper type '{0}'.", name);
				return;
			}

			Utility.Json.IJsonHelper jsonHelper = (Utility.Json.IJsonHelper)Activator.CreateInstance(jsonHelperType);
			if (jsonHelper == null) {
				Log.Error("Can not create JSON helper instance '{0}'.", name);
				return;
			}

			Utility.Json.SetJsonHelper(jsonHelper);
		}

		#endregion
	}
}