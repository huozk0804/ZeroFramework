using UnityEditor;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private SerializedProperty _downloadAgentHelperCount = null;
        private SerializedProperty _downloadTimeout = null;
        private SerializedProperty _flushSize = null;

        private readonly HelperInfo<DownloadAgentHelperBase> _downloadAgentHelperInfo =
            new HelperInfo<DownloadAgentHelperBase>("downloadAgent");

        [InspectorConfigInit]
        void DownloadInspectorInit()
        {
            _enableFunc.AddLast(OnDownloadEnable);
            _inspectorFunc.AddLast(OnDownloadInspectorGUI);
            _completeFunc.AddLast(OnDownloadComplete);
        }

        void OnDownloadInspectorGUI()
        {
            EditorGUILayout.LabelField("Download", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                _downloadAgentHelperInfo.Draw();
                _downloadAgentHelperCount.intValue = EditorGUILayout.IntSlider("Download Agent Helper Count",
                    _downloadAgentHelperCount.intValue, 1, 16);
            }
            EditorGUI.EndDisabledGroup();

            _downloadTimeout.floatValue = EditorGUILayout.Slider("Timeout", _downloadTimeout.floatValue, 0f, 120f);
            _flushSize.intValue = EditorGUILayout.DelayedIntField("Flush Size", _flushSize.intValue);
        }

        void OnDownloadEnable()
        {
            _downloadAgentHelperCount = serializedObject.FindProperty("downloadAgentHelperCount");
            _downloadTimeout = serializedObject.FindProperty("downloadTimeout");
            _flushSize = serializedObject.FindProperty("flushSize");
            _downloadAgentHelperInfo.Init(serializedObject);
            _downloadAgentHelperInfo.Refresh();
        }

        void OnDownloadComplete()
        {
            _downloadAgentHelperInfo.Refresh();
        }
    }
}