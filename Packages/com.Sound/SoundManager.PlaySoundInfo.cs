﻿//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework.Sound
{
    public sealed partial class SoundManager : GameFrameworkModule, ISoundManager
    {
        private sealed class PlaySoundInfo : IReference
        {
            private int _serialId;
            private SoundGroup _soundGroup;
            private PlaySoundParams _playSoundParams;
            private object _userData;

            public PlaySoundInfo()
            {
                _serialId = 0;
                _soundGroup = null;
                _playSoundParams = null;
                _userData = null;
            }

            public int SerialId => _serialId;

            public SoundGroup SoundGroup => _soundGroup;

            public PlaySoundParams PlaySoundParams => _playSoundParams;

            public object UserData => _userData;

            public static PlaySoundInfo Create(int serialId, SoundGroup soundGroup, PlaySoundParams playSoundParams,
                object userData)
            {
                PlaySoundInfo playSoundInfo = ReferencePool.Acquire<PlaySoundInfo>();
                playSoundInfo._serialId = serialId;
                playSoundInfo._soundGroup = soundGroup;
                playSoundInfo._playSoundParams = playSoundParams;
                playSoundInfo._userData = userData;
                return playSoundInfo;
            }

            public void Clear()
            {
                _serialId = 0;
                _soundGroup = null;
                _playSoundParams = null;
                _userData = null;
            }
        }
    }
}