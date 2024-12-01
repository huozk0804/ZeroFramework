using UnityEditor;
using ZeroFramework.WebRequest;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private SerializedProperty m_WebRequestAgentHelperCount = null;
        private SerializedProperty m_WebRequestTimeout = null;
        private HelperInfo<WebRequestAgentHelperBase> m_WebRequestAgentHelperInfo = new HelperInfo<WebRequestAgentHelperBase>("WebRequestAgent");

        [InspectorConfigInit]
        void WebRequestInspectorInit()
        {
            _enableFunc.AddLast(OnWebRequestEnable);
            m_InspectorFunc.AddLast(OnWebRequestInspectorGUI);
            _completeFunc.AddLast(OnWebRequestComplete);
        }

        void OnWebRequestInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.LabelField("WebRequest", EditorStyles.boldLabel);
                m_WebRequestAgentHelperInfo.Draw();
                m_WebRequestAgentHelperCount.intValue = EditorGUILayout.IntSlider("Web Request Agent Helper Count", m_WebRequestAgentHelperCount.intValue, 1, 16);
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
                    m_WebRequestTimeout.floatValue = timeout;
                }
            }
        }

        void OnWebRequestEnable()
        {
            m_WebRequestAgentHelperCount = serializedObject.FindProperty("m_WebRequestAgentHelperCount");
            m_WebRequestTimeout = serializedObject.FindProperty("m_WebRequestTimeout");
            m_WebRequestAgentHelperInfo.Init(serializedObject);
            m_WebRequestAgentHelperInfo.Refresh();
        }

        void OnWebRequestComplete()
        {
            m_WebRequestAgentHelperInfo.Refresh();
        }
    }
}