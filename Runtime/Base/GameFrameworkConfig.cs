//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.Audio;
using ZeroFramework.Config;
using ZeroFramework.Debugger;
using ZeroFramework.Entity;
using ZeroFramework.Localization;
using ZeroFramework.Setting;
using ZeroFramework.Sound;
using ZeroFramework.UI;

namespace ZeroFramework
{
    [CreateAssetMenu(fileName = "GameFrameworkConfig", menuName = "Zero/Game Framework Config", order = 100)]
    public sealed class GameFrameworkConfig : ScriptableObjectSingleton<GameFrameworkConfig>
    {
        //Base
        public bool editorResourceMode = true;
        public Language editorLanguage = Language.Unspecified;
        public string textHelperTypeName = "ZeroFramework.DefaultTextHelper";
        public string versionHelperTypeName = "ZeroFramework.DefaultVersionHelper";
        public string logHelperTypeName = "ZeroFramework.DefaultLogHelper";
        public string compressionHelperTypeName = "ZeroFramework.DefaultCompressionHelper";
        public string jsonHelperTypeName = "ZeroFramework.DefaultJsonHelper";
        public int frameRate = 30;
        public float gameSpeed = 1f;
        public bool runInBackground = true;
        public bool neverSleep = true;
        public string[] runtimeAssemblyNames;
        public string[] runtimeOrEditorAssemblyNames;

        //Download
        public string downloadAgentHelperTypeName = "ZeroFramework.UnityWebRequestDownloadAgentHelper";
        public DownloadAgentHelperBase downloadAgentCustomHelper = null;
        public int downloadAgentHelperCount = 3;
        public float downloadTimeout = 30f;
        public int flushSize = 1024 * 1024;

        //ReferencePool
        public ReferenceStrictCheckType enableStrictCheck = ReferenceStrictCheckType.AlwaysEnable;

        //WebRequest
        public string webRequestAgentHelperTypeName = "ZeroFramework.UnityWebRequestAgentHelper";
        public WebRequestAgentHelperBase webRequestAgentCustomHelper = null;
        public int webRequestAgentHelperCount = 1;
        public float webRequestTimeout = 30f;

		//Resources
		public ResourceHelperBase resourceCustomHelper = null;
		public string resourceHelperTypeName = "ZeroFramework.DefaultResourceHelper";
	

		#region com.Config

		public bool enableLoadConfigUpdateEvent = false;
        public bool enableLoadConfigDependencyAssetEvent = false;
        public string configHelperTypeName = "ZeroFramework.DefaultConfigHelper";
        public ConfigHelperBase configCustomHelper = null;
        public int configCachedBytesSize = 0;

        #endregion

        #region com.Debugger

        public GUISkin skin = null;
        public DebuggerActiveWindowType activeWindow = DebuggerActiveWindowType.AlwaysOpen;
        public bool showFullWindow = false;
        public ConsoleWindow consoleWindow = new ConsoleWindow();

        #endregion

        #region com.Entity

        public bool enableShowEntityUpdateEvent = false;
        public bool enableShowEntityDependencyAssetEvent = false;
        public string entityHelperTypeName = "ZeroFramework.DefaultEntityHelper";
        public EntityHelperBase entityCustomHelper = null;
        public string entityGroupHelperTypeName = "ZeroFramework.DefaultEntityGroupHelper";
        public EntityGroupHelperBase entityGroupCustomHelper = null;
        public EntityGroup[] entityGroups = null;

        [Serializable]
        public sealed class EntityGroup
        {
            public string name = null;
            public float instanceAutoReleaseInterval = 60f;
            public int instanceCapacity = 16;
            public float instanceExpireTime = 60f;
            public int instancePriority = 0;
        }

        #endregion

        #region com.Localization

        public bool enableLoadDictionaryUpdateEvent = false;
        public bool enableLoadDictionaryDependencyAssetEvent = false;
        public string localizationHelperTypeName = "ZeroFramework.Localization.DefaultLocalizationHelper";
        public LocalizationHelperBase localizationCustomHelper = null;
        public int localizationCachedBytesSize = 0;

        #endregion

        #region com.Scenes

        public bool enableLoadSceneUpdateEvent = true;
        public bool enableLoadSceneDependencyAssetEvent = true;

        #endregion

        #region com.Setting

        public string settingHelperTypeName = "ZeroFramework.Setting.DefaultSettingHelper";
        public SettingHelperBase settingCustomHelper = null;

        #endregion

        #region com.Sound

        public bool enablePlaySoundUpdateEvent = false;
        public bool enablePlaySoundDependencyAssetEvent = false;
        public AudioMixer audioMixer = null;
        public string soundHelperTypeName = "ZeroFramework.Sound.DefaultSoundHelper";
        public SoundHelperBase soundCustomHelper = null;
        public string soundGroupHelperTypeName = "ZeroFramework.Sound.DefaultSoundGroupHelper";
        public SoundGroupHelperBase soundGroupCustomHelper = null;
        public string soundAgentHelperTypeName = "ZeroFramework.Sound.DefaultSoundAgentHelper";
        public SoundAgentHelperBase soundAgentCustomHelper = null;
        public SoundGroup[] soundGroups = null;

        [Serializable]
        public sealed class SoundGroup
        {
            public string name = null;
            public bool avoidBeingReplacedBySamePriority = false;
            public bool mute = false;
            [Range(0f, 1f)] public float volume = 1f;
            public int agentHelperCount = 1;
        }

        #endregion

        #region com.UI

        public bool enableOpenUIFormSuccessEvent = true;
        public bool enableOpenUIFormFailureEvent = true;
        public bool enableOpenUIFormUpdateEvent = false;
        public bool enableOpenUIFormDependencyAssetEvent = false;
        public bool enableCloseUIFormCompleteEvent = true;
        public float instanceAutoReleaseInterval = 60f;
        public int instanceCapacity = 16;
        public float instanceExpireTime = 60f;
        public int instancePriority = 0;
        public string uiFormHelperTypeName = "ZeroFramework.UI.DefaultUIFormHelper";
        public UIPanelHelperBase uiFormCustomHelper = null;
        public string uiGroupHelperTypeName = "ZeroFramework.UI.DefaultUIGroupHelper";
        public UIGroupHelperBase uiGroupCustomHelper = null;
        public MonoBehaviour[] customBindingComponent = null;

        #endregion
    }
}