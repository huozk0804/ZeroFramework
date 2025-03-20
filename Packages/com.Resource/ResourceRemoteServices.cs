using YooAsset;

namespace ZeroFramework.Resource
{
	/// <summary>
	/// Զ����Դ��ַ��ѯ������
	/// </summary>
	internal class ResourceRemoteServices : IRemoteServices
	{
		private readonly string _defaultHostServer;
		private readonly string _fallbackHostServer;

		public ResourceRemoteServices (string defaultHostServer, string fallbackHostServer) {
			_defaultHostServer = defaultHostServer;
			_fallbackHostServer = fallbackHostServer;
		}

		string IRemoteServices.GetRemoteMainURL (string fileName) {
			return $"{_defaultHostServer}/{fileName}";
		}

		string IRemoteServices.GetRemoteFallbackURL (string fileName) {
			return $"{_fallbackHostServer}/{fileName}";
		}
	}
}