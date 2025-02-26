using UnityEditor;
using ZeroFramework.UI;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private SerializedProperty _enableOpenUIFormSuccessEvent = null;
        private SerializedProperty _enableOpenUIFormFailureEvent = null;
        private SerializedProperty _enableOpenUIFormUpdateEvent = null;
        private SerializedProperty _enableOpenUIFormDependencyAssetEvent = null;
        private SerializedProperty _enableCloseUIFormCompleteEvent = null;
        private SerializedProperty _instanceAutoReleaseInterval = null;
        private SerializedProperty _instanceCapacity = null;
        private SerializedProperty _instanceExpireTime = null;
        private SerializedProperty _instancePriority = null;
        private SerializedProperty _uiGroups = null;

        private readonly HelperInfo<UIFormHelperBase> _uiFormHelperInfo = new HelperInfo<UIFormHelperBase>("uiForm");

        private readonly HelperInfo<UIGroupHelperBase>
            _uiGroupHelperInfo = new HelperInfo<UIGroupHelperBase>("uiGroup");

        [InspectorConfigInit]
        void UIInspectorInit()
        {
            _enableFunc.AddLast(OnUIEnable);
            _inspectorFunc.AddLast(OnUIInspectorGUI);
            _completeFunc.AddLast(OnUIComplete);
        }

        void OnUIEnable()
        {
            _enableOpenUIFormSuccessEvent = serializedObject.FindProperty("enableOpenUIFormSuccessEvent");
            _enableOpenUIFormFailureEvent = serializedObject.FindProperty("enableOpenUIFormFailureEvent");
            _enableOpenUIFormUpdateEvent = serializedObject.FindProperty("enableOpenUIFormUpdateEvent");
            _enableOpenUIFormDependencyAssetEvent =
                serializedObject.FindProperty("enableOpenUIFormDependencyAssetEvent");
            _enableCloseUIFormCompleteEvent = serializedObject.FindProperty("enableCloseUIFormCompleteEvent");
            _instanceAutoReleaseInterval = serializedObject.FindProperty("instanceAutoReleaseInterval");
            _instanceCapacity = serializedObject.FindProperty("instanceCapacity");
            _instanceExpireTime = serializedObject.FindProperty("instanceExpireTime");
            _instancePriority = serializedObject.FindProperty("instancePriority");
            _uiGroups = serializedObject.FindProperty("uiGroups");

            _uiFormHelperInfo.Init(serializedObject);
            _uiGroupHelperInfo.Init(serializedObject);

            _uiFormHelperInfo.Refresh();
            _uiGroupHelperInfo.Refresh();
        }

        void OnUIInspectorGUI()
        {
            EditorGUILayout.LabelField("UI", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(_enableOpenUIFormSuccessEvent);
                EditorGUILayout.PropertyField(_enableOpenUIFormFailureEvent);
                EditorGUILayout.PropertyField(_enableOpenUIFormUpdateEvent);
                EditorGUILayout.PropertyField(_enableOpenUIFormDependencyAssetEvent);
                EditorGUILayout.PropertyField(_enableCloseUIFormCompleteEvent);
            }
            EditorGUI.EndDisabledGroup();

            _instanceAutoReleaseInterval.floatValue = EditorGUILayout.DelayedFloatField(
                "Instance Auto Release Interval", _instanceAutoReleaseInterval.floatValue);
            _instanceCapacity.intValue =
                EditorGUILayout.DelayedIntField("Instance Capacity", _instanceCapacity.intValue);
            _instanceExpireTime.floatValue =
                EditorGUILayout.DelayedFloatField("Instance Expire Time", _instanceExpireTime.floatValue);
            _instancePriority.intValue =
                EditorGUILayout.DelayedIntField("Instance Priority", _instancePriority.intValue);

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                _uiFormHelperInfo.Draw();
                _uiGroupHelperInfo.Draw();
                EditorGUILayout.PropertyField(_uiGroups, true);
            }
            EditorGUI.EndDisabledGroup();
        }

        void OnUIComplete()
        {
            _uiFormHelperInfo.Refresh();
            _uiGroupHelperInfo.Refresh();
        }
    }
}