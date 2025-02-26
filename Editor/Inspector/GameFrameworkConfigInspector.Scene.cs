using UnityEditor;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private SerializedProperty _enableLoadSceneUpdateEvent = null;
        private SerializedProperty _enableLoadSceneDependencyAssetEvent = null;

        [InspectorConfigInit]
        void SceneInspectorInit()
        {
            _enableFunc.AddLast(OnSceneEnable);
            _inspectorFunc.AddLast(OnSceneInspectorGUI);
            _completeFunc.AddLast(OnSceneComplete);
        }

        void OnSceneEnable()
        {
            _enableLoadSceneUpdateEvent = serializedObject.FindProperty("enableLoadSceneUpdateEvent");
            _enableLoadSceneDependencyAssetEvent =
                serializedObject.FindProperty("enableLoadSceneDependencyAssetEvent");
        }

        void OnSceneInspectorGUI()
        {
            EditorGUILayout.LabelField("Scene", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(_enableLoadSceneUpdateEvent);
                EditorGUILayout.PropertyField(_enableLoadSceneDependencyAssetEvent);
            }
            EditorGUI.EndDisabledGroup();
        }

        void OnSceneComplete()
        {
        }
    }
}