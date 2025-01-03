﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

namespace ZeroFramework.Debugger
{
    internal sealed partial class RuntimeMemoryInformationWindow<T> : ScrollableDebuggerWindowBase
        where T : UnityEngine.Object
    {
        private sealed class Sample
        {
            private readonly string m_Name;
            private readonly string m_Type;
            private readonly long m_Size;
            private bool m_Highlight;

            public Sample(string name, string type, long size)
            {
                m_Name = name;
                m_Type = type;
                m_Size = size;
                m_Highlight = false;
            }

            public string Name => m_Name;

            public string Type => m_Type;

            public long Size => m_Size;

            public bool Highlight
            {
                get => m_Highlight;
                set => m_Highlight = value;
            }
        }
    }
}