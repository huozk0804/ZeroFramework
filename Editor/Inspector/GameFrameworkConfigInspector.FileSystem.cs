using UnityEditor;
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
            m_InspectorFunc.AddLast(OnFileSystemInspectorGUI);
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
    }
}