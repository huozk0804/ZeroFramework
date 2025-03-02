//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework.Entity
{
    public sealed partial class EntityManager : GameFrameworkModule, IEntityManager
    {
        private sealed class ShowEntityInfo : IReference
        {
            private int m_SerialId;
            private int m_EntityId;
            private EntityGroup m_EntityGroup;
            private object m_UserData;

            public ShowEntityInfo()
            {
                m_SerialId = 0;
                m_EntityId = 0;
                m_EntityGroup = null;
                m_UserData = null;
            }

            public int SerialId => m_SerialId;

            public int EntityId => m_EntityId;

            public EntityGroup EntityGroup => m_EntityGroup;

            public object UserData => m_UserData;

            public static ShowEntityInfo Create(int serialId, int entityId, EntityGroup entityGroup, object userData)
            {
                ShowEntityInfo showEntityInfo = ReferencePool.Acquire<ShowEntityInfo>();
                showEntityInfo.m_SerialId = serialId;
                showEntityInfo.m_EntityId = entityId;
                showEntityInfo.m_EntityGroup = entityGroup;
                showEntityInfo.m_UserData = userData;
                return showEntityInfo;
            }

            public void Clear()
            {
                m_SerialId = 0;
                m_EntityId = 0;
                m_EntityGroup = null;
                m_UserData = null;
            }
        }
    }
}
