//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

using ZeroFramework;
using ZeroFramework.Event;

namespace ZeroFramework.Runtime
{
    /// <summary>
    /// 资源校验成功事件。
    /// </summary>
    public sealed class ResourceVerifySuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 资源校验成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ResourceVerifySuccessEventArgs).GetHashCode();

        /// <summary>
        /// 初始化资源校验成功事件的新实例。
        /// </summary>
        public ResourceVerifySuccessEventArgs()
        {
            Name = null;
            Length = 0;
        }

        /// <summary>
        /// 获取资源校验成功事件编号。
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 获取资源名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取资源大小。
        /// </summary>
        public int Length
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建资源校验成功事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的资源校验成功事件。</returns>
        public static ResourceVerifySuccessEventArgs Create(ZeroFramework.Resource.ResourceVerifySuccessEventArgs_0 e)
        {
            ResourceVerifySuccessEventArgs resourceVerifySuccessEventArgs = ReferencePool.Acquire<ResourceVerifySuccessEventArgs>();
            resourceVerifySuccessEventArgs.Name = e.Name;
            resourceVerifySuccessEventArgs.Length = e.Length;
            return resourceVerifySuccessEventArgs;
        }

        /// <summary>
        /// 清理资源校验成功事件。
        /// </summary>
        public override void Clear()
        {
            Name = null;
            Length = 0;
        }
    }
}
