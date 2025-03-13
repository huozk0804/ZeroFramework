using UnityEditor;
using ZeroFramework.WebRequest;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private SerializedProperty _webRequestAgentHelperCount = null;
        private SerializedProperty _webRequestTimeout = null;

        private readonly HelperInfo<WebRequestAgentHelperBase> _webRequestAgentHelperInfo =
            new HelperInfo<WebRequestAgentHelperBase>("webRequestAgent");

        [InspectorConfigInit]
        void WebRequestInspectorInit()
        {
            _enableFunc.AddLast(OnWebRequestEnable);
            _inspectorFunc.AddLast(OnWebRequestInspectorGUI);
            _completeFunc.AddLast(OnWebRequestComplete);
        }

        void OnWebRequestInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.LabelField("WebRequest", EditorStyles.boldLabel);
                _webRequestAgentHelperInfo.Draw();
                _webRequestAgentHelperCount.intValue = EditorGUILayout.IntSlider("Web Request Agent Helper Count",
                    _webRequestAgentHelperCount.intValue, 1, 16);
            }
            EditorGUI.EndDisabledGroup();

            _webRequestTimeout.floatValue = EditorGUILayout.Slider("Timeout", _downloadTimeout.floatValue, 0f, 120f);
        }

        void OnWebRequestEnable()
        {
            _webRequestAgentHelperCount = serializedObject.FindProperty("webRequestAgentHelperCount");
            _webRequestTimeout = serializedObject.FindProperty("webRequestTimeout");
            _webRequestAgentHelperInfo.Init(serializedObject);
            _webRequestAgentHelperInfo.Refresh();
        }

        void OnWebRequestComplete()
        {
            _webRequestAgentHelperInfo.Refresh();
        }
    }
}