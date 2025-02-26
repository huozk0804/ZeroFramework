﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

namespace ZeroFramework
{
    /// <summary>
    /// 关闭游戏框架类型。
    /// </summary>
    public enum ShutdownType : byte
    {
        /// <summary>
        /// 仅关闭游戏框架。
        /// </summary>
        None = 0,

        /// <summary>
        /// 关闭游戏框架并重启游戏。
        /// </summary>
        Restart,

        /// <summary>
        /// 关闭游戏框架并退出游戏。
        /// </summary>
        Quit,
    }
}
