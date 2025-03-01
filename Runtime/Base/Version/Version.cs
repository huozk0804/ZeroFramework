//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework
{
    /// <summary>
    /// 版本号类。
    /// </summary>
    public static partial class Version
    {
        private const string FrameworkVersionString = "2021.12.29";
        private static IVersionHelper s_VersionHelper = null;

        /// <summary>
        /// 获取游戏框架版本号。
        /// </summary>
        public static string GameFrameworkVersion => FrameworkVersionString;

        /// <summary>
        /// 获取游戏版本号。
        /// </summary>
        public static string GameVersion
        {
            get
            {
                if (s_VersionHelper == null)
                    return string.Empty;
                return s_VersionHelper.GameVersion;
            }
        }

        public static string GameResVersion
        {
            get
            {
                if (s_VersionHelper == null)
                    return string.Empty;
                return s_VersionHelper.GameResVersion;
            }
        }

        /// <summary>
        /// 获取内部游戏版本号。
        /// </summary>
        public static int InternalGameVersion
        {
            get
            {
                if (s_VersionHelper == null)
                    return 0;
                return s_VersionHelper.InternalGameVersion;
            }
        }

        /// <summary>
        /// 设置版本号辅助器。
        /// </summary>
        /// <param name="versionHelper">要设置的版本号辅助器。</param>
        public static void SetVersionHelper(IVersionHelper versionHelper)
        {
            s_VersionHelper = versionHelper;
        }
    }
}
