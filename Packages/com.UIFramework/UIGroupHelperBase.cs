//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using UnityEngine;

namespace ZeroFramework.UI
{
    /// <summary>
    /// 界面组辅助器基类。
    /// </summary>
    public abstract class UIGroupHelperBase : MonoBehaviour, IUIGroupHelper
    {
        /// <summary>
        /// 设置界面组深度。
        /// </summary>
        /// <param name="depth">界面组深度。</param>
        public abstract void SetDepth(int depth);
    }
}
