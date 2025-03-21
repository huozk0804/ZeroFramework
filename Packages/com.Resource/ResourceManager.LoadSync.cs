using System;
using UnityEngine;
using YooAsset;

namespace ZeroFramework.Resource
{
	/// <summary>
	/// 同步加载资源接口
	/// </summary>
	public sealed partial class ResourceManager
	{
		/// <summary>
		/// ͬ��������Դ��
		/// </summary>
		/// <param name="location">��Դ�Ķ�λ��ַ��</param>
		/// <param name="packageName">ָ����Դ�������ơ�����ʹ��Ĭ����Դ��</param>
		/// <typeparam name="T">Ҫ������Դ�����͡�</typeparam>
		/// <returns>��Դʵ����</returns>
		public T LoadAsset<T> (string location, string packageName = "") where T : UnityEngine.Object {
			if (string.IsNullOrEmpty(location)) {
				throw new GameFrameworkException("Asset name is invalid.");
			}

			string assetObjectKey = GetCacheKey(location, packageName);
			AssetObject assetObject = _assetPool.Spawn(assetObjectKey);
			if (assetObject != null) {
				return assetObject.Target as T;
			}

			AssetHandle handle = GetHandleSync<T>(location, packageName: packageName);
			T ret = handle.AssetObject as T;
			assetObject = AssetObject.Create(assetObjectKey, handle.AssetObject, handle, this);
			_assetPool.Register(assetObject, true);

			return ret;
		}

		/// <summary>
		/// ͬ��������Ϸ���岢ʵ������
		/// </summary>
		/// <param name="location">��Դ�Ķ�λ��ַ��</param>
		/// <param name="parent">��Դʵ�����ڵ㡣</param>
		/// <param name="packageName">ָ����Դ�������ơ�����ʹ��Ĭ����Դ��</param>
		/// <returns>��Դʵ����</returns>
		/// <remarks>��ʵ������Դ����������������UnloadAsset��Destroyʱ�Զ�UnloadAsset��</remarks>
		public GameObject LoadGameObject (string location, Transform parent = null, string packageName = "") {
			if (string.IsNullOrEmpty(location)) {
				throw new GameFrameworkException("Asset name is invalid.");
			}

			string assetObjectKey = GetCacheKey(location, packageName);
			AssetObject assetObject = _assetPool.Spawn(assetObjectKey);
			if (assetObject != null) {
				return AssetsReference.Instantiate(assetObject.Target as GameObject, parent, this).gameObject;
			}

			AssetHandle handle = GetHandleSync<GameObject>(location, packageName: packageName);

			GameObject gameObject =
				AssetsReference.Instantiate(handle.AssetObject as GameObject, parent, this).gameObject;

			assetObject = AssetObject.Create(assetObjectKey, handle.AssetObject, handle, this);
			_assetPool.Register(assetObject, true);

			return gameObject;
		}

		/// <summary>
		/// ͬ����������Դ��
		/// </summary>
		/// <param name="location">��Դ�Ķ�λ��ַ��</param>
		/// <param name="packageName">ָ����Դ�������ơ�����ʹ��Ĭ����Դ��</param>
		/// <typeparam name="T">Ҫ������Դ�����͡�</typeparam>
		/// <returns>��Դʵ����</returns>
		public T LoadSubAsset<T> (string location, string packageName = "") where T : UnityEngine.Object {
			if (string.IsNullOrEmpty(location)) {
				throw new GameFrameworkException("Asset name is invalid.");
			}

			throw new NotImplementedException();
		}

		/// <summary>
		/// ͬ������ԭ����Դ
		/// </summary>
		/// <param name="location">��Դ�Ķ�λ��ַ��</param>
		/// <param name="packageName">ָ����Դ�������ơ�����ʹ��Ĭ����Դ��</param>
		/// <returns>��Դʵ����</returns>
		public byte[] LoadRawAsset (string location, string packageName = "") {
			if (string.IsNullOrEmpty(location)) {
				throw new GameFrameworkException("Asset name is invalid.");
			}

			string assetObjectKey = GetCacheKey(location, packageName);
			AssetObject assetObject = _assetPool.Spawn(assetObjectKey);
			if (assetObject != null) {
				return assetObject.Target as byte[];
			}

			RawFileHandle handle;
			if (string.IsNullOrEmpty(packageName)) {
				handle = YooAssets.LoadRawFileSync(location);
			} else {
				var package = YooAssets.GetPackage(packageName);
				handle = package.LoadRawFileSync(location);
			}

			byte[] rawData = handle.GetRawFileData();

			// 创建并注册资源对象
			assetObject = AssetObject.Create(assetObjectKey, rawData, handle, this);
			_assetPool.Register(assetObject, true);

			return rawData;
		}

		private AssetHandle GetHandleSync<T> (string location, string packageName = "") where T : UnityEngine.Object {
			return GetHandleSync(location, typeof(T), packageName);
		}

		private AssetHandle GetHandleSync (string location, Type assetType, string packageName = "") {
			if (string.IsNullOrEmpty(packageName)) {
				return YooAssets.LoadAssetSync(location, assetType);
			}

			var package = YooAssets.GetPackage(packageName);
			return package.LoadAssetSync(location, assetType);
		}
	}
}