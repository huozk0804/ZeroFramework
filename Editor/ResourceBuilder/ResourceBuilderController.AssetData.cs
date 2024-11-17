//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

namespace ZeroFramework.Editor.ResourceTools
{
    public sealed partial class ResourceBuilderController
    {
        private sealed class AssetData
        {
            private readonly string m_Guid;
            private readonly string m_Name;
            private readonly int m_Length;
            private readonly int m_HashCode;
            private readonly string[] m_DependencyAssetNames;

            public AssetData(string guid, string name, int length, int hashCode, string[] dependencyAssetNames)
            {
                m_Guid = guid;
                m_Name = name;
                m_Length = length;
                m_HashCode = hashCode;
                m_DependencyAssetNames = dependencyAssetNames;
            }

            public string Guid => m_Guid;

            public string Name => m_Name;

            public int Length => m_Length;

            public int HashCode => m_HashCode;

            public string[] GetDependencyAssetNames()
            {
                return m_DependencyAssetNames;
            }
        }
    }
}
