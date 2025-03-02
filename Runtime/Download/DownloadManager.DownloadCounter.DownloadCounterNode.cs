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
                private long _deltaLength;
                private float _elapseSeconds;

                public DownloadCounterNode()
                {
                    _deltaLength = 0L;
                    _elapseSeconds = 0f;
                }

                public long DeltaLength => _deltaLength;

                public float ElapseSeconds => _elapseSeconds;

                public static DownloadCounterNode Create()
                {
                    return ReferencePool.Acquire<DownloadCounterNode>();
                }

                public void Update(float elapseSeconds, float realElapseSeconds)
                {
                    _elapseSeconds += realElapseSeconds;
                }

                public void AddDeltaLength(int deltaLength)
                {
                    _deltaLength += deltaLength;
                }

                public void Clear()
                {
                    _deltaLength = 0L;
                    _elapseSeconds = 0f;
                }
            }
        }
    }
}
