using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;
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

		public string ApplicableGameVersion => throw new NotImplementedException();

		public int InternalResourceVersion => throw new NotImplementedException();

		public EPlayMode PlayMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public EVerifyLevel VerifyLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int DownloadingMaxNum { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int FailedTryAgain { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public string ReadOnlyPath => throw new NotImplementedException();

		public string ReadWritePath => throw new NotImplementedException();

		public string DefaultPackageName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public long Milliseconds { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public Transform InstanceRoot { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public string HostServerURL { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public float AssetAutoReleaseInterval { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int AssetCapacity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public float AssetExpireTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int AssetPriority { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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

		public void SetReadOnlyPath (string readOnlyPath) {
			throw new NotImplementedException();
		}

		public void SetReadWritePath (string readWritePath) {
			throw new NotImplementedException();
		}

		public void Initialize () {
			throw new NotImplementedException();
		}

		public UniTask<InitializationOperation> InitPackage (string packageName) {
			throw new NotImplementedException();
		}

		public void UnloadAsset (object asset) {
			throw new NotImplementedException();
		}

		public void UnloadUnusedAssets () {
			throw new NotImplementedException();
		}

		public void ForceUnloadAllAssets () {
			throw new NotImplementedException();
		}

		public HasAssetResult HasAsset (string location, string packageName = "") {
			throw new NotImplementedException();
		}

		public bool CheckLocationValid (string location, string packageName = "") {
			throw new NotImplementedException();
		}

		public AssetInfo[] GetAssetInfos (string resTag, string packageName = "") {
			throw new NotImplementedException();
		}

		public AssetInfo[] GetAssetInfos (string[] tags, string packageName = "") {
			throw new NotImplementedException();
		}

		public AssetInfo GetAssetInfo (string location, string packageName = "") {
			throw new NotImplementedException();
		}

		public void LoadAssetAsync (string location, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData, string packageName = "") {
			throw new NotImplementedException();
		}

		public void LoadAssetAsync (string location, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData, string packageName = "") {
			throw new NotImplementedException();
		}

		public T LoadAsset<T> (string location, string packageName = "") where T : Object {
			throw new NotImplementedException();
		}

		public GameObject LoadGameObject (string location, Transform parent = null, string packageName = "") {
			throw new NotImplementedException();
		}

		public UniTaskVoid LoadAsset<T> (string location, Action<T> callback, string packageName = "") where T : Object {
			throw new NotImplementedException();
		}

		public TObject[] LoadSubAssetsSync<TObject> (string location, string packageName = "") where TObject : Object {
			throw new NotImplementedException();
		}

		public UniTask<TObject[]> LoadSubAssetsAsync<TObject> (string location, string packageName = "") where TObject : Object {
			throw new NotImplementedException();
		}

		public UniTask<T> LoadAssetAsync<T> (string location, CancellationToken cancellationToken = default, string packageName = "") where T : Object {
			throw new NotImplementedException();
		}

		public UniTask<GameObject> LoadGameObjectAsync (string location, Transform parent = null, CancellationToken cancellationToken = default, string packageName = "") {
			throw new NotImplementedException();
		}
	}
}