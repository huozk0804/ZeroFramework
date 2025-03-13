//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework.Network
{
    public sealed partial class NetworkManager : GameFrameworkModule, INetworkManager
    {
        private sealed class HeartBeatState
        {
            private float _heartBeatElapseSeconds;
            private int _missHeartBeatCount;

            public HeartBeatState()
            {
                _heartBeatElapseSeconds = 0f;
                _missHeartBeatCount = 0;
            }

            public float HeartBeatElapseSeconds
            {
                get => _heartBeatElapseSeconds;
                set => _heartBeatElapseSeconds = value;
            }

            public int MissHeartBeatCount
            {
                get => _missHeartBeatCount;
                set => _missHeartBeatCount = value;
            }

            public void Reset(bool resetHeartBeatElapseSeconds)
            {
                if (resetHeartBeatElapseSeconds)
                {
                    _heartBeatElapseSeconds = 0f;
                }

                _missHeartBeatCount = 0;
            }
        }
    }
}
