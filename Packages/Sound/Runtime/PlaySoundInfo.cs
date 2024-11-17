//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

using UnityEngine;

namespace ZeroFramework.Sound
{
    internal sealed class PlaySoundInfo : IReference
    {
        private Entity.Entity m_BindingEntity;
        private Vector3 m_WorldPosition;
        private object m_UserData;

        public PlaySoundInfo()
        {
            m_BindingEntity = null;
            m_WorldPosition = Vector3.zero;
            m_UserData = null;
        }

        public Entity.Entity BindingEntity => m_BindingEntity;

        public Vector3 WorldPosition => m_WorldPosition;

        public object UserData => m_UserData;

        public static PlaySoundInfo Create(Entity.Entity bindingEntity, Vector3 worldPosition, object userData)
        {
            PlaySoundInfo playSoundInfo = ReferencePool.Acquire<PlaySoundInfo>();
            playSoundInfo.m_BindingEntity = bindingEntity;
            playSoundInfo.m_WorldPosition = worldPosition;
            playSoundInfo.m_UserData = userData;
            return playSoundInfo;
        }

        public void Clear()
        {
            m_BindingEntity = null;
            m_WorldPosition = Vector3.zero;
            m_UserData = null;
        }
    }
}