using UnityEditor;
using ZeroFramework.Config;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private SerializedProperty _enableLoadConfigUpdateEvent = null;
        private SerializedProperty _enableLoadConfigDependencyAssetEvent = null;
        private SerializedProperty _configCachedBytesSize = null;
        private readonly HelperInfo<ConfigHelperBase> _configHelperInfo = new HelperInfo<ConfigHelperBase>("config");

        [InspectorConfigInit]
        void ConfigInspectorInit()
        {
            _enableFunc.AddLast(OnConfigEnable);
            _inspectorFunc.AddLast(OnConfigInspectorGUI);
            _completeFunc.AddLast(OnConfigComplete);
        }

        void OnConfigInspectorGUI()
        {
            GameFrameworkConfig t = (GameFrameworkConfig)target;
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.LabelField("Config Module", EditorStyles.boldLabel);
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.PropertyField(_enableLoadConfigUpdateEvent);
                    EditorGUILayout.PropertyField(_enableLoadConfigDependencyAssetEvent);
                    _configHelperInfo.Draw();
                    EditorGUILayout.PropertyField(_configCachedBytesSize);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        void OnConfigEnable()
        {
            _enableLoadConfigUpdateEvent = serializedObject.FindProperty("enableLoadConfigUpdateEvent");
            _enableLoadConfigDependencyAssetEvent =
                serializedObject.FindProperty("enableLoadConfigDependencyAssetEvent");
            _configCachedBytesSize = serializedObject.FindProperty("configCachedBytesSize");
            _configHelperInfo.Init(serializedObject);
            _configHelperInfo.Refresh();
        }

        void OnConfigComplete()
        {
            _configHelperInfo.Refresh();
        }
    }
}