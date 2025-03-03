//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework.Entity
{
    public sealed partial class EntityManager : GameFrameworkModule, IEntityManager
    {
        /// <summary>
        /// 实体实例对象。
        /// </summary>
        private sealed class EntityInstanceObject : ObjectBase
        {
            private object _entityAsset;
            private IEntityHelper _entityHelper;

            public EntityInstanceObject()
            {
                _entityAsset = null;
                _entityHelper = null;
            }

            public static EntityInstanceObject Create(string name, object entityAsset, object entityInstance, IEntityHelper entityHelper)
            {
                if (entityAsset == null)
                {
                    throw new GameFrameworkException("Entity asset is invalid.");
                }

                if (entityHelper == null)
                {
                    throw new GameFrameworkException("Entity helper is invalid.");
                }

                EntityInstanceObject entityInstanceObject = ReferencePool.Acquire<EntityInstanceObject>();
                entityInstanceObject.Initialize(name, entityInstance);
                entityInstanceObject._entityAsset = entityAsset;
                entityInstanceObject._entityHelper = entityHelper;
                return entityInstanceObject;
            }

            public override void Clear()
            {
                base.Clear();
                _entityAsset = null;
                _entityHelper = null;
            }

            protected internal override void Release(bool isShutdown)
            {
                _entityHelper.ReleaseEntity(_entityAsset, Target);
            }
        }
    }
}
