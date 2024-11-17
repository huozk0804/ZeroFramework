//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

namespace ZeroFramework.Resource
{
    /// <summary>
    /// 加载资源代理辅助器异步将资源文件转换为加载对象完成事件。
    /// </summary>
    public sealed class LoadResourceAgentHelperReadFileCompleteEventArgs_0 : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载资源代理辅助器异步将资源文件转换为加载对象完成事件的新实例。
        /// </summary>
        public LoadResourceAgentHelperReadFileCompleteEventArgs_0()
        {
            Resource = null;
        }

        /// <summary>
        /// 获取加载对象。
        /// </summary>
        public object Resource
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建加载资源代理辅助器异步将资源文件转换为加载对象完成事件。
        /// </summary>
        /// <param name="resource">资源对象。</param>
        /// <returns>创建的加载资源代理辅助器异步将资源文件转换为加载对象完成事件。</returns>
        public static LoadResourceAgentHelperReadFileCompleteEventArgs_0 Create(object resource)
        {
            LoadResourceAgentHelperReadFileCompleteEventArgs_0 loadResourceAgentHelperReadFileCompleteEventArgs = ReferencePool.Acquire<LoadResourceAgentHelperReadFileCompleteEventArgs_0>();
            loadResourceAgentHelperReadFileCompleteEventArgs.Resource = resource;
            return loadResourceAgentHelperReadFileCompleteEventArgs;
        }

        /// <summary>
        /// 清理加载资源代理辅助器异步将资源文件转换为加载对象完成事件。
        /// </summary>
        public override void Clear()
        {
            Resource = null;
        }
    }
}
