﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

namespace ZeroFramework
{
    /// <summary>
    /// 任务状态。
    /// </summary>
    public enum TaskStatus : byte
    {
        /// <summary>
        /// 未开始。
        /// </summary>
        Todo = 0,

        /// <summary>
        /// 执行中。
        /// </summary>
        Doing,

        /// <summary>
        /// 完成。
        /// </summary>
        Done
    }
}
