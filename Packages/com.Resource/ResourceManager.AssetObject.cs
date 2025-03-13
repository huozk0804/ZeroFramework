using YooAsset;

namespace ZeroFramework.Resource
{
	public partial class ResourceManager
	{
		/// <summary>
		/// ��Դ����
		/// </summary>
		private sealed class AssetObject : ObjectBase
		{
			private AssetHandle _assetHandle;
			private ResourceManager _resourceManager;


			public AssetObject () {
				_assetHandle = null;
			}

			public static AssetObject Create (string name, object target, object assetHandle, ResourceManager resourceManager) {
				if (assetHandle == null) {
					throw new GameFrameworkException("Resource is invalid.");
				}

				if (resourceManager == null) {
					throw new GameFrameworkException("Resource Manager is invalid.");
				}

				AssetObject assetObject = ReferencePool.Acquire<AssetObject>();
				assetObject.Initialize(name, target);
				assetObject._assetHandle = (AssetHandle)assetHandle;
				assetObject._resourceManager = resourceManager;
				return assetObject;
			}

			public override void Clear () {
				base.Clear();
				_assetHandle = null;
			}

			protected internal override void OnUnspawn () {
				base.OnUnspawn();
			}

			protected internal override void Release (bool isShutdown) {
				if (!isShutdown) {
					AssetHandle handle = _assetHandle;
					if (handle is { IsValid: true }) {
						handle.Dispose();
					}
					handle = null;
				}
			}
		}
	}
}