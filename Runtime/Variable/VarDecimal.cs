//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework
{
    /// <summary>
    /// System.Decimal 变量类。
    /// </summary>
    public sealed class VarDecimal : Variable<decimal>
    {
        /// <summary>
        /// 初始化 System.Decimal 变量类的新实例。
        /// </summary>
        public VarDecimal()
        {
        }

        /// <summary>
        /// 从 System.Decimal 到 System.Decimal 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarDecimal(decimal value)
        {
            VarDecimal varValue = ReferencePool.Acquire<VarDecimal>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.Decimal 变量类到 System.Decimal 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator decimal(VarDecimal value)
        {
            return value.Value;
        }
    }
}
