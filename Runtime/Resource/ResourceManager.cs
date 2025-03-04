using System;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace ZeroFramework
{
    public sealed class ResourceManager : GameFrameworkModule, IResourceManager
    {
        private IResourceHelper _resourceHelper;

        public ResourceManager()
        {
			ResourceHelperBase helper = Helper.CreateHelper(GameFrameworkConfig.Instance.resourceHelperTypeName,
				GameFrameworkConfig.Instance.resourceCustomHelper);
			if (helper == null) {
				Log.Error("Can not create resources config helper.");
				return;
			}
			SetResourceHelper(helper);
            _resourceHelper?.OnStart();
		}

        public override int Priority { get; } = 3;

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            _resourceHelper?.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        public override void Shutdown()
        {
            _resourceHelper?.OnShutdown();
        }

        public void SetResourceHelper(IResourceHelper helper) {
            if(helper == null) {
				throw new GameFrameworkException("Resource form helper is invalid.");
			}

            _resourceHelper = helper;
			_resourceHelper?.OnStart();
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