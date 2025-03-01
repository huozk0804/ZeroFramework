//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework.Entity
{
    /// <summary>
    /// 隐藏实体完成事件。
    /// </summary>
    public sealed class HideEntityCompleteEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化隐藏实体完成事件的新实例。
        /// </summary>
        public HideEntityCompleteEventArgs()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroup = null;
            UserData = null;
        }

        /// <summary>
        /// 获取实体编号。
        /// </summary>
        public int EntityId { get; private set; }

        /// <summary>
        /// 获取实体资源名称。
        /// </summary>
        public string EntityAssetName { get; private set; }

        /// <summary>
        /// 获取实体所属的实体组。
        /// </summary>
        public IEntityGroup EntityGroup { get; private set; }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建隐藏实体完成事件。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <param name="entityGroup">实体所属的实体组。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的隐藏实体完成事件。</returns>
        public static HideEntityCompleteEventArgs Create(int entityId, string entityAssetName, IEntityGroup entityGroup,
            object userData)
        {
            HideEntityCompleteEventArgs hideEntityCompleteEventArgs0 =
                ReferencePool.Acquire<HideEntityCompleteEventArgs>();
            hideEntityCompleteEventArgs0.EntityId = entityId;
            hideEntityCompleteEventArgs0.EntityAssetName = entityAssetName;
            hideEntityCompleteEventArgs0.EntityGroup = entityGroup;
            hideEntityCompleteEventArgs0.UserData = userData;
            return hideEntityCompleteEventArgs0;
        }

        /// <summary>
        /// 清理隐藏实体完成事件。
        /// </summary>
        public override void Clear()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroup = null;
            UserData = null;
        }
    }
}