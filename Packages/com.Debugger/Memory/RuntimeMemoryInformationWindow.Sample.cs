﻿//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework.Debugger
{
    internal sealed partial class RuntimeMemoryInformationWindow<T> : ScrollableDebuggerWindowBase
        where T : UnityEngine.Object
    {
        private sealed class Sample
        {
            private readonly string _name;
            private readonly string _type;
            private readonly long _size;
            private bool _highlight;

            public Sample(string name, string type, long size)
            {
                _name = name;
                _type = type;
                _size = size;
                _highlight = false;
            }

            public string Name => _name;

            public string Type => _type;

            public long Size => _size;

            public bool Highlight
            {
                get => _highlight;
                set => _highlight = value;
            }
        }
    }
}