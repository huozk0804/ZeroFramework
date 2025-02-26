//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

namespace ZeroFramework
{
    public static partial class Version
    {
        /// <summary>
        /// 版本号辅助器接口。
        /// </summary>
        public interface IVersionHelper
        {
            /// <summary>
            /// 获取游戏版本号。
            /// </summary>
            string GameVersion { get; }

            /// <summary>
            /// 游戏资源版本号
            /// </summary>
            string GameResVersion { get; }

            /// <summary>
            /// 获取内部游戏版本号。
            /// </summary>
            int InternalGameVersion { get; }
        }
    }
}