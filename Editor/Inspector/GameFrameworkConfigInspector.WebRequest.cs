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
            _inspectorFunc.AddLast(OnWebRequestInspectorGUI);
            _completeFunc.AddLast(OnWebRequestComplete);
        }

        void OnWebRequestInspectorGUI()
        {
            //WebRequestComponent t = (WebRequestComponent)target;
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

            //if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
            //{
            //    EditorGUILayout.LabelField("Total Agent Count", t.TotalAgentCount.ToString());
            //    EditorGUILayout.LabelField("Free Agent Count", t.FreeAgentCount.ToString());
            //    EditorGUILayout.LabelField("Working Agent Count", t.WorkingAgentCount.ToString());
            //    EditorGUILayout.LabelField("Waiting Agent Count", t.WaitingTaskCount.ToString());
            //    EditorGUILayout.BeginVertical("box");
            //    {
            //        TaskInfo[] webRequestInfos = t.GetAllWebRequestInfos();
            //        if (webRequestInfos.Length > 0)
            //        {
            //            foreach (TaskInfo webRequestInfo in webRequestInfos)
            //            {
            //                DrawWebRequestInfo(webRequestInfo);
            //            }

            //            if (GUILayout.Button("Export CSV Data"))
            //            {
            //                string exportFileName = EditorUtility.SaveFilePanel("Export CSV Data", string.Empty, "WebRequest Task Data.csv", string.Empty);
            //                if (!string.IsNullOrEmpty(exportFileName))
            //                {
            //                    try
            //                    {
            //                        int index = 0;
            //                        string[] data = new string[webRequestInfos.Length + 1];
            //                        data[index++] = "WebRequest Uri,Serial Id,Tag,Priority,Status";
            //                        foreach (TaskInfo webRequestInfo in webRequestInfos)
            //                        {
            //                            data[index++] = Utility.Text.Format("{0},{1},{2},{3},{4}", webRequestInfo.Description, webRequestInfo.SerialId, webRequestInfo.Tag ?? string.Empty, webRequestInfo.Priority, webRequestInfo.Status);
            //                        }

            //                        File.WriteAllLines(exportFileName, data, Encoding.UTF8);
            //                        Debug.Log(Utility.Text.Format("Export web request task CSV data to '{0}' success.", exportFileName));
            //                    }
            //                    catch (Exception exception)
            //                    {
            //                        Debug.LogError(Utility.Text.Format("Export web request task CSV data to '{0}' failure, exception is '{1}'.", exportFileName, exception));
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            GUILayout.Label("WebRequset Task is Empty ...");
            //        }
            //    }
            //    EditorGUILayout.EndVertical();
            //}
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

        private void DrawWebRequestInfo(TaskInfo webRequestInfo)
        {
            EditorGUILayout.LabelField(webRequestInfo.Description, Utility.Text.Format("[SerialId]{0} [Tag]{1} [Priority]{2} [Status]{3}", webRequestInfo.SerialId, webRequestInfo.Tag ?? "<None>", webRequestInfo.Priority, webRequestInfo.Status));
        }
    }
}