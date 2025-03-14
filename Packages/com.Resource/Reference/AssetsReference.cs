//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace ZeroFramework.Resource
{
    [Serializable]
    public struct AssetsRefInfo
    {
        public int instanceId;

        public Object refAsset;

        public AssetsRefInfo(Object refAsset)
        {
            this.refAsset = refAsset;
            instanceId = this.refAsset.GetInstanceID();
        }
    }

    public sealed class AssetsReference : MonoBehaviour
    {
        [FormerlySerializedAs("_sourceGameObject")] [SerializeField] private GameObject sourceGameObject;

        [FormerlySerializedAs("_refAssetInfoList")] [SerializeField] private List<AssetsRefInfo> refAssetInfoList;

        private IResourceManager _resourceManager;

        private void OnDestroy()
        {
            if (_resourceManager == null)
            {
                _resourceManager = Zero.resource;
            }

            if (_resourceManager == null)
            {
                throw new GameFrameworkException($"ResourceManager is null.");
            }

            if (sourceGameObject != null)
            {
                _resourceManager.UnloadAsset(sourceGameObject);
            }

            if (refAssetInfoList != null)
            {
                foreach (var refInfo in refAssetInfoList)
                {
                    _resourceManager.UnloadAsset(refInfo.refAsset);
                }

                refAssetInfoList.Clear();
            }
        }

        public AssetsReference Ref(GameObject source, IResourceManager resourceManager = null)
        {
            if (source == null)
            {
                throw new GameFrameworkException($"Source gameObject is null.");
            }

            if (source.scene.name != null)
            {
                throw new GameFrameworkException($"Source gameObject is in scene.");
            }

            _resourceManager = resourceManager;
            sourceGameObject = source;
            return this;
        }

        public AssetsReference Ref<T>(T source, IResourceManager resourceManager = null) where T : UnityEngine.Object
        {
            if (source == null)
            {
                throw new GameFrameworkException($"Source gameObject is null.");
            }

            _resourceManager = resourceManager;
            refAssetInfoList = new List<AssetsRefInfo> { new AssetsRefInfo(source) };
            return this;
        }

        public static AssetsReference Instantiate(GameObject source, Transform parent = null, IResourceManager resourceManager = null)
        {
            if (source == null)
            {
                throw new GameFrameworkException($"Source gameObject is null.");
            }

            if (source.scene.name != null)
            {
                throw new GameFrameworkException($"Source gameObject is in scene.");
            }

            GameObject instance = Object.Instantiate(source, parent);
            return instance.AddComponent<AssetsReference>().Ref(source, resourceManager);
        }

        public static AssetsReference Ref(GameObject source, GameObject instance, IResourceManager resourceManager = null)
        {
            if (source == null)
            {
                throw new GameFrameworkException($"Source gameObject is null.");
            }

            if (source.scene.name != null)
            {
                throw new GameFrameworkException($"Source gameObject is in scene.");
            }

            return instance.GetOrAddComponent<AssetsReference>().Ref(source, resourceManager);
        }

        public static AssetsReference Ref<T>(T source, GameObject instance, IResourceManager resourceManager = null) where T : UnityEngine.Object
        {
            if (source == null)
            {
                throw new GameFrameworkException($"Source gameObject is null.");
            }

            return instance.GetOrAddComponent<AssetsReference>().Ref(source, resourceManager);
        }
    }
}