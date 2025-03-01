﻿//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using UnityEngine;

namespace ZeroFramework
{
    /// <summary>
    /// 默认游戏框架日志辅助器。
    /// </summary>
    public class DefaultLogHelper : GameFrameworkLog.ILogHelper
    {
        /// <summary>
        /// 记录日志。
        /// </summary>
        /// <param name="level">日志等级。</param>
        /// <param name="message">日志内容。</param>
        public void Log(GameFrameworkLogLevel level, object message)
        {
            switch (level)
            {
                case GameFrameworkLogLevel.Debug:
                    Debug.Log(Utility.Text.Format("<color=#888888>{0}</color>", message));
                    break;

                case GameFrameworkLogLevel.Info:
                    Debug.Log(message.ToString());
                    break;

                case GameFrameworkLogLevel.Warning:
                    Debug.LogWarning(message.ToString());
                    break;

                case GameFrameworkLogLevel.Error:
                    Debug.LogError(message.ToString());
                    break;

                default:
                    throw new GameFrameworkException(message.ToString());
            }
        }
    }
}
