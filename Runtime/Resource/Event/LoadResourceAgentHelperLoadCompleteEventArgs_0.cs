//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

namespace ZeroFramework.Resource
{
    /// <summary>
    /// 加载资源代理辅助器异步加载资源完成事件。
    /// </summary>
    public sealed class LoadResourceAgentHelperLoadCompleteEventArgs_0 : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载资源代理辅助器异步加载资源完成事件的新实例。
        /// </summary>
        public LoadResourceAgentHelperLoadCompleteEventArgs_0()
        {
            Asset = null;
        }

        /// <summary>
        /// 获取加载的资源。
        /// </summary>
        public object Asset { get; private set; }

        /// <summary>
        /// 创建加载资源代理辅助器异步加载资源完成事件。
        /// </summary>
        /// <param name="asset">加载的资源。</param>
        /// <returns>创建的加载资源代理辅助器异步加载资源完成事件。</returns>
        public static LoadResourceAgentHelperLoadCompleteEventArgs_0 Create(object asset)
        {
            LoadResourceAgentHelperLoadCompleteEventArgs_0 loadResourceAgentHelperLoadCompleteEventArgs =
                ReferencePool.Acquire<LoadResourceAgentHelperLoadCompleteEventArgs_0>();
            loadResourceAgentHelperLoadCompleteEventArgs.Asset = asset;
            return loadResourceAgentHelperLoadCompleteEventArgs;
        }

        /// <summary>
        /// 清理加载资源代理辅助器异步加载资源完成事件。
        /// </summary>
        public override void Clear()
        {
            Asset = null;
        }
    }
}