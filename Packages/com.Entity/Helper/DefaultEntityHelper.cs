//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using UnityEngine;

namespace ZeroFramework.Entity
{
    /// <summary>
    /// 默认实体辅助器。
    /// </summary>
    public class DefaultEntityHelper : EntityHelperBase
    {
        //private ResourceManager _resourceComponent;

        /// <summary>
        /// 实例化实体。
        /// </summary>
        /// <param name="entityAsset">要实例化的实体资源。</param>
        /// <returns>实例化后的实体。</returns>
        public override object InstantiateEntity(object entityAsset)
        {
            return Instantiate((Object)entityAsset);
        }

        /// <summary>
        /// 创建实体。
        /// </summary>
        /// <param name="entityInstance">实体实例。</param>
        /// <param name="entityGroup">实体所属的实体组。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>实体。</returns>
        public override IEntity CreateEntity(object entityInstance, IEntityGroup entityGroup, object userData)
        {
            GameObject obj = entityInstance as GameObject;
            if (obj == null)
            {
                Log.Error("Entity instance is invalid.");
                return null;
            }

            Transform trans = obj.transform;
            trans.SetParent(((MonoBehaviour)entityGroup.Helper).transform);

            return obj.GetOrAddComponent<Entity>();
        }

        /// <summary>
        /// 释放实体。
        /// </summary>
        /// <param name="entityAsset">要释放的实体资源。</param>
        /// <param name="entityInstance">要释放的实体实例。</param>
        public override void ReleaseEntity(object entityAsset, object entityInstance)
        {
            //TODO:资源框架引用待修改
            // m_ResourceComponent.UnloadAsset(entityAsset);
            Destroy((Object)entityInstance);
        }

        private void Start()
        {
            //m_ResourceComponent = GameEntry.GetComponent<ResourceComponent>();
            //if (m_ResourceComponent == null)
            //{
            //    Log.Fatal("Resource component is invalid.");
            //}
        }
    }
}
