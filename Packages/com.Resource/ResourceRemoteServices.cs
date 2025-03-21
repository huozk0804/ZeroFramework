using YooAsset;

namespace ZeroFramework.Resource
{
	/// <summary>
	/// Զ����Դ��ַ��ѯ������
	/// </summary>
	internal class ResourceRemoteServices : IRemoteServices
	{
		private string Channel => $"{Version.Platform}_{Version.Channel}";

		string IRemoteServices.GetRemoteMainURL (string fileName) {
			if (!Version.LocalLoaded && !Version.RemoteLoaded)
				throw new GameFrameworkException("No version file is loaded! need check.");

			return $"{Version.CdnUrl}/{Channel}/{Version.GameVersion}/{Version.ResVersion}/{fileName}";
		}

		string IRemoteServices.GetRemoteFallbackURL (string fileName) {
			if (!Version.LocalLoaded && !Version.RemoteLoaded)
				throw new GameFrameworkException("No version file is loaded! need check.");

			return $"{Version.CdnUrl}/{Channel}/{Version.GameVersion}/{Version.ResVersion}/{fileName}";
		}
	}
}