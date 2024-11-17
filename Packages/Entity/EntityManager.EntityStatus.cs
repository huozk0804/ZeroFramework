//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

namespace ZeroFramework.Entity
{
    public sealed partial class EntityManager : GameFrameworkModule, IEntityManager
    {
        /// <summary>
        /// 实体状态。
        /// </summary>
        private enum EntityStatus : byte
        {
            Unknown = 0,
            WillInit,
            Inited,
            WillShow,
            Showed,
            WillHide,
            Hidden,
            WillRecycle,
            Recycled
        }
    }
}
