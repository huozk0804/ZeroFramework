using System;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace ZeroFramework.Resource
{
    public sealed class ResourceManager : GameFrameworkModule, IResourceManager
    {
        private IResourceHelper _resourceHelper;

        public ResourceManager()
        {
        }

        public override int Priority { get; } = 3;

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        public override void Shutdown()
        {
        }

        public HasAssetResult HasAsset(string location)
        {
            throw new NotImplementedException();
        }


        #region 同步加载资源接口

        public T LoadAsset<T>(string location, string packageName = "") where T : Object
        {
            if (_resourceHelper == null)
            {
                throw new GameFrameworkException("Resource Helper is not initialized.");
            }

            return _resourceHelper.LoadAsset<T>(location, packageName);
        }

		public T LoadSubAsset<T> (string location, string subName, string packageName = "") where T : Object {
			throw new NotImplementedException();
		}

		public Object LoadAsset(string location, Type type, string packageName = "")
        {
            throw new NotImplementedException();
        }

        public Object[] LoadSubAssets(string location, string packageName = "")
        {
            throw new NotImplementedException();
        }

        public byte[] LoadRawFile(string location, string packageName = "")
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 异步加载资源接口

        public UniTask<T> LoadAssetAsync<T>(string location, string packageName = "") where T : Object
        {
            throw new NotImplementedException();
        }

		public UniTask<T> LoadSubAssetAsync<T> (string location, string subName, string packageName = "") where T : Object {
			throw new NotImplementedException();
		}

		public UniTask LoadSubAssetsAsync(string location, string packageName = "")
        {
            throw new NotImplementedException();
        }

        public UniTask LoadSceneAsync(string location, string packageName = "")
        {
            throw new NotImplementedException();
        }

        public UniTask LoadRawFileAsync(string location, string packageName = "")
        {
            throw new NotImplementedException();
        }

		#endregion
	}
}