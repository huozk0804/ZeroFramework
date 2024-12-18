﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

namespace ZeroFramework.Resource
{
    /// <summary>
    /// 检查版本资源列表结果。
    /// </summary>
    public enum CheckVersionListResult : byte
    {
        /// <summary>
        /// 已经是最新的。
        /// </summary>
        Updated = 0,

        /// <summary>
        /// 需要更新。
        /// </summary>
        NeedUpdate
    }
}