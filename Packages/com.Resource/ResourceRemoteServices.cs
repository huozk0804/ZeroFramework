using YooAsset;

namespace ZeroFramework.Resource
{
	/// <summary>
	/// 远端资源地址查询服务类
	/// </summary>
	internal class ResourceRemoteServices : IRemoteServices
	{
		private string channel => $"{Version.Platform}_{Version.Channel}";

		string IRemoteServices.GetRemoteMainURL (string fileName) {
			if (!Version.LocalLoaded && !Version.RemoteLoaded)
				throw new GameFrameworkException("No version file is loaded! need check.");

			return $"{Version.CdnUrl}/{channel}/{Version.GameVersion}/{Version.ResVersion}/{fileName}";
		}

		string IRemoteServices.GetRemoteFallbackURL (string fileName) {
			if (!Version.LocalLoaded && !Version.RemoteLoaded)
				throw new GameFrameworkException("No version file is loaded! need check.");

			return $"{Version.CdnUrl}/{channel}/{Version.GameVersion}/{Version.ResVersion}/{fileName}";
		}
	}
}