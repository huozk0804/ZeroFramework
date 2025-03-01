﻿//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using UnityEngine;

namespace ZeroFramework
{
    /// <summary>
    /// UnityEngine.GameObject 变量类。
    /// </summary>
    public sealed class VarGameObject : Variable<GameObject>
    {
        /// <summary>
        /// 初始化 UnityEngine.GameObject 变量类的新实例。
        /// </summary>
        public VarGameObject()
        {
        }

        /// <summary>
        /// 从 UnityEngine.GameObject 到 UnityEngine.GameObject 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarGameObject(GameObject value)
        {
            VarGameObject varValue = ReferencePool.Acquire<VarGameObject>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 UnityEngine.GameObject 变量类到 UnityEngine.GameObject 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator GameObject(VarGameObject value)
        {
            return value.Value;
        }
    }
}
