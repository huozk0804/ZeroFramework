using Cysharp.Threading.Tasks;
using UnityEngine;
using ZeroFramework.Resource;

namespace ZeroFramework
{
	public class DefaultResourceHelper : ResourceHelperBase
	{
		public override T LoadAsset<T> (string location, string packageName = "") {
			throw new System.NotImplementedException();
		}

		public override UniTask LoadAssetAsync<T> (string location, string packageName = "") {
			throw new System.NotImplementedException();
		}

		public override byte[] LoadRawFile (string location, string packageName = "") {
			throw new System.NotImplementedException();
		}

		public override UniTask LoadRawFileAsync (string location, string packageName = "") {
			throw new System.NotImplementedException();
		}

		public override UniTask LoadSceneAsync (string location, string packageName = "") {
			throw new System.NotImplementedException();
		}

		public override T LoadSubAsset<T> (string location, string subName, string packageName = "") {
			throw new System.NotImplementedException();
		}

		public override UniTask LoadSubAssetAsync<T> (string location, string subName, string packageName = "") {
			throw new System.NotImplementedException();
		}

		public override Object[] LoadSubAssets (string location, string packageName = "") {
			throw new System.NotImplementedException();
		}

		public override UniTask LoadSubAssetsAsync (string location, string packageName = "") {
			throw new System.NotImplementedException();
		}
	}
}