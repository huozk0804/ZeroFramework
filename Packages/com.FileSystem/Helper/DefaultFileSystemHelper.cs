//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;

namespace ZeroFramework.FileSystem
{
    /// <summary>
    /// 默认文件系统辅助器。
    /// </summary>
    public class DefaultFileSystemHelper : FileSystemHelperBase
    {
        private const string AndroidFileSystemPrefixString = "jar:";

        /// <summary>
        /// 创建文件系统流。
        /// </summary>
        /// <param name="fullPath">要加载的文件系统的完整路径。</param>
        /// <param name="access">要加载的文件系统的访问方式。</param>
        /// <param name="createNew">是否创建新的文件系统流。</param>
        /// <returns>创建的文件系统流。</returns>
        public override FileSystemStream CreateFileSystemStream(string fullPath, FileSystemAccess access, bool createNew)
        {
            if (fullPath.StartsWith(AndroidFileSystemPrefixString, StringComparison.Ordinal))
            {
                return new AndroidFileSystemStream(fullPath, access, createNew);
            }
            else
            {
                return new CommonFileSystemStream(fullPath, access, createNew);
            }
        }
    }
}
