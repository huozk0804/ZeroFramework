//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

using UnityEngine;

namespace ZeroFramework
{
    /// <summary>
    /// 默认版本号辅助器。
    /// </summary>
    public class DefaultVersionHelper : Version.IVersionHelper
    {
        /// <summary>
        /// 获取游戏版本号。
        /// </summary>
        public string GameVersion => Application.version;

        /// <summary>
        /// 游戏资源版本号
        /// </summary>
        public string GameResVersion => "0.1.0";

        /// <summary>
        /// 获取内部游戏版本号。
        /// </summary>
        public int InternalGameVersion => 0;
    }
}
