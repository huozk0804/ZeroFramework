//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;

namespace ZeroFramework
{
	/// <summary>
	/// 版本号信息类。
	/// </summary>
	public static partial class Version
	{
		[Serializable]
		public abstract class VersionInfo
		{
			public string gameVersion;
			public string resVersion;
			public string cdnUrl;
			public string serverUrl;
			public string noticeUrl;
			public int platform;
			public int channel;
			public string[] resPackage;
			public string[] dllName;
			public string[] aotDllName;
		}
	}
}
