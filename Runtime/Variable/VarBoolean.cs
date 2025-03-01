﻿//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework
{
    /// <summary>
    /// System.Boolean 变量类。
    /// </summary>
    public sealed class VarBoolean : Variable<bool>
    {
        /// <summary>
        /// 初始化 System.Boolean 变量类的新实例。
        /// </summary>
        public VarBoolean()
        {
        }

        /// <summary>
        /// 从 System.Boolean 到 System.Boolean 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarBoolean(bool value)
        {
            VarBoolean varValue = ReferencePool.Acquire<VarBoolean>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.Boolean 变量类到 System.Boolean 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator bool(VarBoolean value)
        {
            return value.Value;
        }
    }
}
