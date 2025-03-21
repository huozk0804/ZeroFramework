//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace ZeroFramework.Resource
{
    /// <summary>
    /// 资源管理器接口。
    /// </summary>
    public interface IResourceManager
    {
        /// <summary>
        /// 默认资源包
        /// </summary>
        ResourcePackage DefaultPackage { get; set; }
        
        /// <summary>
        /// 初始化接口
        /// </summary>
        void Initialize(string defaultPackageName);

        /// <summary>
        /// 初始化操作
        /// </summary>
        /// <param name="playMode">资源包运行模式</param>
        /// <param name="packageName">资源包名称</param>
        UniTask<InitializationOperation> InitPackage(EPlayMode playMode, string packageName);

        /// <summary>
        /// 卸载资源。
        /// </summary>
        /// <param name="asset">要卸载的资源。</param>
        void UnloadAsset(object asset);

        /// <summary>
        /// 资源回收（卸载引用计数为零的资源）
        /// </summary>
        void UnloadUnusedAssets();

        /// <summary>
        /// 强制回收所有资源
        /// </summary>
        void ForceUnloadAllAssets();

        /// <summary>
        /// 检查资源是否存在。
        /// </summary>
        /// <param name="location">资源定位地址。</param>
        /// <param name="packageName">资源包名称。</param>
        /// <returns>检查资源是否存在的结果。</returns>
        HasAssetResult HasAsset(string location, string packageName = "");

        /// <summary>
        /// 检查资源定位地址是否有效。
        /// </summary>
        /// <param name="location">资源的定位地址</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        bool CheckLocationValid(string location, string packageName = "");

        /// <summary>
        /// 获取资源信息列表。
        /// </summary>
        /// <param name="resTag">资源标签。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        /// <returns>资源信息列表。</returns>
        AssetInfo[] GetAssetInfos(string resTag, string packageName = "");

        /// <summary>
        /// 获取资源信息列表。
        /// </summary>
        /// <param name="tags">资源标签列表。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        /// <returns>资源信息列表。</returns>
        AssetInfo[] GetAssetInfos(string[] tags, string packageName = "");

        /// <summary>
        /// 获取资源信息。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        /// <returns>资源信息。</returns>
        AssetInfo GetAssetInfo(string location, string packageName = "");

        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <param name="location">资源的定位地址</param>
        /// <param name="packageName">指定资源包的名称,不传使用默认资源包</param>
        /// <typeparam name="T">要加载资源的类型</typeparam>
        /// <returns>资源实例。</returns>
        T LoadAsset<T>(string location, string packageName = "") where T : UnityEngine.Object;

        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <param name="location">资源的定位地址</param>
        /// <param name="type">要加载资源的类型</param>
        /// <param name="packageName">指定资源包的名称,不传使用默认资源包</param>
        /// <returns>资源实例</returns>
        UnityEngine.Object LoadAsset(string location, Type type, string packageName = "");

        /// <summary>
        /// 同步加载原生资源
        /// </summary>
        /// <param name="location">资源的定位地址</param>
        /// <param name="packageName">指定资源包的名称,不传使用默认资源包</param>
        /// <returns>资源实例</returns>
        byte[] LoadRawAsset(string location, string packageName = "");

        /// <summary>
        /// 同步加载游戏物体并实例化。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <param name="parent">资源实例父节点。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        /// <returns>资源实例。</returns>
        /// <remarks>会实例化资源到场景，无需主动UnloadAsset，Destroy时自动UnloadAsset。</remarks>
        GameObject LoadGameObject(string location, Transform parent = null, string packageName = "");

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <param name="assetType">要加载的资源类型。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包。</param>
        void LoadAssetAsync(string location, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks,
            object userData, string packageName = "");

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="location">资源定位地址。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        /// <typeparam name="T">要加载资源的类型。</typeparam>
        /// <returns>异步资源实例。</returns>
        UniTask<T> LoadAssetAsync<T>(string location, LoadAssetCallbacks loadAssetCallbacks, string packageName = "")
            where T : UnityEngine.Object;

        /// <summary>
        /// 异步加载游戏物体并实例化。
        /// </summary>
        /// <param name="location">资源定位地址。</param>
        /// <param name="parent">资源实例父节点。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        /// <returns>异步游戏物体实例。</returns>
        /// <remarks>会实例化资源到场景，无需主动UnloadAsset，Destroy时自动UnloadAsset。</remarks>
        UniTask<GameObject> LoadGameObjectAsync(string location, Transform parent = null,
            LoadAssetCallbacks loadAssetCallbacks = null, string packageName = "");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="mode"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        UniTask<SceneManager> LoadSceneAsync(string location, LoadSceneMode mode, string packageName = "");
    }
}