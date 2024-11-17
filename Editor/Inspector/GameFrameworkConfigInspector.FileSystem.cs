using UnityEditor;
using UnityEngine;
using ZeroFramework.Entity;
using ZeroFramework.FileSystem;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private HelperInfo<FileSystemHelperBase> m_FileSystemHelperInfo =
            new HelperInfo<FileSystemHelperBase>("FileSystem");

        [InspectorConfigInit]
        void FileSystemInspectorInit()
        {
            _enableFunc.AddLast(OnFileSystemEnable);
            _inspectorFunc.AddLast(OnFileSystemInspectorGUI);
            _completeFunc.AddLast(OnFileSystemComplete);
        }

        void OnFileSystemInspectorGUI()
        {
            //FileSystemComponent t = (FileSystemComponent)target;
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.LabelField("FileSystem", EditorStyles.boldLabel);
                m_FileSystemHelperInfo.Draw();
            }
            EditorGUI.EndDisabledGroup();

            //if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            //{
            //    EditorGUILayout.LabelField("File System Count", t.Count.ToString());

            //    IFileSystem[] fileSystems = t.GetAllFileSystems();
            //    foreach (IFileSystem fileSystem in fileSystems)
            //    {
            //        DrawFileSystem(fileSystem);
            //    }
            //}
        }

        void OnFileSystemEnable()
        {
            m_FileSystemHelperInfo.Init(serializedObject);
            m_FileSystemHelperInfo.Refresh();
        }

        void OnFileSystemComplete()
        {
            m_FileSystemHelperInfo.Refresh();
        }

        private void DrawFileSystem(IFileSystem fileSystem)
        {
            EditorGUILayout.LabelField(fileSystem.FullPath,
                Utility.Text.Format("{0}, {1} / {2} Files", fileSystem.Access, fileSystem.FileCount,
                    fileSystem.MaxFileCount));
        }
    }
}