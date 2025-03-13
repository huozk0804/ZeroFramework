﻿//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace ZeroFramework.Network
{
    /// <summary>
    /// 网络管理器。
    /// </summary>
    public sealed partial class NetworkManager : GameFrameworkModule, INetworkManager
    {
        private readonly Dictionary<string, NetworkChannelBase> _networkChannels;

        private EventHandler<NetworkConnectedEventArgs> _networkConnectedEventHandler;
        private EventHandler<NetworkClosedEventArgs> _networkClosedEventHandler;
        private EventHandler<NetworkMissHeartBeatEventArgs> _networkMissHeartBeatEventHandler;
        private EventHandler<NetworkErrorEventArgs> _networkErrorEventHandler;
        private EventHandler<NetworkCustomErrorEventArgs> _networkCustomErrorEventHandler;

        /// <summary>
        /// 初始化网络管理器的新实例。
        /// </summary>
        public NetworkManager()
        {
            _networkChannels = new Dictionary<string, NetworkChannelBase>(StringComparer.Ordinal);
            _networkConnectedEventHandler = null;
            _networkClosedEventHandler = null;
            _networkMissHeartBeatEventHandler = null;
            _networkErrorEventHandler = null;
            _networkCustomErrorEventHandler = null;
        }

        /// <summary>
        /// 获取网络频道数量。
        /// </summary>
        public int NetworkChannelCount => _networkChannels.Count;

        /// <summary>
        /// 网络连接成功事件。
        /// </summary>
        public event EventHandler<NetworkConnectedEventArgs> NetworkConnected
        {
            add => _networkConnectedEventHandler += value;
            remove => _networkConnectedEventHandler -= value;
        }

        /// <summary>
        /// 网络连接关闭事件。
        /// </summary>
        public event EventHandler<NetworkClosedEventArgs> NetworkClosed
        {
            add => _networkClosedEventHandler += value;
            remove => _networkClosedEventHandler -= value;
        }

        /// <summary>
        /// 网络心跳包丢失事件。
        /// </summary>
        public event EventHandler<NetworkMissHeartBeatEventArgs> NetworkMissHeartBeat
        {
            add => _networkMissHeartBeatEventHandler += value;
            remove => _networkMissHeartBeatEventHandler -= value;
        }

        /// <summary>
        /// 网络错误事件。
        /// </summary>
        public event EventHandler<NetworkErrorEventArgs> NetworkError
        {
            add => _networkErrorEventHandler += value;
            remove => _networkErrorEventHandler -= value;
        }

        /// <summary>
        /// 用户自定义网络错误事件。
        /// </summary>
        public event EventHandler<NetworkCustomErrorEventArgs> NetworkCustomError
        {
            add => _networkCustomErrorEventHandler += value;
            remove => _networkCustomErrorEventHandler -= value;
        }

		/// <summary>
		/// 网络管理器轮询。
		/// </summary>
		/// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
		/// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
		protected internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (KeyValuePair<string, NetworkChannelBase> networkChannel in _networkChannels)
            {
                networkChannel.Value.Update(elapseSeconds, realElapseSeconds);
            }
        }

		/// <summary>
		/// 关闭并清理网络管理器。
		/// </summary>
		protected internal override void Shutdown()
        {
            foreach (KeyValuePair<string, NetworkChannelBase> networkChannel in _networkChannels)
            {
                NetworkChannelBase networkChannelBase = networkChannel.Value;
                networkChannelBase.NetworkChannelConnected -= OnNetworkChannelConnected;
                networkChannelBase.NetworkChannelClosed -= OnNetworkChannelClosed;
                networkChannelBase.NetworkChannelMissHeartBeat -= OnNetworkChannelMissHeartBeat;
                networkChannelBase.NetworkChannelError -= OnNetworkChannelError;
                networkChannelBase.NetworkChannelCustomError -= OnNetworkChannelCustomError;
                networkChannelBase.Shutdown();
            }

            _networkChannels.Clear();
        }

        /// <summary>
        /// 检查是否存在网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>是否存在网络频道。</returns>
        public bool HasNetworkChannel(string name)
        {
            return _networkChannels.ContainsKey(name ?? string.Empty);
        }

        /// <summary>
        /// 获取网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>要获取的网络频道。</returns>
        public INetworkChannel GetNetworkChannel(string name)
        {
            if (_networkChannels.TryGetValue(name ?? string.Empty, out var networkChannel))
            {
                return networkChannel;
            }

            return null;
        }

        /// <summary>
        /// 获取所有网络频道。
        /// </summary>
        /// <returns>所有网络频道。</returns>
        public INetworkChannel[] GetAllNetworkChannels()
        {
            int index = 0;
            INetworkChannel[] results = new INetworkChannel[_networkChannels.Count];
            foreach (KeyValuePair<string, NetworkChannelBase> networkChannel in _networkChannels)
            {
                results[index++] = networkChannel.Value;
            }

            return results;
        }

        /// <summary>
        /// 获取所有网络频道。
        /// </summary>
        /// <param name="results">所有网络频道。</param>
        public void GetAllNetworkChannels(List<INetworkChannel> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<string, NetworkChannelBase> networkChannel in _networkChannels)
            {
                results.Add(networkChannel.Value);
            }
        }

        /// <summary>
        /// 创建网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <param name="serviceType">网络服务类型。</param>
        /// <param name="networkChannelHelper">网络频道辅助器。</param>
        /// <returns>要创建的网络频道。</returns>
        public INetworkChannel CreateNetworkChannel(string name, ServiceType serviceType, INetworkChannelHelper networkChannelHelper)
        {
            if (networkChannelHelper == null)
            {
                throw new GameFrameworkException("Network channel helper is invalid.");
            }

            if (networkChannelHelper.PacketHeaderLength < 0)
            {
                throw new GameFrameworkException("Packet header length is invalid.");
            }

            if (HasNetworkChannel(name))
            {
                throw new GameFrameworkException(Utility.Text.Format("Already exist network channel '{0}'.", name ?? string.Empty));
            }

            NetworkChannelBase networkChannel = null;
            switch (serviceType)
            {
                case ServiceType.Tcp:
                    networkChannel = new TcpNetworkChannel(name, networkChannelHelper);
                    break;

                case ServiceType.TcpWithSyncReceive:
                    networkChannel = new TcpWithSyncReceiveNetworkChannel(name, networkChannelHelper);
                    break;

                default:
                    throw new GameFrameworkException(Utility.Text.Format("Not supported service type '{0}'.", serviceType));
            }

            networkChannel.NetworkChannelConnected += OnNetworkChannelConnected;
            networkChannel.NetworkChannelClosed += OnNetworkChannelClosed;
            networkChannel.NetworkChannelMissHeartBeat += OnNetworkChannelMissHeartBeat;
            networkChannel.NetworkChannelError += OnNetworkChannelError;
            networkChannel.NetworkChannelCustomError += OnNetworkChannelCustomError;
            _networkChannels.Add(name, networkChannel);
            return networkChannel;
        }

        /// <summary>
        /// 销毁网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>是否销毁网络频道成功。</returns>
        public bool DestroyNetworkChannel(string name)
        {
            if (!_networkChannels.TryGetValue(name ?? string.Empty, out var networkChannel))
            {
				return false;
			}

			networkChannel.NetworkChannelConnected -= OnNetworkChannelConnected;
			networkChannel.NetworkChannelClosed -= OnNetworkChannelClosed;
			networkChannel.NetworkChannelMissHeartBeat -= OnNetworkChannelMissHeartBeat;
			networkChannel.NetworkChannelError -= OnNetworkChannelError;
			networkChannel.NetworkChannelCustomError -= OnNetworkChannelCustomError;
			networkChannel.Shutdown();
			return _networkChannels.Remove(name);
        }

        private void OnNetworkChannelConnected(NetworkChannelBase networkChannel, object userData)
        {
            if (_networkConnectedEventHandler != null)
            {
                lock (_networkConnectedEventHandler)
                {
                    NetworkConnectedEventArgs networkConnectedEventArgs = NetworkConnectedEventArgs.Create(networkChannel, userData);
                    _networkConnectedEventHandler(this, networkConnectedEventArgs);
                    ReferencePool.Release(networkConnectedEventArgs);
                }
            }
        }

        private void OnNetworkChannelClosed(NetworkChannelBase networkChannel)
        {
            if (_networkClosedEventHandler != null)
            {
                lock (_networkClosedEventHandler)
                {
                    NetworkClosedEventArgs networkClosedEventArgs = NetworkClosedEventArgs.Create(networkChannel);
                    _networkClosedEventHandler(this, networkClosedEventArgs);
                    ReferencePool.Release(networkClosedEventArgs);
                }
            }
        }

        private void OnNetworkChannelMissHeartBeat(NetworkChannelBase networkChannel, int missHeartBeatCount)
        {
            if (_networkMissHeartBeatEventHandler != null)
            {
                lock (_networkMissHeartBeatEventHandler)
                {
                    NetworkMissHeartBeatEventArgs networkMissHeartBeatEventArgs = NetworkMissHeartBeatEventArgs.Create(networkChannel, missHeartBeatCount);
                    _networkMissHeartBeatEventHandler(this, networkMissHeartBeatEventArgs);
                    ReferencePool.Release(networkMissHeartBeatEventArgs);
                }
            }
        }

        private void OnNetworkChannelError(NetworkChannelBase networkChannel, NetworkErrorCode errorCode, SocketError socketErrorCode, string errorMessage)
        {
            if (_networkErrorEventHandler != null)
            {
                lock (_networkErrorEventHandler)
                {
                    NetworkErrorEventArgs networkErrorEventArgs = NetworkErrorEventArgs.Create(networkChannel, errorCode, socketErrorCode, errorMessage);
                    _networkErrorEventHandler(this, networkErrorEventArgs);
                    ReferencePool.Release(networkErrorEventArgs);
                }
            }
        }

        private void OnNetworkChannelCustomError(NetworkChannelBase networkChannel, object customErrorData)
        {
            if (_networkCustomErrorEventHandler != null)
            {
                lock (_networkCustomErrorEventHandler)
                {
                    NetworkCustomErrorEventArgs networkCustomErrorEventArgs = NetworkCustomErrorEventArgs.Create(networkChannel, customErrorData);
                    _networkCustomErrorEventHandler(this, networkCustomErrorEventArgs);
                    ReferencePool.Release(networkCustomErrorEventArgs);
                }
            }
        }
    }
}
