//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework
{
    /// <summary>
    /// System.Byte 数组变量类。
    /// </summary>
    public sealed class VarByteArray : Variable<byte[]>
    {
        /// <summary>
        /// 初始化 System.Byte 数组变量类的新实例。
        /// </summary>
        public VarByteArray()
        {
        }

        /// <summary>
        /// 从 System.Byte 数组到 System.Byte 数组变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarByteArray(byte[] value)
        {
            VarByteArray varValue = ReferencePool.Acquire<VarByteArray>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.Byte 数组变量类到 System.Byte 数组的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator byte[](VarByteArray value)
        {
            return value.Value;
        }
    }
}
