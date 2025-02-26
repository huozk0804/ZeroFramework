using UnityEditor;
using ZeroFramework.Localization;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private SerializedProperty m_EnableLoadDictionaryUpdateEvent = null;
        private SerializedProperty m_EnableLoadDictionaryDependencyAssetEvent = null;
        private SerializedProperty m_LocalizationCachedBytesSize = null;
        private HelperInfo<LocalizationHelperBase> m_LocalizationHelperInfo = new HelperInfo<LocalizationHelperBase>("Localization");

        [InspectorConfigInit]
        void LocalizationInspectorInit()
        {
            _enableFunc.AddLast(OnLocalizationEnable);
            m_InspectorFunc.AddLast(OnLocalizationInspectorGUI);
            _completeFunc.AddLast(OnLocalizationComplete);
        }

        void OnLocalizationEnable()
        {
            m_EnableLoadDictionaryUpdateEvent = serializedObject.FindProperty("m_EnableLoadDictionaryUpdateEvent");
            m_EnableLoadDictionaryDependencyAssetEvent = serializedObject.FindProperty("m_EnableLoadDictionaryDependencyAssetEvent");
            m_LocalizationCachedBytesSize = serializedObject.FindProperty("m_LocalizationCachedBytesSize");

            m_LocalizationHelperInfo.Init(serializedObject);
            m_LocalizationHelperInfo.Refresh();
        }

        void OnLocalizationInspectorGUI()
        {
            //LocalizationComponent t = (LocalizationComponent)target;
            EditorGUILayout.LabelField("Localization", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(m_EnableLoadDictionaryUpdateEvent);
                EditorGUILayout.PropertyField(m_EnableLoadDictionaryDependencyAssetEvent);
                m_LocalizationHelperInfo.Draw();
                EditorGUILayout.PropertyField(m_LocalizationCachedBytesSize);
            }
            EditorGUI.EndDisabledGroup();
        }

        void OnLocalizationComplete()
        {
            m_LocalizationHelperInfo.Refresh();
        }
    }
}