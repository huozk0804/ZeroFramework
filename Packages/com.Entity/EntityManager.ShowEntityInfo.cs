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
            private int _serialId;
            private int _entityId;
            private EntityGroup _entityGroup;
            private object _userData;

            public ShowEntityInfo()
            {
                _serialId = 0;
                _entityId = 0;
                _entityGroup = null;
                _userData = null;
            }

            public int SerialId => _serialId;

            public int EntityId => _entityId;

            public EntityGroup EntityGroup => _entityGroup;

            public object UserData => _userData;

            public static ShowEntityInfo Create(int serialId, int entityId, EntityGroup entityGroup, object userData)
            {
                ShowEntityInfo showEntityInfo = ReferencePool.Acquire<ShowEntityInfo>();
                showEntityInfo._serialId = serialId;
                showEntityInfo._entityId = entityId;
                showEntityInfo._entityGroup = entityGroup;
                showEntityInfo._userData = userData;
                return showEntityInfo;
            }

            public void Clear()
            {
                _serialId = 0;
                _entityId = 0;
                _entityGroup = null;
                _userData = null;
            }
        }
    }
}
