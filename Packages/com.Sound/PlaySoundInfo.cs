//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using UnityEngine;

namespace ZeroFramework.Sound
{
    internal sealed class PlaySoundInfo : IReference
    {
        private Entity.Entity _bindingEntity;
        private Vector3 _worldPosition;
        private object _userData;

        public PlaySoundInfo()
        {
            _bindingEntity = null;
            _worldPosition = Vector3.zero;
            _userData = null;
        }

        public Entity.Entity BindingEntity => _bindingEntity;

        public Vector3 WorldPosition => _worldPosition;

        public object UserData => _userData;

        public static PlaySoundInfo Create(Entity.Entity bindingEntity, Vector3 worldPosition, object userData)
        {
            PlaySoundInfo playSoundInfo = ReferencePool.Acquire<PlaySoundInfo>();
            playSoundInfo._bindingEntity = bindingEntity;
            playSoundInfo._worldPosition = worldPosition;
            playSoundInfo._userData = userData;
            return playSoundInfo;
        }

        public void Clear()
        {
            _bindingEntity = null;
            _worldPosition = Vector3.zero;
            _userData = null;
        }
    }
}