namespace ZeroFramework.Resource
{
	public partial class ResourceManager
	{
		private IObjectPool<AssetObject> _assetPool;

		/// <summary>
		/// ��ȡ��������Դ������Զ��ͷſ��ͷŶ���ļ��������
		/// </summary>
		public float AssetAutoReleaseInterval {
			get => _assetPool.AutoReleaseInterval;
			set => _assetPool.AutoReleaseInterval = value;
		}

		/// <summary>
		/// ��ȡ��������Դ����ص�������
		/// </summary>
		public int AssetCapacity {
			get => _assetPool.Capacity;
			set => _assetPool.Capacity = value;
		}

		/// <summary>
		/// ��ȡ��������Դ����ض������������
		/// </summary>
		public float AssetExpireTime {
			get => _assetPool.ExpireTime;
			set => _assetPool.ExpireTime = value;
		}

		/// <summary>
		/// ��ȡ��������Դ����ص����ȼ���
		/// </summary>
		public int AssetPriority {
			get => _assetPool.Priority;
			set => _assetPool.Priority = value;
		}

		/// <summary>
		/// ж����Դ��
		/// </summary>
		/// <param name="asset">Ҫж�ص���Դ��</param>
		public void UnloadAsset (object asset) {
			if (_assetPool != null) {
				_assetPool.Unspawn(asset);
			}
		}

		/// <summary>
		/// ���ö���ع�������
		/// </summary>
		/// <param name="objectPoolManager">����ع�������</param>
		public void SetObjectPoolManager (IObjectPoolManager objectPoolManager) {
			if (objectPoolManager == null) {
				throw new GameFrameworkException("Object pool manager is invalid.");
			}
			_assetPool = objectPoolManager.CreateMultiSpawnObjectPool<AssetObject>("Asset Pool");
		}
	}
}