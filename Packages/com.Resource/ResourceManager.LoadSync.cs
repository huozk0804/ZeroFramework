using System;
using UnityEngine;
using YooAsset;

namespace ZeroFramework.Resource
{
    public partial class ResourceManager
    {
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


        public TObject[] LoadSubAssetsSync<TObject>(string location, string packageName = "")
            where TObject : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            throw new NotImplementedException();
        }
    }
}