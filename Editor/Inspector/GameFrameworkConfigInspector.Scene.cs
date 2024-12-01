using UnityEditor;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private SerializedProperty m_EnableLoadSceneUpdateEvent = null;
        private SerializedProperty m_EnableLoadSceneDependencyAssetEvent = null;

        [InspectorConfigInit]
        void SceneInspectorInit()
        {
            _enableFunc.AddLast(OnSceneEnable);
            m_InspectorFunc.AddLast(OnSceneInspectorGUI);
            _completeFunc.AddLast(OnSceneComplete);
        }

        void OnSceneEnable()
        {
            m_EnableLoadSceneUpdateEvent = serializedObject.FindProperty("m_EnableLoadSceneUpdateEvent");
            m_EnableLoadSceneDependencyAssetEvent =
                serializedObject.FindProperty("m_EnableLoadSceneDependencyAssetEvent");
        }

        void OnSceneInspectorGUI()
        {
            EditorGUILayout.LabelField("Scene", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(m_EnableLoadSceneUpdateEvent);
                EditorGUILayout.PropertyField(m_EnableLoadSceneDependencyAssetEvent);
            }
            EditorGUI.EndDisabledGroup();
        }

        void OnSceneComplete()
        {
        }
    }
}