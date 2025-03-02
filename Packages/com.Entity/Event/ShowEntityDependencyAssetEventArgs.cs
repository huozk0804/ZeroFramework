//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework.Entity
{
    /// <summary>
    /// 显示实体时加载依赖资源事件。
    /// </summary>
    public sealed class ShowEntityDependencyAssetEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化显示实体时加载依赖资源事件的新实例。
        /// </summary>
        public ShowEntityDependencyAssetEventArgs()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroupName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
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
        /// 获取被加载的依赖资源名称。
        /// </summary>
        public string DependencyAssetName { get; private set; }

        /// <summary>
        /// 获取当前已加载依赖资源数量。
        /// </summary>
        public int LoadedCount { get; private set; }

        /// <summary>
        /// 获取总共加载依赖资源数量。
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建显示实体时加载依赖资源事件。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <param name="entityGroupName">实体组名称。</param>
        /// <param name="dependencyAssetName">被加载的依赖资源名称。</param>
        /// <param name="loadedCount">当前已加载依赖资源数量。</param>
        /// <param name="totalCount">总共加载依赖资源数量。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的显示实体时加载依赖资源事件。</returns>
        public static ShowEntityDependencyAssetEventArgs Create(int entityId, string entityAssetName,
            string entityGroupName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            ShowEntityDependencyAssetEventArgs showEntityDependencyAssetEventArgs0 =
                ReferencePool.Acquire<ShowEntityDependencyAssetEventArgs>();
            showEntityDependencyAssetEventArgs0.EntityId = entityId;
            showEntityDependencyAssetEventArgs0.EntityAssetName = entityAssetName;
            showEntityDependencyAssetEventArgs0.EntityGroupName = entityGroupName;
            showEntityDependencyAssetEventArgs0.DependencyAssetName = dependencyAssetName;
            showEntityDependencyAssetEventArgs0.LoadedCount = loadedCount;
            showEntityDependencyAssetEventArgs0.TotalCount = totalCount;
            showEntityDependencyAssetEventArgs0.UserData = userData;
            return showEntityDependencyAssetEventArgs0;
        }

        /// <summary>
        /// 清理显示实体时加载依赖资源事件。
        /// </summary>
        public override void Clear()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroupName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}