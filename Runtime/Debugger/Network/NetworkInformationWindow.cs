﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

using ZeroFramework.Network;
using UnityEngine;

namespace ZeroFramework.Debugger
{
    internal sealed class NetworkInformationWindow : ScrollableDebuggerWindowBase
    {
        private INetworkManager m_NetworkManager = null;

        public override void Initialize(params object[] args)
        {
            m_NetworkManager = Zero.Instance.Network;
            if (m_NetworkManager == null)
            {
                Log.Fatal("Network component is invalid.");
                return;
            }
        }

        protected override void OnDrawScrollableWindow()
        {
            GUILayout.Label("<b>Network Information</b>");
            GUILayout.BeginVertical("box");
            {
                DrawItem("Network Channel Count", m_NetworkManager.NetworkChannelCount.ToString());
            }
            GUILayout.EndVertical();
            INetworkChannel[] networkChannels = m_NetworkManager.GetAllNetworkChannels();
            for (int i = 0; i < networkChannels.Length; i++)
            {
                DrawNetworkChannel(networkChannels[i]);
            }
        }

        private void DrawNetworkChannel(INetworkChannel networkChannel)
        {
            GUILayout.Label(Utility.Text.Format("<b>Network Channel: {0} ({1})</b>", networkChannel.Name,
                networkChannel.Connected ? "Connected" : "Disconnected"));
            GUILayout.BeginVertical("box");
            {
                DrawItem("Service Type", networkChannel.ServiceType.ToString());
                DrawItem("Address Family", networkChannel.AddressFamily.ToString());
                DrawItem("Local Address",
                    networkChannel.Connected ? networkChannel.Socket.LocalEndPoint.ToString() : "Unavailable");
                DrawItem("Remote Address",
                    networkChannel.Connected ? networkChannel.Socket.RemoteEndPoint.ToString() : "Unavailable");
                DrawItem("Send Packet",
                    Utility.Text.Format("{0} / {1}", networkChannel.SendPacketCount,
                        networkChannel.SentPacketCount));
                DrawItem("Receive Packet",
                    Utility.Text.Format("{0} / {1}", networkChannel.ReceivePacketCount,
                        networkChannel.ReceivedPacketCount));
                DrawItem("Miss Heart Beat Count", networkChannel.MissHeartBeatCount.ToString());
                DrawItem("Heart Beat",
                    Utility.Text.Format("{0:F2} / {1:F2}", networkChannel.HeartBeatElapseSeconds,
                        networkChannel.HeartBeatInterval));
                if (networkChannel.Connected)
                {
                    if (GUILayout.Button("Disconnect", GUILayout.Height(30f)))
                    {
                        networkChannel.Close();
                    }
                }
            }
            GUILayout.EndVertical();
        }
    }
}