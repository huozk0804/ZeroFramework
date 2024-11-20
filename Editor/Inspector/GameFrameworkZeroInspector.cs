using UnityEditor;
using ZeroFramework.Config;

namespace ZeroFramework.Editor
{
    [CustomEditor(typeof(Zero))]
    public sealed class GameFrameworkZeroInspector : GameFrameworkInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            Zero t = (Zero)target;

            //DataTableManager
            if (EditorApplication.isPlaying && Zero.Instance.HasModule<IDataTableManager>())
            {
                var config = Zero.Instance.DataTable;
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("DataTable", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("Data Table Count", config.Count.ToString());
                    EditorGUILayout.LabelField("Cached Bytes Size", config.CachedBytesSize.ToString());

                    DataTableBase[] dataTables = config.GetAllDataTables();
                    foreach (DataTableBase dataTable in dataTables)
                    {
                        DrawDataTable(dataTable);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        private void DrawDataTable(DataTableBase dataTable)
        {
            EditorGUILayout.LabelField(dataTable.FullName, Utility.Text.Format("{0} Rows", dataTable.Count));
        }
    }
}