using UnityEngine;
using ZeroFramework.Localization;

namespace ZeroFramework
{
    public sealed partial class GameFrameworkConfig : ScriptableObjectSingleton<GameFrameworkConfig>
    {
        [SerializeField] private bool m_EnableLoadDictionaryUpdateEvent = false;
        [SerializeField] private bool m_EnableLoadDictionaryDependencyAssetEvent = false;
        [SerializeField] private string m_LocalizationHelperTypeName = "ZeroFramework.Localization.DefaultLocalizationHelper";
        [SerializeField] private LocalizationHelperBase m_CustomLocalizationHelper = null;
        [SerializeField] private int m_LocalizationCachedBytesSize = 0;
    }
}