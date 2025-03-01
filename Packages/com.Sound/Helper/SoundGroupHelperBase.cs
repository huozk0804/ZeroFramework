//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using UnityEngine;
using UnityEngine.Audio;

namespace ZeroFramework.Sound
{
    /// <summary>
    /// 声音组辅助器基类。
    /// </summary>
    public abstract class SoundGroupHelperBase : MonoBehaviour, ISoundGroupHelper
    {
        [SerializeField]
        private AudioMixerGroup m_AudioMixerGroup = null;

        /// <summary>
        /// 获取或设置声音组辅助器所在的混音组。
        /// </summary>
        public AudioMixerGroup AudioMixerGroup
        {
            get => m_AudioMixerGroup;
            set => m_AudioMixerGroup = value;
        }
    }
}
