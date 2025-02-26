﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

namespace ZeroFramework.Sound
{
    /// <summary>
    /// 声音辅助器接口。
    /// </summary>
    public interface ISoundHelper
    {
        /// <summary>
        /// 释放声音资源。
        /// </summary>
        /// <param name="soundAsset">要释放的声音资源。</param>
        void ReleaseSoundAsset(object soundAsset);
    }
}
