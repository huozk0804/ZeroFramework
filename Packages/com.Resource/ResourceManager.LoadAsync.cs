using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace ZeroFramework.Resource
{
    /// <summary>
    /// 异步加载接口
    /// </summary>
    public partial class ResourceManager
    {
        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <param name="callback">回调函数。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        /// <typeparam name="T">要加载资源的类型。</typeparam>
        public async UniTaskVoid LoadAsset<T>(string location, Action<T> callback, string packageName = "")
            where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(location))
            {
                Log.Error("Asset name is invalid.");
                return;
            }

            if (string.IsNullOrEmpty(location))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            string assetObjectKey = GetCacheKey(location, packageName);

            await TryWaitingLoading(assetObjectKey);

            AssetObject assetObject = _assetPool.Spawn(assetObjectKey);
            if (assetObject != null)
            {
                await UniTask.Yield();
                callback?.Invoke(assetObject.Target as T);
                return;
            }

            _assetLoadingList.Add(assetObjectKey);

            AssetHandle handle = GetHandleAsync<T>(location, packageName: packageName);

            handle.Completed += assetHandle =>
            {
                _assetLoadingList.Remove(assetObjectKey);

                if (assetHandle.AssetObject != null)
                {
                    assetObject = AssetObject.Create(assetObjectKey, handle.AssetObject, handle, this);
                    _assetPool.Register(assetObject, true);

                    callback?.Invoke(assetObject.Target as T);
                }
                else
                {
                    callback?.Invoke(null);
                }
            };
        }

        public UniTask<TObject[]> LoadSubAssetsAsync<TObject>(string location, string packageName = "")
            where TObject : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            throw new NotImplementedException();
        }

        public async UniTask<T> LoadAssetAsync<T>(string location, CancellationToken cancellationToken = default,
            string packageName = "") where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            string assetObjectKey = GetCacheKey(location, packageName);

            await TryWaitingLoading(assetObjectKey);

            AssetObject assetObject = _assetPool.Spawn(assetObjectKey);
            if (assetObject != null)
            {
                await UniTask.Yield();
                return assetObject.Target as T;
            }

            _assetLoadingList.Add(assetObjectKey);

            AssetHandle handle = GetHandleAsync<T>(location, packageName: packageName);

            bool cancelOrFailed = await handle.ToUniTask().AttachExternalCancellation(cancellationToken)
                .SuppressCancellationThrow();

            if (cancelOrFailed)
            {
                _assetLoadingList.Remove(assetObjectKey);
                return null;
            }

            assetObject = AssetObject.Create(assetObjectKey, handle.AssetObject, handle, this);
            _assetPool.Register(assetObject, true);

            _assetLoadingList.Remove(assetObjectKey);

            return handle.AssetObject as T;
        }

        public async UniTask<GameObject> LoadGameObjectAsync(string location, Transform parent = null,
            CancellationToken cancellationToken = default, string packageName = "")
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            string assetObjectKey = GetCacheKey(location, packageName);

            await TryWaitingLoading(assetObjectKey);

            AssetObject assetObject = _assetPool.Spawn(assetObjectKey);
            if (assetObject != null)
            {
                await UniTask.Yield();
                return AssetsReference.Instantiate(assetObject.Target as GameObject, parent, this).gameObject;
            }

            _assetLoadingList.Add(assetObjectKey);

            AssetHandle handle = GetHandleAsync<GameObject>(location, packageName: packageName);

            bool cancelOrFailed = await handle.ToUniTask().AttachExternalCancellation(cancellationToken)
                .SuppressCancellationThrow();

            if (cancelOrFailed)
            {
                _assetLoadingList.Remove(assetObjectKey);
                return null;
            }

            GameObject gameObject =
                AssetsReference.Instantiate(handle.AssetObject as GameObject, parent, this).gameObject;

            assetObject = AssetObject.Create(assetObjectKey, handle.AssetObject, handle, this);
            _assetPool.Register(assetObject, true);

            _assetLoadingList.Remove(assetObjectKey);

            return gameObject;
        }
        
        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包。</param>
        public async void LoadAssetAsync(string location, Type assetType, int priority,
            LoadAssetCallbacks loadAssetCallbacks, object userData, string packageName = "")
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            if (loadAssetCallbacks == null)
            {
                throw new GameFrameworkException("Load asset callbacks is invalid.");
            }

            string assetObjectKey = GetCacheKey(location, packageName);

            await TryWaitingLoading(assetObjectKey);

            float duration = Time.time;

            AssetObject assetObject = _assetPool.Spawn(assetObjectKey);
            if (assetObject != null)
            {
                await UniTask.Yield();
                loadAssetCallbacks.LoadAssetSuccessCallback(location, assetObject.Target, Time.time - duration,
                    userData);
                return;
            }

            _assetLoadingList.Add(assetObjectKey);

            AssetInfo assetInfo = GetAssetInfo(location, packageName);

            if (!string.IsNullOrEmpty(assetInfo.Error))
            {
                _assetLoadingList.Remove(assetObjectKey);

                string errorMessage = $"Can not load asset '{location}' because :'{assetInfo.Error}'.";
                if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                {
                    loadAssetCallbacks.LoadAssetFailureCallback(location, LoadResourceStatus.NotExist, errorMessage,
                        userData);
                    return;
                }

                throw new GameFrameworkException(errorMessage);
            }

            AssetHandle handle = GetHandleAsync(location, assetType, packageName: packageName);

            if (loadAssetCallbacks.LoadAssetUpdateCallback != null)
            {
                InvokeProgress(location, handle, loadAssetCallbacks.LoadAssetUpdateCallback, userData).Forget();
            }

            await handle.ToUniTask();

            if (handle.AssetObject == null || handle.Status == EOperationStatus.Failed)
            {
                _assetLoadingList.Remove(assetObjectKey);

                string errorMessage = Utility.Text.Format("Can not load asset '{0}'.", location);
                if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                {
                    loadAssetCallbacks.LoadAssetFailureCallback(location, LoadResourceStatus.NotReady, errorMessage,
                        userData);
                    return;
                }

                throw new GameFrameworkException(errorMessage);
            }
            else
            {
                assetObject = AssetObject.Create(assetObjectKey, handle.AssetObject, handle, this);
                _assetPool.Register(assetObject, true);

                _assetLoadingList.Remove(assetObjectKey);

                if (loadAssetCallbacks.LoadAssetSuccessCallback != null)
                {
                    duration = Time.time - duration;

                    loadAssetCallbacks.LoadAssetSuccessCallback(location, handle.AssetObject, duration, userData);
                }
            }
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包。</param>
        public async void LoadAssetAsync(string location, int priority, LoadAssetCallbacks loadAssetCallbacks,
            object userData, string packageName = "")
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            if (loadAssetCallbacks == null)
            {
                throw new GameFrameworkException("Load asset callbacks is invalid.");
            }

            string assetObjectKey = GetCacheKey(location, packageName);

            await TryWaitingLoading(assetObjectKey);

            float duration = Time.time;

            AssetObject assetObject = _assetPool.Spawn(assetObjectKey);
            if (assetObject != null)
            {
                await UniTask.Yield();
                loadAssetCallbacks.LoadAssetSuccessCallback(location, assetObject.Target, Time.time - duration,
                    userData);
                return;
            }

            _assetLoadingList.Add(assetObjectKey);

            AssetInfo assetInfo = GetAssetInfo(location, packageName);

            if (!string.IsNullOrEmpty(assetInfo.Error))
            {
                _assetLoadingList.Remove(assetObjectKey);

                string errorMessage = Utility.Text.Format("Can not load asset '{0}' because :'{1}'.", location,
                    assetInfo.Error);
                if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                {
                    loadAssetCallbacks.LoadAssetFailureCallback(location, LoadResourceStatus.NotExist, errorMessage,
                        userData);
                    return;
                }

                throw new GameFrameworkException(errorMessage);
            }

            AssetHandle handle = GetHandleAsync(location, assetInfo.AssetType, packageName: packageName);

            if (loadAssetCallbacks.LoadAssetUpdateCallback != null)
            {
                InvokeProgress(location, handle, loadAssetCallbacks.LoadAssetUpdateCallback, userData).Forget();
            }

            await handle.ToUniTask();

            if (handle.AssetObject == null || handle.Status == EOperationStatus.Failed)
            {
                _assetLoadingList.Remove(assetObjectKey);

                string errorMessage = Utility.Text.Format("Can not load asset '{0}'.", location);
                if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                {
                    loadAssetCallbacks.LoadAssetFailureCallback(location, LoadResourceStatus.NotReady, errorMessage,
                        userData);
                    return;
                }

                throw new GameFrameworkException(errorMessage);
            }
            else
            {
                assetObject = AssetObject.Create(assetObjectKey, handle.AssetObject, handle, this);
                _assetPool.Register(assetObject, true);

                _assetLoadingList.Remove(assetObjectKey);

                if (loadAssetCallbacks.LoadAssetSuccessCallback != null)
                {
                    duration = Time.time - duration;

                    loadAssetCallbacks.LoadAssetSuccessCallback(location, handle.AssetObject, duration, userData);
                }
            }
        }
    }
}