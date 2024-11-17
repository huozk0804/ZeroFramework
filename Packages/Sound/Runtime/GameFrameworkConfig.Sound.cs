using System;
using UnityEngine;
using UnityEngine.Audio;
using ZeroFramework.Sound;

namespace ZeroFramework
{
    public sealed partial class GameFrameworkConfig : ScriptableObjectSingleton<GameFrameworkConfig>
    {
        [SerializeField] private bool m_EnablePlaySoundUpdateEvent = false;
        [SerializeField] private bool m_EnablePlaySoundDependencyAssetEvent = false;
        [SerializeField] private AudioMixer m_AudioMixer = null;
        [SerializeField] private string m_SoundHelperTypeName = "ZeroFramework.Sound.DefaultSoundHelper";
        [SerializeField] private SoundHelperBase m_CustomSoundHelper = null;
        [SerializeField] private string m_SoundGroupHelperTypeName = "ZeroFramework.Sound.DefaultSoundGroupHelper";
        [SerializeField] private SoundGroupHelperBase m_CustomSoundGroupHelper = null;
        [SerializeField] private string m_SoundAgentHelperTypeName = "ZeroFramework.Sound.DefaultSoundAgentHelper";
        [SerializeField] private SoundAgentHelperBase m_CustomSoundAgentHelper = null;
        [SerializeField] public SoundGroup[] m_SoundGroups = null;

        [Serializable]
        public sealed class SoundGroup
        {
            [SerializeField] private string m_Name = null;
            [SerializeField] private bool m_AvoidBeingReplacedBySamePriority = false;
            [SerializeField] private bool m_Mute = false;
            [SerializeField, Range(0f, 1f)] private float m_Volume = 1f;
            [SerializeField] private int m_AgentHelperCount = 1;

            public string Name => m_Name;
            public bool AvoidBeingReplacedBySamePriority => m_AvoidBeingReplacedBySamePriority;
            public bool Mute => m_Mute;
            public float Volume => m_Volume;
            public int AgentHelperCount => m_AgentHelperCount;
        }
    }
}