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
        private sealed class ResourceCode
        {
            private readonly Platform m_Platform;
            private readonly int m_Length;
            private readonly int m_HashCode;
            private readonly int m_CompressedLength;
            private readonly int m_CompressedHashCode;

            public ResourceCode(Platform platform, int length, int hashCode, int compressedLength, int compressedHashCode)
            {
                m_Platform = platform;
                m_Length = length;
                m_HashCode = hashCode;
                m_CompressedLength = compressedLength;
                m_CompressedHashCode = compressedHashCode;
            }

            public Platform Platform => m_Platform;

            public int Length => m_Length;

            public int HashCode => m_HashCode;

            public int CompressedLength => m_CompressedLength;

            public int CompressedHashCode => m_CompressedHashCode;
        }
    }
}
