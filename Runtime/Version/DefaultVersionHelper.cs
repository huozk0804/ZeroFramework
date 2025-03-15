//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework
{
    /// <summary>
    /// 默认版本号辅助器。
    /// </summary>
    public class DefaultVersionHelper : Version.IVersionHelper
    {

		public Version.VersionInfo GetRemoteVersion (string url) {
			throw new System.NotImplementedException();
		}

		public bool IsForceUpdate () {
			if (Version.GameVersion.Equals(Version.GameVersion))
				return false;
			return true;
		}

		public Version.VersionInfo LoadLocalVersion (string path = null) {
			throw new System.NotImplementedException();
		}

		public void WriteNewVersion (string path = null) {
			throw new System.NotImplementedException();
		}
	}
}
