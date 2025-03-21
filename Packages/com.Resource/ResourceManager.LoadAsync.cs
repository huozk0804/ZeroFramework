using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;
using Object = UnityEngine.Object;

namespace ZeroFramework.Resource
{
    /// <summary>
    /// 异步加载接口
    /// </summary>
    public sealed partial class ResourceManager
    {
        public void LoadAssetAsync(string location, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks,
            object userData, string packageName = "")
        {
            throw new NotImplementedException();
        }

        public UniTask<T> LoadAssetAsync<T>(string location, LoadAssetCallbacks loadAssetCallbacks,
            string packageName = "") where T : Object
        {
            throw new NotImplementedException();
        }

        public UniTask<T[]> LoadSubAssetsAsync<T>(string location, string packageName = "") where T : Object
        {
            throw new NotImplementedException();
        }

        public UniTask<GameObject> LoadGameObjectAsync(string location, Transform parent = null,
            LoadAssetCallbacks loadAssetCallbacks = null,
            string packageName = "")
        {
            throw new NotImplementedException();
        }

        public UniTask<SceneManager> LoadSceneAsync(string location, LoadSceneMode mode, string packageName = "")
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取异步资源句柄。
        /// </summary>
        /// <param name="location">资源定位地址</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns>资源句柄。</returns>
        private AssetHandle GetHandleAsync<T>(string location, string packageName = "") where T : UnityEngine.Object
        {
            return GetHandleAsync(location, typeof(T), packageName);
        }

        /// <summary>
        /// 获取异步资源句柄。
        /// </summary>
        /// <param name="location">资源定位地址</param>
        /// <param name="assetType">资源类型</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        /// <returns></returns>
        private AssetHandle GetHandleAsync(string location, Type assetType, string packageName = "")
        {
            if (string.IsNullOrEmpty(packageName))
            {
                return YooAssets.LoadAssetAsync(location, assetType);
            }

            var package = YooAssets.GetPackage(packageName);
            return package.LoadAssetAsync(location, assetType);
        }
    }
}