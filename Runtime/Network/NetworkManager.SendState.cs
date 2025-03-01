//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;
using System.IO;

namespace ZeroFramework.Network
{
    public sealed partial class NetworkManager : GameFrameworkModule, INetworkManager
    {
        private sealed class SendState : IDisposable
        {
            private const int DefaultBufferLength = 1024 * 64;
            private MemoryStream m_Stream;
            private bool m_Disposed;

            public SendState()
            {
                m_Stream = new MemoryStream(DefaultBufferLength);
                m_Disposed = false;
            }

            public MemoryStream Stream => m_Stream;

            public void Reset()
            {
                m_Stream.Position = 0L;
                m_Stream.SetLength(0L);
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                if (m_Disposed)
                {
                    return;
                }

                if (disposing)
                {
                    if (m_Stream != null)
                    {
                        m_Stream.Dispose();
                        m_Stream = null;
                    }
                }

                m_Disposed = true;
            }
        }
    }
}
