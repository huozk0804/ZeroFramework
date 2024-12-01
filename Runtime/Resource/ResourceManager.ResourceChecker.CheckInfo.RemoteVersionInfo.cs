//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

using System.Runtime.InteropServices;

namespace ZeroFramework.Resource
{
    public sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
    {
        private sealed partial class ResourceChecker
        {
            private sealed partial class CheckInfo
            {
                /// <summary>
                /// 远程资源状态信息。
                /// </summary>
                [StructLayout(LayoutKind.Auto)]
                private struct RemoteVersionInfo
                {
                    private readonly bool m_Exist;
                    private readonly string m_FileSystemName;
                    private readonly LoadType m_LoadType;
                    private readonly int m_Length;
                    private readonly int m_HashCode;
                    private readonly int m_CompressedLength;
                    private readonly int m_CompressedHashCode;

                    public RemoteVersionInfo(string fileSystemName, LoadType loadType, int length, int hashCode,
                        int compressedLength, int compressedHashCode)
                    {
                        m_Exist = true;
                        m_FileSystemName = fileSystemName;
                        m_LoadType = loadType;
                        m_Length = length;
                        m_HashCode = hashCode;
                        m_CompressedLength = compressedLength;
                        m_CompressedHashCode = compressedHashCode;
                    }

                    public bool Exist => m_Exist;

                    public bool UseFileSystem => !string.IsNullOrEmpty(m_FileSystemName);

                    public string FileSystemName => m_FileSystemName;

                    public LoadType LoadType => m_LoadType;

                    public int Length => m_Length;

                    public int HashCode => m_HashCode;

                    public int CompressedLength => m_CompressedLength;

                    public int CompressedHashCode => m_CompressedHashCode;
                }
            }
        }
    }
}