//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework.Download
{
    public sealed partial class DownloadManager : GameFrameworkModule, IDownloadManager
    {
        private sealed partial class DownloadCounter
        {
            private sealed class DownloadCounterNode : IReference
            {
                private long m_DeltaLength;
                private float m_ElapseSeconds;

                public DownloadCounterNode()
                {
                    m_DeltaLength = 0L;
                    m_ElapseSeconds = 0f;
                }

                public long DeltaLength => m_DeltaLength;

                public float ElapseSeconds => m_ElapseSeconds;

                public static DownloadCounterNode Create()
                {
                    return ReferencePool.Acquire<DownloadCounterNode>();
                }

                public void Update(float elapseSeconds, float realElapseSeconds)
                {
                    m_ElapseSeconds += realElapseSeconds;
                }

                public void AddDeltaLength(int deltaLength)
                {
                    m_DeltaLength += deltaLength;
                }

                public void Clear()
                {
                    m_DeltaLength = 0L;
                    m_ElapseSeconds = 0f;
                }
            }
        }
    }
}
