//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

namespace ZeroFramework.Localization
{
    /// <summary>
    /// 本地化辅助器接口。
    /// </summary>
    public interface ILocalizationHelper
    {
        /// <summary>
        /// 获取系统语言。
        /// </summary>
        Language SystemLanguage { get; }
    }
}