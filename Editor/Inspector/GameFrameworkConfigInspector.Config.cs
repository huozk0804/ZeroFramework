using UnityEditor;
using ZeroFramework.Config;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private SerializedProperty m_EnableLoadConfigUpdateEvent = null;
        private SerializedProperty m_EnableLoadConfigDependencyAssetEvent = null;
        private SerializedProperty m_ConfigCachedBytesSize = null;
        private HelperInfo<ConfigHelperBase> m_ConfigHelperInfo = new HelperInfo<ConfigHelperBase>("Config");

        [InspectorConfigInit]
        void ConfigInspectorInit()
        {
            _enableFunc.AddLast(OnConfigEnable);
            m_InspectorFunc.AddLast(OnConfigInspectorGUI);
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
                    EditorGUILayout.PropertyField(m_EnableLoadConfigUpdateEvent);
                    EditorGUILayout.PropertyField(m_EnableLoadConfigDependencyAssetEvent);
                    m_ConfigHelperInfo.Draw();
                    EditorGUILayout.PropertyField(m_ConfigCachedBytesSize);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUI.EndDisabledGroup();

            // if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            // {
            //     EditorGUILayout.LabelField("Config Count", t.Count.ToString());
            //     EditorGUILayout.LabelField("Cached Bytes Size", t.CachedBytesSize.ToString());
            // }

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        void OnConfigEnable()
        {
            m_EnableLoadConfigUpdateEvent = serializedObject.FindProperty("m_EnableLoadConfigUpdateEvent");
            m_EnableLoadConfigDependencyAssetEvent =
                serializedObject.FindProperty("m_EnableLoadConfigDependencyAssetEvent");
            m_ConfigCachedBytesSize = serializedObject.FindProperty("m_ConfigCachedBytesSize");
            m_ConfigHelperInfo.Init(serializedObject);
            m_ConfigHelperInfo.Refresh();
        }

        void OnConfigComplete()
        {
            m_ConfigHelperInfo.Refresh();
        }
    }
}