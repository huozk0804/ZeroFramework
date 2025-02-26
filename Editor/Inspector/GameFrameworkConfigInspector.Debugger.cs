using UnityEditor;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private SerializedProperty _skin = null;
        private SerializedProperty _activeWindow = null;
        private SerializedProperty _showFullWindow = null;
        private SerializedProperty _consoleWindow = null;

        [InspectorConfigInit]
        void DebugConsoleInspectorInit()
        {
            _enableFunc.AddLast(OnDebuggerEnable);
            _inspectorFunc.AddLast(OnDebuggerInspectorGUI);
            _completeFunc.AddLast(OnDebuggerComplete);
        }

        void OnDebuggerEnable()
        {
            _skin = serializedObject.FindProperty("skin");
            _activeWindow = serializedObject.FindProperty("activeWindow");
            _showFullWindow = serializedObject.FindProperty("showFullWindow");
            _consoleWindow = serializedObject.FindProperty("consoleWindow");
        }

        void OnDebuggerInspectorGUI()
        {
            EditorGUILayout.LabelField("Debugger", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_skin);
            EditorGUILayout.PropertyField(_activeWindow);
            EditorGUILayout.PropertyField(_showFullWindow);
            EditorGUILayout.PropertyField(_consoleWindow, true);
        }

        void OnDebuggerComplete()
        {
        }
    }
}