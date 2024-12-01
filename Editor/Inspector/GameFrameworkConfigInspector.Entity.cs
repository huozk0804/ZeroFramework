using UnityEditor;
using ZeroFramework.Entity;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private SerializedProperty m_EnableShowEntityUpdateEvent = null;
        private SerializedProperty m_EnableShowEntityDependencyAssetEvent = null;
        private SerializedProperty m_EntityGroups = null;
        private HelperInfo<EntityHelperBase> m_EntityHelperInfo = new HelperInfo<EntityHelperBase>("Entity");
        private HelperInfo<EntityGroupHelperBase> m_EntityGroupHelperInfo = new HelperInfo<EntityGroupHelperBase>("EntityGroup");

        [InspectorConfigInit]
        void EntityInspectorInit()
        {
            _enableFunc.AddLast(OnEntityEnable);
            m_InspectorFunc.AddLast(OnEntityInspectorGUI);
            _completeFunc.AddLast(OnEntityComplete);
        }

        void OnEntityInspectorGUI()
        {
            //EntityComponent t = (EntityComponent)target;
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.LabelField("Entity", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_EnableShowEntityUpdateEvent);
                EditorGUILayout.PropertyField(m_EnableShowEntityDependencyAssetEvent);
                m_EntityHelperInfo.Draw();
                m_EntityGroupHelperInfo.Draw();
                EditorGUILayout.PropertyField(m_EntityGroups, true);
            }
            EditorGUI.EndDisabledGroup();
        }

        void OnEntityEnable()
        {
            m_EnableShowEntityUpdateEvent = serializedObject.FindProperty("m_EnableShowEntityUpdateEvent");
            m_EnableShowEntityDependencyAssetEvent =
                serializedObject.FindProperty("m_EnableShowEntityDependencyAssetEvent");
            m_EntityGroups = serializedObject.FindProperty("m_EntityGroups");
            m_EntityHelperInfo.Init(serializedObject);
            m_EntityGroupHelperInfo.Init(serializedObject);
            m_EntityHelperInfo.Refresh();
            m_EntityGroupHelperInfo.Refresh();
        }

        void OnEntityComplete()
        {
            m_EntityGroupHelperInfo.Refresh();
            m_EntityHelperInfo.Refresh();
        }
    }
}