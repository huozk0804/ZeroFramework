using System;
using UnityEngine;
using YooAsset;
using Object = UnityEngine.Object;

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
        public T LoadAsset<T>(string location, string packageName = "") where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            string assetObjectKey = GetCacheKey(location, packageName);
            AssetObject assetObject = _assetPool.Spawn(assetObjectKey);
            if (assetObject != null)
            {
                return assetObject.Target as T;
            }

            AssetHandle handle = GetHandleSync<T>(location, packageName: packageName);
            T ret = handle.AssetObject as T;
            assetObject = AssetObject.Create(assetObjectKey, handle.AssetObject, handle, this);
            _assetPool.Register(assetObject, true);

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="type"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string location, Type type, string packageName = "")
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        /// <exception cref="GameFrameworkException"></exception>
        public byte[] LoadRawAsset(string location, string packageName = "")
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            string assetObjectKey = GetCacheKey(location, packageName);
            AssetObject assetObject = _assetPool.Spawn(assetObjectKey);
            if (assetObject != null)
            {
                return assetObject.Target as byte[];
            }

            RawFileHandle handle;
            if (string.IsNullOrEmpty(packageName))
            {
                handle = YooAssets.LoadRawFileSync(location);
            }
            else
            {
                var package = YooAssets.GetPackage(packageName);
                handle = package.LoadRawFileSync(location);
            }

            byte[] rawData = handle.GetRawFileData();

            // 创建并注册资源对象
            assetObject = AssetObject.Create(assetObjectKey, rawData, handle, this);
            _assetPool.Register(assetObject, true);

            return rawData;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="parent"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        /// <exception cref="GameFrameworkException"></exception>
        public GameObject LoadGameObject(string location, Transform parent = null, string packageName = "")
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            string assetObjectKey = GetCacheKey(location, packageName);
            AssetObject assetObject = _assetPool.Spawn(assetObjectKey);
            if (assetObject != null)
            {
                return AssetsReference.Instantiate(assetObject.Target as GameObject, parent, this).gameObject;
            }

            AssetHandle handle = GetHandleSync<GameObject>(location, packageName: packageName);

            GameObject gameObject =
                AssetsReference.Instantiate(handle.AssetObject as GameObject, parent, this).gameObject;

            assetObject = AssetObject.Create(assetObjectKey, handle.AssetObject, handle, this);
            _assetPool.Register(assetObject, true);

            return gameObject;
        }

        private AssetHandle GetHandleSync<T>(string location, string packageName = "") where T : UnityEngine.Object
        {
            return GetHandleSync(location, typeof(T), packageName);
        }

        private AssetHandle GetHandleSync(string location, Type assetType, string packageName = "")
        {
            if (string.IsNullOrEmpty(packageName))
            {
                return YooAssets.LoadAssetSync(location, assetType);
            }

            var package = YooAssets.GetPackage(packageName);
            return package.LoadAssetSync(location, assetType);
        }
    }
}