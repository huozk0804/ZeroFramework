//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

namespace ZeroFramework.Resource
{
    public sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
    {
        private sealed partial class ResourceLoader
        {
            private sealed class LoadBinaryInfo : IReference
            {
                private string m_BinaryAssetName;
                private ResourceInfo m_ResourceInfo;
                private LoadBinaryCallbacks m_LoadBinaryCallbacks;
                private object m_UserData;

                public LoadBinaryInfo()
                {
                    m_BinaryAssetName = null;
                    m_ResourceInfo = null;
                    m_LoadBinaryCallbacks = null;
                    m_UserData = null;
                }

                public string BinaryAssetName => m_BinaryAssetName;

                public ResourceInfo ResourceInfo => m_ResourceInfo;

                public LoadBinaryCallbacks LoadBinaryCallbacks => m_LoadBinaryCallbacks;

                public object UserData => m_UserData;

                public static LoadBinaryInfo Create(string binaryAssetName, ResourceInfo resourceInfo,
                    LoadBinaryCallbacks loadBinaryCallbacks, object userData)
                {
                    LoadBinaryInfo loadBinaryInfo = ReferencePool.Acquire<LoadBinaryInfo>();
                    loadBinaryInfo.m_BinaryAssetName = binaryAssetName;
                    loadBinaryInfo.m_ResourceInfo = resourceInfo;
                    loadBinaryInfo.m_LoadBinaryCallbacks = loadBinaryCallbacks;
                    loadBinaryInfo.m_UserData = userData;
                    return loadBinaryInfo;
                }

                public void Clear()
                {
                    m_BinaryAssetName = null;
                    m_ResourceInfo = null;
                    m_LoadBinaryCallbacks = null;
                    m_UserData = null;
                }
            }
        }
    }
}