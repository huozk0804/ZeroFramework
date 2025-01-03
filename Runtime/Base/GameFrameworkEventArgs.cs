﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

using System;

namespace ZeroFramework
{
    /// <summary>
    /// 游戏框架中包含事件数据的类的基类。
    /// </summary>
    public abstract class GameFrameworkEventArgs : EventArgs, IReference
    {
        /// <summary>
        /// 初始化游戏框架中包含事件数据的类的新实例。
        /// </summary>
        public GameFrameworkEventArgs()
        {
        }

        /// <summary>
        /// 清理引用。
        /// </summary>
        public abstract void Clear();
    }
}