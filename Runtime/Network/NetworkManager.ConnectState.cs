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
            private readonly Socket m_Socket;
            private readonly object m_UserData;

            public ConnectState(Socket socket, object userData)
            {
                m_Socket = socket;
                m_UserData = userData;
            }

            public Socket Socket => m_Socket;

            public object UserData => m_UserData;
        }
    }
}
