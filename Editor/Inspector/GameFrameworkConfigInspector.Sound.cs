using UnityEditor;
using ZeroFramework.Sound;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private SerializedProperty m_EnablePlaySoundUpdateEvent = null;
        private SerializedProperty m_EnablePlaySoundDependencyAssetEvent = null;
        private SerializedProperty m_AudioMixer = null;
        private SerializedProperty m_SoundGroups = null;

        private HelperInfo<SoundHelperBase> m_SoundHelperInfo = new HelperInfo<SoundHelperBase>("Sound");
        private HelperInfo<SoundGroupHelperBase> m_SoundGroupHelperInfo = new HelperInfo<SoundGroupHelperBase>("SoundGroup");
        private HelperInfo<SoundAgentHelperBase> m_SoundAgentHelperInfo = new HelperInfo<SoundAgentHelperBase>("SoundAgent");

        [InspectorConfigInit]
        void SoundInspectorInit()
        {
            _enableFunc.AddLast(OnSoundEnable);
            m_InspectorFunc.AddLast(OnSoundInspectorGUI);
            _completeFunc.AddLast(OnSoundComplete);
        }

        void OnSoundEnable()
        {
            m_EnablePlaySoundUpdateEvent = serializedObject.FindProperty("m_EnablePlaySoundUpdateEvent");
            m_EnablePlaySoundDependencyAssetEvent = serializedObject.FindProperty("m_EnablePlaySoundDependencyAssetEvent");
            m_AudioMixer = serializedObject.FindProperty("m_AudioMixer");
            m_SoundGroups = serializedObject.FindProperty("m_SoundGroups");

            m_SoundHelperInfo.Init(serializedObject);
            m_SoundGroupHelperInfo.Init(serializedObject);
            m_SoundAgentHelperInfo.Init(serializedObject);
            m_SoundHelperInfo.Refresh();
            m_SoundGroupHelperInfo.Refresh();
            m_SoundAgentHelperInfo.Refresh();
        }

        void OnSoundInspectorGUI()
        {
            EditorGUILayout.LabelField("Sound", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(m_EnablePlaySoundUpdateEvent);
                EditorGUILayout.PropertyField(m_EnablePlaySoundDependencyAssetEvent);
                EditorGUILayout.PropertyField(m_AudioMixer);
                m_SoundHelperInfo.Draw();
                m_SoundGroupHelperInfo.Draw();
                m_SoundAgentHelperInfo.Draw();
                EditorGUILayout.PropertyField(m_SoundGroups, true);
            }
            EditorGUI.EndDisabledGroup();
        }

        void OnSoundComplete()
        {
            m_SoundHelperInfo.Refresh();
            m_SoundGroupHelperInfo.Refresh();
            m_SoundAgentHelperInfo.Refresh();
        }
    }
}