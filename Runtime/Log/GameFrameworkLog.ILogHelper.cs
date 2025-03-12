//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework
{
    public static partial class GameFrameworkLog
    {
        /// <summary>
        /// 游戏框架日志辅助器接口。
        /// </summary>
        public interface ILogHelper
        {
            /// <summary>
            /// 记录日志。
            /// </summary>
            /// <param name="level">游戏框架日志等级。</param>
            /// <param name="message">日志内容。</param>
            void Log(GameFrameworkLogLevel level, object message);

            /// <summary>
            /// 立即刷新缓冲区
            /// </summary>
            void Flush();
        }
    }
}
