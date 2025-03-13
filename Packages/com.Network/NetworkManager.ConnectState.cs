//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System.Net.Sockets;

namespace ZeroFramework.Network
{
    public sealed partial class NetworkManager : GameFrameworkModule, INetworkManager
    {
        private sealed class ConnectState
        {
            private readonly Socket _socket;
            private readonly object _userData;

            public ConnectState(Socket socket, object userData)
            {
                _socket = socket;
                _userData = userData;
            }

            public Socket Socket => _socket;

            public object UserData => _userData;
        }
    }
}
