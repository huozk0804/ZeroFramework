using UnityEditor;
using ZeroFramework.DataTable;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private SerializedProperty m_EnableLoadDataTableUpdateEvent = null;
        private SerializedProperty m_EnableLoadDataTableDependencyAssetEvent = null;
        private SerializedProperty m_DataTableCachedBytesSize = null;

        private HelperInfo<DataTableHelperBase>
            m_DataTableHelperInfo = new HelperInfo<DataTableHelperBase>("DataTable");

        [InspectorConfigInit]
        void DataTableInspectorInit()
        {
            _enableFunc.AddLast(OnDataTableEnable);
            _inspectorFunc.AddLast(OnDataTableInspectorGUI);
            _completeFunc.AddLast(OnDataTableComplete);
        }

        void OnDataTableInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.LabelField("Data Table", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_EnableLoadDataTableUpdateEvent);
                EditorGUILayout.PropertyField(m_EnableLoadDataTableDependencyAssetEvent);
                m_DataTableHelperInfo.Draw();
                EditorGUILayout.PropertyField(m_DataTableCachedBytesSize);
            }
            EditorGUI.EndDisabledGroup();

            // if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            // {
            //     EditorGUILayout.LabelField("Data Table Count", t.Count.ToString());
            //     EditorGUILayout.LabelField("Cached Bytes Size", t.CachedBytesSize.ToString());
            //
            //     DataTableBase[] dataTables = t.GetAllDataTables();
            //     foreach (DataTableBase dataTable in dataTables)
            //     {
            //         DrawDataTable(dataTable);
            //     }
            // }
        }

        void OnDataTableEnable()
        {
            m_EnableLoadDataTableUpdateEvent = serializedObject.FindProperty("m_EnableLoadDataTableUpdateEvent");
            m_EnableLoadDataTableDependencyAssetEvent =
                serializedObject.FindProperty("m_EnableLoadDataTableDependencyAssetEvent");
            m_DataTableCachedBytesSize = serializedObject.FindProperty("m_DataTableCachedBytesSize");
            m_DataTableHelperInfo.Init(serializedObject);
            m_DataTableHelperInfo.Refresh();
        }

        void OnDataTableComplete()
        {
            m_DataTableHelperInfo.Refresh();
        }
    }
}