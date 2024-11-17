//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

using System.Runtime.InteropServices;

namespace ZeroFramework.Config
{
    public sealed partial class ConfigManager : GameFrameworkModule, IConfigManager
    {
        [StructLayout(LayoutKind.Auto)]
        private struct ConfigData
        {
            private readonly bool m_BoolValue;
            private readonly int m_IntValue;
            private readonly float m_FloatValue;
            private readonly string m_StringValue;

            public ConfigData(bool boolValue, int intValue, float floatValue, string stringValue)
            {
                m_BoolValue = boolValue;
                m_IntValue = intValue;
                m_FloatValue = floatValue;
                m_StringValue = stringValue;
            }

            public bool BoolValue => m_BoolValue;

            public int IntValue => m_IntValue;

            public float FloatValue => m_FloatValue;

            public string StringValue => m_StringValue;
        }
    }
}
