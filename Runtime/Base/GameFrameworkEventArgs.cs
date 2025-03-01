//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
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