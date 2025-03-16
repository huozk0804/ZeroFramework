//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZeroFramework
{
    public static partial class Version
    {
        /// <summary>
        /// 版本号辅助器接口。
        /// </summary>
        public interface IVersionHelper
        {
            string VersionFileName { get; }

            VersionInfo LoadLocalVersion(string localPath = null);
            UniTask<VersionInfo> GetRemoteVersion(string url);
            bool IsForceUpdate(string localVer, string remoteVer);
            void SaveNewVersion(VersionInfo data, string path = null);
            VersionInfo ParseCustomString(string content);
            string ParseCustomVersion(VersionInfo data);
        }
    }
}