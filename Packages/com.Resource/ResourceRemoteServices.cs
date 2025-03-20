using YooAsset;

namespace ZeroFramework.Resource
{
	/// <summary>
	/// 远端资源地址查询服务类
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