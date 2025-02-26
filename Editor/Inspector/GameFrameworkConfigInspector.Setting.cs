using UnityEditor;
using ZeroFramework.Setting;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private readonly HelperInfo<SettingHelperBase>
            _settingHelperInfo = new HelperInfo<SettingHelperBase>("setting");

        [InspectorConfigInit]
        void SettingInspectorInit()
        {
            _enableFunc.AddLast(OnSettingEnable);
            _inspectorFunc.AddLast(OnSettingInspectorGUI);
            _completeFunc.AddLast(OnSettingComplete);
        }

        void OnSettingInspectorGUI()
        {
            EditorGUILayout.LabelField("Setting", EditorStyles.boldLabel);
            _settingHelperInfo.Draw();
        }

        void OnSettingEnable()
        {
            _settingHelperInfo.Init(serializedObject);
            _settingHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }

        void OnSettingComplete()
        {
            _settingHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }
    }
}