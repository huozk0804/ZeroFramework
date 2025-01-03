﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

namespace ZeroFramework.Entity
{
    /// <summary>
    /// 显示实体更新事件。
    /// </summary>
    public sealed class ShowEntityUpdateEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化显示实体更新事件的新实例。
        /// </summary>
        public ShowEntityUpdateEventArgs()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroupName = null;
            Progress = 0f;
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
        /// 获取实体组名称。
        /// </summary>
        public string EntityGroupName { get; private set; }

        /// <summary>
        /// 获取显示实体进度。
        /// </summary>
        public float Progress { get; private set; }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建显示实体更新事件。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <param name="entityGroupName">实体组名称。</param>
        /// <param name="progress">显示实体进度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的显示实体更新事件。</returns>
        public static ShowEntityUpdateEventArgs Create(int entityId, string entityAssetName, string entityGroupName,
            float progress, object userData)
        {
            ShowEntityUpdateEventArgs showEntityUpdateEventArgs0 = ReferencePool.Acquire<ShowEntityUpdateEventArgs>();
            showEntityUpdateEventArgs0.EntityId = entityId;
            showEntityUpdateEventArgs0.EntityAssetName = entityAssetName;
            showEntityUpdateEventArgs0.EntityGroupName = entityGroupName;
            showEntityUpdateEventArgs0.Progress = progress;
            showEntityUpdateEventArgs0.UserData = userData;
            return showEntityUpdateEventArgs0;
        }

        /// <summary>
        /// 清理显示实体更新事件。
        /// </summary>
        public override void Clear()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroupName = null;
            Progress = 0f;
            UserData = null;
        }
    }
}