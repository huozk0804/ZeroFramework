//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

namespace ZeroFramework.Resource
{
    /// <summary>
    /// 资源校验失败事件。
    /// </summary>
    public sealed class ResourceVerifyFailureEventArgs_0 : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化资源校验失败事件的新实例。
        /// </summary>
        public ResourceVerifyFailureEventArgs_0()
        {
            Name = null;
        }

        /// <summary>
        /// 获取资源名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建资源校验失败事件。
        /// </summary>
        /// <param name="name">资源名称。</param>
        /// <returns>创建的资源校验失败事件。</returns>
        public static ResourceVerifyFailureEventArgs_0 Create(string name)
        {
            ResourceVerifyFailureEventArgs_0 resourceVerifyFailureEventArgs = ReferencePool.Acquire<ResourceVerifyFailureEventArgs_0>();
            resourceVerifyFailureEventArgs.Name = name;
            return resourceVerifyFailureEventArgs;
        }

        /// <summary>
        /// 清理资源校验失败事件。
        /// </summary>
        public override void Clear()
        {
            Name = null;
        }
    }
}
