using UnityEditor;
using ZeroFramework.Sound;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private SerializedProperty _enablePlaySoundUpdateEvent = null;
        private SerializedProperty _enablePlaySoundDependencyAssetEvent = null;
        private SerializedProperty _audioMixer = null;
        private SerializedProperty _soundGroups = null;

        private readonly HelperInfo<SoundHelperBase> _soundHelperInfo = new HelperInfo<SoundHelperBase>("sound");

        private readonly HelperInfo<SoundGroupHelperBase> _soundGroupHelperInfo =
            new HelperInfo<SoundGroupHelperBase>("soundGroup");

        private readonly HelperInfo<SoundAgentHelperBase> _soundAgentHelperInfo =
            new HelperInfo<SoundAgentHelperBase>("soundAgent");

        [InspectorConfigInit]
        void SoundInspectorInit()
        {
            _enableFunc.AddLast(OnSoundEnable);
            _inspectorFunc.AddLast(OnSoundInspectorGUI);
            _completeFunc.AddLast(OnSoundComplete);
        }

        void OnSoundEnable()
        {
            _enablePlaySoundUpdateEvent = serializedObject.FindProperty("enablePlaySoundUpdateEvent");
            _enablePlaySoundDependencyAssetEvent =
                serializedObject.FindProperty("enablePlaySoundDependencyAssetEvent");
            _audioMixer = serializedObject.FindProperty("audioMixer");
            _soundGroups = serializedObject.FindProperty("soundGroups");

            _soundHelperInfo.Init(serializedObject);
            _soundGroupHelperInfo.Init(serializedObject);
            _soundAgentHelperInfo.Init(serializedObject);
            _soundHelperInfo.Refresh();
            _soundGroupHelperInfo.Refresh();
            _soundAgentHelperInfo.Refresh();
        }

        void OnSoundInspectorGUI()
        {
            EditorGUILayout.LabelField("Sound", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(_enablePlaySoundUpdateEvent);
                EditorGUILayout.PropertyField(_enablePlaySoundDependencyAssetEvent);
                EditorGUILayout.PropertyField(_audioMixer);
                _soundHelperInfo.Draw();
                _soundGroupHelperInfo.Draw();
                _soundAgentHelperInfo.Draw();
                EditorGUILayout.PropertyField(_soundGroups, true);
            }
            EditorGUI.EndDisabledGroup();
        }

        void OnSoundComplete()
        {
            _soundHelperInfo.Refresh();
            _soundGroupHelperInfo.Refresh();
            _soundAgentHelperInfo.Refresh();
        }
    }
}