using System;
using UnityEngine;
using UnityEngine.Audio;
using ZeroFramework.Config;
using ZeroFramework.Debugger;
using ZeroFramework.Download;
using ZeroFramework.Entity;
using ZeroFramework.Localization;
using ZeroFramework.Setting;
using ZeroFramework.Sound;
using ZeroFramework.UI;
using ZeroFramework.WebRequest;

namespace ZeroFramework
{
    [CreateAssetMenu(fileName = "GameFrameworkConfig", menuName = "Zero/Game Framework Config", order = 100)]
    public sealed partial class GameFrameworkConfig : ScriptableObjectSingleton<GameFrameworkConfig>
    {
        //Base
        [SerializeField] public bool m_EditorResourceMode = true;
        [SerializeField] public Language m_EditorLanguage = Language.Unspecified;
        [SerializeField] public string m_TextHelperTypeName = "ZeroFramework.DefaultTextHelper";
        [SerializeField] public string m_VersionHelperTypeName = "ZeroFramework.DefaultVersionHelper";
        [SerializeField] public string m_LogHelperTypeName = "ZeroFramework.DefaultLogHelper";
        [SerializeField] public string m_CompressionHelperTypeName = "ZeroFramework.DefaultCompressionHelper";
        [SerializeField] public string m_JsonHelperTypeName = "ZeroFramework.DefaultJsonHelper";
        [SerializeField] public int m_FrameRate = 30;
        [SerializeField] public float m_GameSpeed = 1f;
        [SerializeField] public bool m_RunInBackground = true;
        [SerializeField] public bool m_NeverSleep = true;
        [SerializeField] public string[] m_RuntimeAssemblyNames;
        [SerializeField] public string[] m_RuntimeOrEditorAssemblyNames;

        //Config
        [SerializeField] public bool m_EnableLoadConfigUpdateEvent = false;
        [SerializeField] public bool m_EnableLoadConfigDependencyAssetEvent = false;
        [SerializeField] public string m_ConfigHelperTypeName = "ZeroFramework.DefaultConfigHelper";
        [SerializeField] public ConfigHelperBase m_CustomConfigHelper = null;
        [SerializeField] public int m_ConfigCachedBytesSize = 0;

        //Debugger
        [SerializeField] public GUISkin m_Skin = null;
        [SerializeField] public DebuggerActiveWindowType m_ActiveWindow = DebuggerActiveWindowType.AlwaysOpen;
        [SerializeField] public bool m_ShowFullWindow = false;
        [SerializeField] public ConsoleWindow m_ConsoleWindow = new ConsoleWindow();

        //Download
        [SerializeField]
        public string m_DownloadAgentHelperTypeName = "ZeroFramework.UnityWebRequestDownloadAgentHelper";

        [SerializeField] public DownloadAgentHelperBase m_CustomDownloadAgentHelper = null;
        [SerializeField] public int m_DownloadAgentHelperCount = 3;
        [SerializeField] public float m_DownloadTimeout = 30f;
        [SerializeField] public int m_FlushSize = 1024 * 1024;

        //Entity
        [SerializeField] public bool m_EnableShowEntityUpdateEvent = false;
        [SerializeField] public bool m_EnableShowEntityDependencyAssetEvent = false;
        [SerializeField] public string m_EntityHelperTypeName = "ZeroFramework.DefaultEntityHelper";
        [SerializeField] public EntityHelperBase m_CustomEntityHelper = null;
        [SerializeField] public string m_EntityGroupHelperTypeName = "ZeroFramework.DefaultEntityGroupHelper";
        [SerializeField] public EntityGroupHelperBase m_CustomEntityGroupHelper = null;
        [SerializeField] public EntityGroup[] m_EntityGroups = null;

        [Serializable]
        public sealed class EntityGroup
        {
            [SerializeField] private string m_Name = null;
            [SerializeField] private float m_InstanceAutoReleaseInterval = 60f;
            [SerializeField] private int m_InstanceCapacity = 16;
            [SerializeField] private float m_InstanceExpireTime = 60f;
            [SerializeField] private int m_InstancePriority = 0;

            public string Name => m_Name;
            public float InstanceAutoReleaseInterval => m_InstanceAutoReleaseInterval;
            public int InstanceCapacity => m_InstanceCapacity;
            public float InstanceExpireTime => m_InstanceExpireTime;
            public int InstancePriority => m_InstancePriority;
        }
        
        //Localization
        [SerializeField] public bool m_EnableLoadDictionaryUpdateEvent = false;
        [SerializeField] public bool m_EnableLoadDictionaryDependencyAssetEvent = false;

        [SerializeField]
        public string m_LocalizationHelperTypeName = "ZeroFramework.Localization.DefaultLocalizationHelper";

        [SerializeField] public LocalizationHelperBase m_CustomLocalizationHelper = null;
        [SerializeField] public int m_LocalizationCachedBytesSize = 0;

        //ReferencePool
        [SerializeField] public ReferenceStrictCheckType m_EnableStrictCheck = ReferenceStrictCheckType.AlwaysEnable;

        //Scenes
        [SerializeField] public bool m_EnableLoadSceneUpdateEvent = true;
        [SerializeField] public bool m_EnableLoadSceneDependencyAssetEvent = true;

        //Setting
        [SerializeField] public string m_SettingHelperTypeName = "ZeroFramework.Setting.DefaultSettingHelper";
        [SerializeField] public SettingHelperBase m_CustomSettingHelper = null;

        //Sound
        [SerializeField] public bool m_EnablePlaySoundUpdateEvent = false;
        [SerializeField] public bool m_EnablePlaySoundDependencyAssetEvent = false;
        [SerializeField] public AudioMixer m_AudioMixer = null;
        [SerializeField] public string m_SoundHelperTypeName = "ZeroFramework.Sound.DefaultSoundHelper";
        [SerializeField] public SoundHelperBase m_CustomSoundHelper = null;
        [SerializeField] public string m_SoundGroupHelperTypeName = "ZeroFramework.Sound.DefaultSoundGroupHelper";
        [SerializeField] public SoundGroupHelperBase m_CustomSoundGroupHelper = null;
        [SerializeField] public string m_SoundAgentHelperTypeName = "ZeroFramework.Sound.DefaultSoundAgentHelper";
        [SerializeField] public SoundAgentHelperBase m_CustomSoundAgentHelper = null;
        [SerializeField] public SoundGroup[] m_SoundGroups = null;

        [Serializable]
        public sealed class SoundGroup
        {
            [SerializeField] private string m_Name = null;
            [SerializeField] private bool m_AvoidBeingReplacedBySamePriority = false;
            [SerializeField] private bool m_Mute = false;
            [SerializeField, Range(0f, 1f)] private float m_Volume = 1f;
            [SerializeField] private int m_AgentHelperCount = 1;

            public string Name => m_Name;
            public bool AvoidBeingReplacedBySamePriority => m_AvoidBeingReplacedBySamePriority;
            public bool Mute => m_Mute;
            public float Volume => m_Volume;
            public int AgentHelperCount => m_AgentHelperCount;
        }

        //UI
        [SerializeField] public bool m_EnableOpenUIFormSuccessEvent = true;
        [SerializeField] public bool m_EnableOpenUIFormFailureEvent = true;
        [SerializeField] public bool m_EnableOpenUIFormUpdateEvent = false;
        [SerializeField] public bool m_EnableOpenUIFormDependencyAssetEvent = false;
        [SerializeField] public bool m_EnableCloseUIFormCompleteEvent = true;
        [SerializeField] public float m_InstanceAutoReleaseInterval = 60f;
        [SerializeField] public int m_InstanceCapacity = 16;
        [SerializeField] public float m_InstanceExpireTime = 60f;
        [SerializeField] public int m_InstancePriority = 0;
        [SerializeField] public string m_UIFormHelperTypeName = "ZeroFramework.UI.DefaultUIFormHelper";
        [SerializeField] public UIFormHelperBase m_CustomUIFormHelper = null;
        [SerializeField] public string m_UIGroupHelperTypeName = "ZeroFramework.UI.DefaultUIGroupHelper";
        [SerializeField] public UIGroupHelperBase m_CustomUIGroupHelper = null;
        [SerializeField] public UIGroup[] m_UIGroups = null;

        [Serializable]
        public sealed class UIGroup
        {
            [SerializeField] private string m_Name = null;
            [SerializeField] private int m_Depth = 0;
            public string Name => m_Name;
            public int Depth => m_Depth;
        }

        //WebRequest
        [SerializeField] public string m_WebRequestAgentHelperTypeName = "ZeroFramework.WebRequest.UnityWebRequestAgentHelper";
        [SerializeField] public WebRequestAgentHelperBase m_CustomWebRequestAgentHelper = null;
        [SerializeField] public int m_WebRequestAgentHelperCount = 1;
        [SerializeField] public float m_WebRequestTimeout = 30f;
    }
}