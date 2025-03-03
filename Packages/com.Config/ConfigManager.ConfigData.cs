//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System.Runtime.InteropServices;

namespace ZeroFramework.Config
{
    public sealed partial class ConfigManager : GameFrameworkModule, IConfigManager
    {
        [StructLayout(LayoutKind.Auto)]
        private struct ConfigData
        {
            private readonly bool _boolValue;
            private readonly int _intValue;
            private readonly float _floatValue;
            private readonly string _stringValue;

            public ConfigData(bool boolValue, int intValue, float floatValue, string stringValue)
            {
                _boolValue = boolValue;
                _intValue = intValue;
                _floatValue = floatValue;
                _stringValue = stringValue;
            }

            public bool BoolValue => _boolValue;

            public int IntValue => _intValue;

            public float FloatValue => _floatValue;

            public string StringValue => _stringValue;
        }
    }
}
