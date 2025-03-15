//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
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
            VersionInfo LoadLocalVersion (string path = null);
			VersionInfo GetRemoteVersion (string url);
            bool IsForceUpdate ();
            void WriteNewVersion (string path = null);
        }
    }
}