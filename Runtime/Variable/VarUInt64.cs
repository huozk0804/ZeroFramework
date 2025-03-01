﻿//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework
{
    /// <summary>
    /// System.UInt64 变量类。
    /// </summary>
    public sealed class VarUInt64 : Variable<ulong>
    {
        /// <summary>
        /// 初始化 System.UInt64 变量类的新实例。
        /// </summary>
        public VarUInt64()
        {
        }

        /// <summary>
        /// 从 System.UInt64 到 System.UInt64 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarUInt64(ulong value)
        {
            VarUInt64 varValue = ReferencePool.Acquire<VarUInt64>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.UInt64 变量类到 System.UInt64 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator ulong(VarUInt64 value)
        {
            return value.Value;
        }
    }
}
