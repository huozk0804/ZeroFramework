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
            private MemoryStream _stream;
            private bool _disposed;

            public SendState()
            {
                _stream = new MemoryStream(DefaultBufferLength);
                _disposed = false;
            }

            public MemoryStream Stream => _stream;

            public void Reset()
            {
                _stream.Position = 0L;
                _stream.SetLength(0L);
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                if (_disposed)
                {
                    return;
                }

                if (disposing)
                {
                    if (_stream != null)
                    {
                        _stream.Dispose();
                        _stream = null;
                    }
                }

                _disposed = true;
            }
        }
    }
}
