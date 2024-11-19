using System;
using UnityEngine;
using ZeroFramework.Config;
using ZeroFramework.Download;
using ZeroFramework.Entity;
using ZeroFramework.FileSystem;
using ZeroFramework.WebRequest;

namespace ZeroFramework
{
    [CreateAssetMenu(fileName = "GameFrameworkConfig", menuName = "Zero/Game Framework Config")]
    public sealed partial class GameFrameworkConfig : ScriptableObjectSingleton<GameFrameworkConfig>
    {
        //Base Config
        [SerializeField] public string m_TextHelperTypeName = "ZeroFramework.DefaultTextHelper";
        [SerializeField] public string m_VersionHelperTypeName = "ZeroFramework.DefaultVersionHelper";
        [SerializeField] public string m_LogHelperTypeName = "ZeroFramework.DefaultLogHelper";
        [SerializeField] public string m_CompressionHelperTypeName = "ZeroFramework.DefaultCompressionHelper";
        [SerializeField] public string m_JsonHelperTypeName = "ZeroFramework.DefaultJsonHelper";
        [SerializeField] public int m_FrameRate = 30;
        [SerializeField] public float m_GameSpeed = 1f;
        [SerializeField] public bool m_RunInBackground = true;
        [SerializeField] public bool m_NeverSleep = true;

        //Config
        [SerializeField] public bool m_EnableLoadConfigUpdateEvent = false;
        [SerializeField] public bool m_EnableLoadConfigDependencyAssetEvent = false;
        [SerializeField] public string m_ConfigHelperTypeName = "ZeroFramework.DefaultConfigHelper";
        [SerializeField] public ConfigHelperBase m_CustomConfigHelper = null;
        [SerializeField] public int m_ConfigCachedBytesSize = 0;

        //DataNode

        //DataTable
        [SerializeField] public bool m_EnableLoadDataTableUpdateEvent = false;
        [SerializeField] public bool m_EnableLoadDataTableDependencyAssetEvent = false;
        [SerializeField] public string m_DataTableHelperTypeName = "ZeroFramework.DefaultDataTableHelper";
        [SerializeField] public DataTableHelperBase m_CustomDataTableHelper = null;
        [SerializeField] public int m_DataTableCachedBytesSize = 0;

        //Download
        [SerializeField] public string m_DownloadAgentHelperTypeName = "ZeroFramework.UnityWebRequestDownloadAgentHelper";
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
            [SerializeField] public string m_Name = null;
            [SerializeField] public float m_InstanceAutoReleaseInterval = 60f;
            [SerializeField] public int m_InstanceCapacity = 16;
            [SerializeField] public float m_InstanceExpireTime = 60f;
            [SerializeField] public int m_InstancePriority = 0;
        }

        //fileSystem
        [SerializeField] public string m_FileSystemHelperTypeName = "ZeroFramework.DefaultFileSystemHelper";
        [SerializeField] public FileSystemHelperBase m_CustomFileSystemHelper = null;

        //referencePool
        [SerializeField] public ReferenceStrictCheckType m_EnableStrictCheck = ReferenceStrictCheckType.AlwaysEnable;

        //webRequest
        [SerializeField] public string m_WebRequestAgentHelperTypeName = "ZeroFramework.UnityWebRequestAgentHelper";
        [SerializeField] public WebRequestAgentHelperBase m_CustomWebRequestAgentHelper = null;
        [SerializeField] public int m_WebRequestAgentHelperCount = 1;
        [SerializeField] public float m_WebRequestTimeout = 30f;
    }
}