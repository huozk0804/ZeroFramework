//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;

namespace ZeroFramework
{
	/// <summary>
	/// 版本号类。
	/// </summary>
	public static partial class Version
	{
		private const string FrameworkVersionString = "2021.12.29";
		private static IVersionHelper _VersionHelper = null;
		private static VersionInfo _LocalVersion = null;
		private static VersionInfo _RemoteVersion = null;

		/// <summary>
		/// 游戏框架版本号。
		/// </summary>
		public static string ZeroFrameworkVersion => FrameworkVersionString;

		/// <summary>
		/// 游戏版本号。
		/// </summary>
		public static string GameVersion {
			get {
				return _RemoteVersion != null ? _RemoteVersion.gameVersion : _LocalVersion.gameVersion;
			}
		}

		/// <summary>
		/// 资源版本号
		/// </summary>
		public static string ResVersion {
			get {
				return _RemoteVersion != null ? _RemoteVersion.resVersion : _LocalVersion.resVersion;
			}
		}

		/// <summary>
		/// 设置版本号辅助器。
		/// </summary>
		/// <param name="versionHelper">要设置的版本号辅助器。</param>
		public static void SetVersionHelper (IVersionHelper versionHelper) {
			_VersionHelper = versionHelper;
			_VersionHelper.LoadLocalVersion();
		}

		/// <summary>
		/// 检测是否需要强制更新
		/// </summary>
		/// <returns></returns>
		public static bool IsForceUpdate () {
			return false;
		}
	}
}
