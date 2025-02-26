using UnityEditor;
using ZeroFramework.Localization;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private SerializedProperty _enableLoadDictionaryUpdateEvent = null;
        private SerializedProperty _enableLoadDictionaryDependencyAssetEvent = null;
        private SerializedProperty _localizationCachedBytesSize = null;

        private readonly HelperInfo<LocalizationHelperBase> _localizationHelperInfo =
            new HelperInfo<LocalizationHelperBase>("localization");

        [InspectorConfigInit]
        void LocalizationInspectorInit()
        {
            _enableFunc.AddLast(OnLocalizationEnable);
            _inspectorFunc.AddLast(OnLocalizationInspectorGUI);
            _completeFunc.AddLast(OnLocalizationComplete);
        }

        void OnLocalizationEnable()
        {
            _enableLoadDictionaryUpdateEvent = serializedObject.FindProperty("enableLoadDictionaryUpdateEvent");
            _enableLoadDictionaryDependencyAssetEvent =
                serializedObject.FindProperty("enableLoadDictionaryDependencyAssetEvent");
            _localizationCachedBytesSize = serializedObject.FindProperty("localizationCachedBytesSize");

            _localizationHelperInfo.Init(serializedObject);
            _localizationHelperInfo.Refresh();
        }

        void OnLocalizationInspectorGUI()
        {
            EditorGUILayout.LabelField("Localization", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(_enableLoadDictionaryUpdateEvent);
                EditorGUILayout.PropertyField(_enableLoadDictionaryDependencyAssetEvent);
                _localizationHelperInfo.Draw();
                EditorGUILayout.PropertyField(_localizationCachedBytesSize);
            }
            EditorGUI.EndDisabledGroup();
        }

        void OnLocalizationComplete()
        {
            _localizationHelperInfo.Refresh();
        }
    }
}