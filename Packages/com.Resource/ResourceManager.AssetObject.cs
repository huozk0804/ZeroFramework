//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using YooAsset;

namespace ZeroFramework.Resource
{
    public partial class ResourceManager
    {
        /// <summary>
        /// 资源对象。
        /// </summary>
        private sealed class AssetObject : ObjectBase
        {
            private AssetHandle _assetHandle = null;
            private ResourceManager _resourceManager;

            public static AssetObject Create(string name, object target, object assetHandle,
                ResourceManager resourceManager)
            {
                if (assetHandle == null)
                {
                    throw new GameFrameworkException("Resource is invalid.");
                }

                if (resourceManager == null)
                {
                    throw new GameFrameworkException("Resource Manager is invalid.");
                }

                AssetObject assetObject = ReferencePool.Acquire<AssetObject>();
                assetObject.Initialize(name, target);
                assetObject._assetHandle = (AssetHandle)assetHandle;
                assetObject._resourceManager = resourceManager;
                return assetObject;
            }

            public override void Clear()
            {
                base.Clear();
                _assetHandle = null;
            }

            protected internal override void Release(bool isShutdown)
            {
                if (!isShutdown)
                {
                    AssetHandle handle = _assetHandle;
                    if (handle is { IsValid: true })
                    {
                        handle.Dispose();
                    }

                    handle = null;
                }
            }
        }
    }
}