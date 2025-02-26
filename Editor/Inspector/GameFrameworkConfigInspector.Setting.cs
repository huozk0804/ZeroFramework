using UnityEditor;
using ZeroFramework.Setting;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private HelperInfo<SettingHelperBase> m_SettingHelperInfo = new HelperInfo<SettingHelperBase>("Setting");
        
        [InspectorConfigInit]
        void SettingInspectorInit()
        {
            _enableFunc.AddLast(OnSettingEnable);
            m_InspectorFunc.AddLast(OnSettingInspectorGUI);
            _completeFunc.AddLast(OnSettingComplete);
        }

        void OnSettingInspectorGUI()
        {
            EditorGUILayout.LabelField("Setting",EditorStyles.boldLabel);
            m_SettingHelperInfo.Draw();
        }

        void OnSettingEnable()
        {
            m_SettingHelperInfo.Init(serializedObject);
            m_SettingHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }

        void OnSettingComplete()
        {
            m_SettingHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }
    }
}