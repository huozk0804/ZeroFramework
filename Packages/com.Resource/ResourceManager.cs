using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace ZeroFramework.Resource
{
	public partial class ResourceManager : GameFrameworkModule, IResourceManager
	{
		private int _downloadingMaxNum = 10;
		private float _minUnloadUnusedAssetsInterval = 60f;
		private float _maxUnloadUnusedAssetsInterval = 300f;
		private bool _useSystemUnloadUnusedAssets = true;
		private int _assetCapacity = 64;
		private float _assetExpireTime = 60f;
		private int _assetPriority = 0;

		private Dictionary<string, ResourcePackage> _packageMap = new Dictionary<string, ResourcePackage>();
		private readonly Dictionary<string, AssetInfo> _assetInfoMap = new Dictionary<string, AssetInfo>();
		private readonly HashSet<string> _assetLoadingList = new HashSet<string>();

		/// <summary>
		/// ��ǰ���µİ����汾��
		/// </summary>
		public string PackageVersion { set; get; }

		/// <summary>
		/// ��Դ�����ơ�
		/// </summary>
		public string PackageName = "DefaultPackage";

		/// <summary>
		/// �����첽ϵͳ������ÿִ֡�����ĵ����ʱ����Ƭ����λ�����룩
		/// </summary>
		[SerializeField] public long Milliseconds = 30;

		/// <summary>
		/// ��ȡ������ͬʱ���������Ŀ��
		/// </summary>
		public int DownloadingMaxNum {
			get => _downloadingMaxNum;
			set => _downloadingMaxNum = value;
		}

		/// <summary>
		/// ��ȡ��ǰ��Դ���õ���Ϸ�汾�š�
		/// </summary>
		public string ApplicableGameVersion { get; private set; }

		/// <summary>
		/// ��ȡ��ǰ�ڲ���Դ�汾�š�
		/// </summary>
		public int InternalResourceVersion { get; private set; }

		/// <summary>
		/// ��ȡ������������Դ�ͷŵ���С���ʱ�䣬����Ϊ��λ��
		/// </summary>
		public float MinUnloadUnusedAssetsInterval {
			get => _minUnloadUnusedAssetsInterval;
			set => _minUnloadUnusedAssetsInterval = value;
		}

		/// <summary>
		/// ��ȡ������������Դ�ͷŵ������ʱ�䣬����Ϊ��λ��
		/// </summary>
		public float MaxUnloadUnusedAssetsInterval {
			get => _maxUnloadUnusedAssetsInterval;
			set => _maxUnloadUnusedAssetsInterval = value;
		}

		/// <summary>
		/// ʹ��ϵͳ�ͷ�������Դ���ԡ�
		/// </summary>
		public bool UseSystemUnloadUnusedAssets {
			get => _useSystemUnloadUnusedAssets;
			set => _useSystemUnloadUnusedAssets = value;
		}

		/// <summary>
		/// ��ȡ��Ϸ���ģ�����ȼ���
		/// </summary>
		/// <remarks>���ȼ��ϸߵ�ģ���������ѯ�����ҹرղ��������С�</remarks>
		protected internal override int Priority => 4;

		public ResourceManager () {
			SetObjectPoolManager(Zero.objectPool);
		}

		protected internal override void Shutdown () {
			_packageMap.Clear();
			_assetPool = null;
			_assetLoadingList.Clear();
			_assetInfoMap.Clear();
		}

		protected internal override void Update (float elapseSeconds, float realElapseSeconds) {
		}

		/// <summary>
		/// �Ƿ���Ҫ��Զ�˸������ء�
		/// </summary>
		/// <param name="location">��Դ�Ķ�λ��ַ��</param>
		/// <param name="packageName">��Դ�����ơ�</param>
		public bool IsNeedDownloadFromRemote (string location, string packageName = "") {
			return false;
		}

		/// <summary>
		/// ��ȡ��Դ��Ϣ��
		/// </summary>
		/// <param name="location">��Դ�Ķ�λ��ַ��</param>
		/// <param name="packageName">��Դ�����ơ�</param>
		/// <returns>��Դ��Ϣ��</returns>
		public AssetInfo GetAssetInfo (string location, string packageName = "") {
			return null;
		}

		/// <summary>
		/// �����Դ�Ƿ���ڡ�
		/// </summary>
		/// <param name="location">��Դ��λ��ַ��</param>
		/// <param name="packageName">��Դ�����ơ�</param>
		/// <returns>�����Դ�Ƿ���ڵĽ����</returns>
		public HasAssetResult HasAsset (string location, string packageName = "") {
			return HasAssetResult.NotExist;
		}

		/// <summary>
		/// ͬ����ȡUnity��Դ
		/// </summary>
		/// <typeparam name="T">��Դ����</typeparam>
		/// <param name="location">��Դ��λ��ַ</param>
		/// <param name="packageName">��Դ������</param>
		/// <returns>��ȡ����Դ</returns>
		public T LoadAsset<T> (string location, string packageName = "") where T : UnityEngine.Object {
			return null;
		}

		/// <summary>
		/// ͬ����ȡԭ����Դ
		/// </summary>
		/// <typeparam name="T">��Դ����</typeparam>
		/// <param name="location">��Դ��λ��ַ</param>
		/// <param name="packageName">��Դ������</param>
		/// <returns>��ȡ����Դ</returns>
		public byte[] LoadRawAsset (string location, string packageName = "") {
			return null;
		}

		/// <summary>
		/// �첽������Դ��
		/// </summary>
		/// <param name="location">��Դ�Ķ�λ��ַ��</param>
		/// <param name="callback">�ص�������</param>
		/// <param name="packageName">ָ����Դ�������ơ�����ʹ��Ĭ����Դ��</param>
		/// <typeparam name="T">Ҫ������Դ�����͡�</typeparam>
		public async UniTaskVoid LoadAssetAsync<T> (string location, Action<T> callback, string packageName = "") where T : UnityEngine.Object {

		}

		//public async UniTaskVoid LoadRawAsset(string location, string packageName = "") {

		//}

		public async UniTaskVoid LoadSceneAsync(string location, string packageName = "") {
		
		}
	}
}