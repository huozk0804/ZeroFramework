//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework
{
    /// <summary>
    /// 网络消息包头接口。
    /// </summary>
    public interface IPacketHeader
    {
        /// <summary>
        /// 获取网络消息包长度。
        /// </summary>
        int PacketLength
        {
            get;
        }
    }
}
