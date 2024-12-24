//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

namespace ZeroFramework.Resource
{
    /// <summary>
    /// 加载资源代理辅助器异步读取资源二进制流完成事件。
    /// </summary>
    public sealed class LoadResourceAgentHelperReadBytesCompleteEventArgs_0 : GameFrameworkEventArgs
    {
        private byte[] m_Bytes;

        /// <summary>
        /// 初始化加载资源代理辅助器异步读取资源二进制流完成事件的新实例。
        /// </summary>
        public LoadResourceAgentHelperReadBytesCompleteEventArgs_0()
        {
            m_Bytes = null;
        }

        /// <summary>
        /// 创建加载资源代理辅助器异步读取资源二进制流完成事件。
        /// </summary>
        /// <param name="bytes">资源的二进制流。</param>
        /// <returns>创建的加载资源代理辅助器异步读取资源二进制流完成事件。</returns>
        public static LoadResourceAgentHelperReadBytesCompleteEventArgs_0 Create(byte[] bytes)
        {
            LoadResourceAgentHelperReadBytesCompleteEventArgs_0 loadResourceAgentHelperReadBytesCompleteEventArgs =
                ReferencePool.Acquire<LoadResourceAgentHelperReadBytesCompleteEventArgs_0>();
            loadResourceAgentHelperReadBytesCompleteEventArgs.m_Bytes = bytes;
            return loadResourceAgentHelperReadBytesCompleteEventArgs;
        }

        /// <summary>
        /// 清理加载资源代理辅助器异步读取资源二进制流完成事件。
        /// </summary>
        public override void Clear()
        {
            m_Bytes = null;
        }

        /// <summary>
        /// 获取资源的二进制流。
        /// </summary>
        /// <returns>资源的二进制流。</returns>
        public byte[] GetBytes()
        {
            return m_Bytes;
        }
    }
}