using UnityEditor;
using ZeroFramework.Download;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private SerializedProperty m_DownloadAgentHelperCount = null;
        private SerializedProperty m_DownloadTimeout = null;
        private SerializedProperty m_FlushSize = null;

        private HelperInfo<DownloadAgentHelperBase> m_DownloadAgentHelperInfo =
            new HelperInfo<DownloadAgentHelperBase>("DownloadAgent");

        [InspectorConfigInit]
        void DownloadInspectorInit()
        {
            _enableFunc.AddLast(OnDownloadEnable);
            m_InspectorFunc.AddLast(OnDownloadInspectorGUI);
            _completeFunc.AddLast(OnDownloadComplete);
        }

        void OnDownloadInspectorGUI()
        {
            EditorGUILayout.LabelField("Download", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                m_DownloadAgentHelperInfo.Draw();
                m_DownloadAgentHelperCount.intValue = EditorGUILayout.IntSlider("Download Agent Helper Count",
                    m_DownloadAgentHelperCount.intValue, 1, 16);
            }
            EditorGUI.EndDisabledGroup();

            float timeout = EditorGUILayout.Slider("Timeout", m_DownloadTimeout.floatValue, 0f, 120f);
            if (timeout != m_DownloadTimeout.floatValue)
            {
                if (EditorApplication.isPlaying)
                {
                    //t.Timeout = timeout;
                }
                else
                {
                    m_DownloadTimeout.floatValue = timeout;
                }
            }

            int flushSize = EditorGUILayout.DelayedIntField("Flush Size", m_FlushSize.intValue);
            if (flushSize != m_FlushSize.intValue)
            {
                if (EditorApplication.isPlaying)
                {
                    //t.FlushSize = flushSize;
                }
                else
                {
                    m_FlushSize.intValue = flushSize;
                }
            }
        }

        void OnDownloadEnable()
        {
            m_DownloadAgentHelperCount = serializedObject.FindProperty("m_DownloadAgentHelperCount");
            m_DownloadTimeout = serializedObject.FindProperty("m_DownloadTimeout");
            m_FlushSize = serializedObject.FindProperty("m_FlushSize");
            m_DownloadAgentHelperInfo.Init(serializedObject);
            m_DownloadAgentHelperInfo.Refresh();
        }

        void OnDownloadComplete()
        {
            m_DownloadAgentHelperInfo.Refresh();
        }
    }
}