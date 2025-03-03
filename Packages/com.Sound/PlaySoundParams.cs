﻿//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework.Sound
{
    /// <summary>
    /// 播放声音参数。
    /// </summary>
    public sealed class PlaySoundParams : IReference
    {
        private bool _referenced;
        private float _time;
        private bool _muteInSoundGroup;
        private bool _loop;
        private int _priority;
        private float _volumeInSoundGroup;
        private float _fadeInSeconds;
        private float _pitch;
        private float _panStereo;
        private float _spatialBlend;
        private float _maxDistance;
        private float _dopplerLevel;

        /// <summary>
        /// 初始化播放声音参数的新实例。
        /// </summary>
        public PlaySoundParams()
        {
            _referenced = false;
            _time = Constant.DefaultTime;
            _muteInSoundGroup = Constant.DefaultMute;
            _loop = Constant.DefaultLoop;
            _priority = Constant.DefaultPriority;
            _volumeInSoundGroup = Constant.DefaultVolume;
            _fadeInSeconds = Constant.DefaultFadeInSeconds;
            _pitch = Constant.DefaultPitch;
            _panStereo = Constant.DefaultPanStereo;
            _spatialBlend = Constant.DefaultSpatialBlend;
            _maxDistance = Constant.DefaultMaxDistance;
            _dopplerLevel = Constant.DefaultDopplerLevel;
        }

        /// <summary>
        /// 获取或设置播放位置。
        /// </summary>
        public float Time
        {
            get => _time;
            set => _time = value;
        }

        /// <summary>
        /// 获取或设置在声音组内是否静音。
        /// </summary>
        public bool MuteInSoundGroup
        {
            get => _muteInSoundGroup;
            set => _muteInSoundGroup = value;
        }

        /// <summary>
        /// 获取或设置是否循环播放。
        /// </summary>
        public bool Loop
        {
            get => _loop;
            set => _loop = value;
        }

        /// <summary>
        /// 获取或设置声音优先级。
        /// </summary>
        public int Priority
        {
            get => _priority;
            set => _priority = value;
        }

        /// <summary>
        /// 获取或设置在声音组内音量大小。
        /// </summary>
        public float VolumeInSoundGroup
        {
            get => _volumeInSoundGroup;
            set => _volumeInSoundGroup = value;
        }

        /// <summary>
        /// 获取或设置声音淡入时间，以秒为单位。
        /// </summary>
        public float FadeInSeconds
        {
            get => _fadeInSeconds;
            set => _fadeInSeconds = value;
        }

        /// <summary>
        /// 获取或设置声音音调。
        /// </summary>
        public float Pitch
        {
            get => _pitch;
            set => _pitch = value;
        }

        /// <summary>
        /// 获取或设置声音立体声声相。
        /// </summary>
        public float PanStereo
        {
            get => _panStereo;
            set => _panStereo = value;
        }

        /// <summary>
        /// 获取或设置声音空间混合量。
        /// </summary>
        public float SpatialBlend
        {
            get => _spatialBlend;
            set => _spatialBlend = value;
        }

        /// <summary>
        /// 获取或设置声音最大距离。
        /// </summary>
        public float MaxDistance
        {
            get => _maxDistance;
            set => _maxDistance = value;
        }

        /// <summary>
        /// 获取或设置声音多普勒等级。
        /// </summary>
        public float DopplerLevel
        {
            get => _dopplerLevel;
            set => _dopplerLevel = value;
        }

        internal bool Referenced => _referenced;

        /// <summary>
        /// 创建播放声音参数。
        /// </summary>
        /// <returns>创建的播放声音参数。</returns>
        public static PlaySoundParams Create()
        {
            PlaySoundParams playSoundParams = ReferencePool.Acquire<PlaySoundParams>();
            playSoundParams._referenced = true;
            return playSoundParams;
        }

        /// <summary>
        /// 清理播放声音参数。
        /// </summary>
        public void Clear()
        {
            _time = Constant.DefaultTime;
            _muteInSoundGroup = Constant.DefaultMute;
            _loop = Constant.DefaultLoop;
            _priority = Constant.DefaultPriority;
            _volumeInSoundGroup = Constant.DefaultVolume;
            _fadeInSeconds = Constant.DefaultFadeInSeconds;
            _pitch = Constant.DefaultPitch;
            _panStereo = Constant.DefaultPanStereo;
            _spatialBlend = Constant.DefaultSpatialBlend;
            _maxDistance = Constant.DefaultMaxDistance;
            _dopplerLevel = Constant.DefaultDopplerLevel;
        }
    }
}