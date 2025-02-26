using UnityEditor;
using UnityEngine;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private SerializedProperty m_Skin = null;
        private SerializedProperty m_ActiveWindow = null;
        private SerializedProperty m_ShowFullWindow = null;
        private SerializedProperty m_ConsoleWindow = null;

        [InspectorConfigInit]
        void DebugConsoleInspectorInit()
        {
            _enableFunc.AddLast(OnDebuggerEnable);
            m_InspectorFunc.AddLast(OnDebuggerInspectorGUI);
            _completeFunc.AddLast(OnDebuggerComplete);
        }

        void OnDebuggerEnable()
        {
            m_Skin = serializedObject.FindProperty("m_Skin");
            m_ActiveWindow = serializedObject.FindProperty("m_ActiveWindow");
            m_ShowFullWindow = serializedObject.FindProperty("m_ShowFullWindow");
            m_ConsoleWindow = serializedObject.FindProperty("m_ConsoleWindow");
        }

        void OnDebuggerInspectorGUI()
        {
            EditorGUILayout.LabelField("Debugger", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_Skin);
            EditorGUILayout.PropertyField(m_ActiveWindow);
            EditorGUILayout.PropertyField(m_ShowFullWindow);
            EditorGUILayout.PropertyField(m_ConsoleWindow, true);
        }

        void OnDebuggerComplete()
        {
        }
    }
}