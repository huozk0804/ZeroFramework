//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ZeroFramework.Resource
{
    public sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
    {
        private sealed partial class ResourceLoader
        {
            private abstract class LoadResourceTaskBase : TaskBase
            {
                private static int s_Serial = 0;

                private string m_AssetName;
                private Type m_AssetType;
                private ResourceInfo m_ResourceInfo;
                private string[] m_DependencyAssetNames;
                private readonly List<object> m_DependencyAssets;
                private ResourceObject m_ResourceObject;
                private DateTime m_StartTime;
                private int m_TotalDependencyAssetCount;

                public LoadResourceTaskBase()
                {
                    m_AssetName = null;
                    m_AssetType = null;
                    m_ResourceInfo = null;
                    m_DependencyAssetNames = null;
                    m_DependencyAssets = new List<object>();
                    m_ResourceObject = null;
                    m_StartTime = default(DateTime);
                    m_TotalDependencyAssetCount = 0;
                }

                public string AssetName => m_AssetName;

                public Type AssetType => m_AssetType;

                public ResourceInfo ResourceInfo => m_ResourceInfo;

                public ResourceObject ResourceObject => m_ResourceObject;

                public abstract bool IsScene { get; }

                public DateTime StartTime
                {
                    get => m_StartTime;
                    set => m_StartTime = value;
                }

                public int LoadedDependencyAssetCount => m_DependencyAssets.Count;

                public int TotalDependencyAssetCount
                {
                    get => m_TotalDependencyAssetCount;
                    set => m_TotalDependencyAssetCount = value;
                }

                public override string Description => m_AssetName;

                public override void Clear()
                {
                    base.Clear();
                    m_AssetName = null;
                    m_AssetType = null;
                    m_ResourceInfo = null;
                    m_DependencyAssetNames = null;
                    m_DependencyAssets.Clear();
                    m_ResourceObject = null;
                    m_StartTime = default(DateTime);
                    m_TotalDependencyAssetCount = 0;
                }

                public string[] GetDependencyAssetNames()
                {
                    return m_DependencyAssetNames;
                }

                public List<object> GetDependencyAssets()
                {
                    return m_DependencyAssets;
                }

                public void LoadMain(LoadResourceAgent agent, ResourceObject resourceObject)
                {
                    m_ResourceObject = resourceObject;
                    agent.Helper.LoadAsset(resourceObject.Target, AssetName, AssetType, IsScene);
                }

                public virtual void OnLoadAssetSuccess(LoadResourceAgent agent, object asset, float duration)
                {
                }

                public virtual void OnLoadAssetFailure(LoadResourceAgent agent, LoadResourceStatus status,
                    string errorMessage)
                {
                }

                public virtual void OnLoadAssetUpdate(LoadResourceAgent agent, LoadResourceProgress type,
                    float progress)
                {
                }

                public virtual void OnLoadDependencyAsset(LoadResourceAgent agent, string dependencyAssetName,
                    object dependencyAsset)
                {
                    m_DependencyAssets.Add(dependencyAsset);
                }

                protected void Initialize(string assetName, Type assetType, int priority, ResourceInfo resourceInfo,
                    string[] dependencyAssetNames, object userData)
                {
                    Initialize(++s_Serial, null, priority, userData);
                    m_AssetName = assetName;
                    m_AssetType = assetType;
                    m_ResourceInfo = resourceInfo;
                    m_DependencyAssetNames = dependencyAssetNames;
                }
            }
        }
    }
}