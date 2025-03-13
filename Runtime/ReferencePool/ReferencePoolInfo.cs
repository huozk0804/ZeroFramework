//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace ZeroFramework
{
    /// <summary>
    /// 引用池信息。
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct ReferencePoolInfo
    {
        private readonly Type _type;
        private readonly int _unusedReferenceCount;
        private readonly int _usingReferenceCount;
        private readonly int _acquireReferenceCount;
        private readonly int _releaseReferenceCount;
        private readonly int _addReferenceCount;
        private readonly int _removeReferenceCount;

        /// <summary>
        /// 初始化引用池信息的新实例。
        /// </summary>
        /// <param name="type">引用池类型。</param>
        /// <param name="unusedReferenceCount">未使用引用数量。</param>
        /// <param name="usingReferenceCount">正在使用引用数量。</param>
        /// <param name="acquireReferenceCount">获取引用数量。</param>
        /// <param name="releaseReferenceCount">归还引用数量。</param>
        /// <param name="addReferenceCount">增加引用数量。</param>
        /// <param name="removeReferenceCount">移除引用数量。</param>
        public ReferencePoolInfo(Type type, int unusedReferenceCount, int usingReferenceCount,
            int acquireReferenceCount, int releaseReferenceCount, int addReferenceCount, int removeReferenceCount)
        {
            _type = type;
            _unusedReferenceCount = unusedReferenceCount;
            _usingReferenceCount = usingReferenceCount;
            _acquireReferenceCount = acquireReferenceCount;
            _releaseReferenceCount = releaseReferenceCount;
            _addReferenceCount = addReferenceCount;
            _removeReferenceCount = removeReferenceCount;
        }

        /// <summary>
        /// 获取引用池类型。
        /// </summary>
        public Type Type => _type;

        /// <summary>
        /// 获取未使用引用数量。
        /// </summary>
        public int UnusedReferenceCount => _unusedReferenceCount;

        /// <summary>
        /// 获取正在使用引用数量。
        /// </summary>
        public int UsingReferenceCount => _usingReferenceCount;

        /// <summary>
        /// 获取获取引用数量。
        /// </summary>
        public int AcquireReferenceCount => _acquireReferenceCount;

        /// <summary>
        /// 获取归还引用数量。
        /// </summary>
        public int ReleaseReferenceCount => _releaseReferenceCount;

        /// <summary>
        /// 获取增加引用数量。
        /// </summary>
        public int AddReferenceCount => _addReferenceCount;

        /// <summary>
        /// 获取移除引用数量。
        /// </summary>
        public int RemoveReferenceCount => _removeReferenceCount;
    }
}