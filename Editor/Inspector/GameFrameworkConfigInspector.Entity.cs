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
            _inspectorFunc.AddLast(OnEntityInspectorGUI);
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

            //if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            //{
            //    EditorGUILayout.LabelField("Entity Group Count", t.EntityGroupCount.ToString());
            //    EditorGUILayout.LabelField("Entity Count (Total)", t.EntityCount.ToString());
            //    IEntityGroup[] entityGroups = t.GetAllEntityGroups();
            //    foreach (IEntityGroup entityGroup in entityGroups)
            //    {
            //        EditorGUILayout.LabelField(Utility.Text.Format("Entity Count ({0})", entityGroup.Name), entityGroup.EntityCount.ToString());
            //    }
            //}
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