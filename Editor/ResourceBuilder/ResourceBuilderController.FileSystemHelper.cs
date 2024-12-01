//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

using ZeroFramework.FileSystem;

namespace ZeroFramework.Editor.ResourceTools
{
    public sealed partial class ResourceBuilderController
    {
        private sealed class FileSystemHelper : IFileSystemHelper
        {
            public FileSystemStream CreateFileSystemStream(string fullPath, FileSystemAccess access, bool createNew)
            {
                return new CommonFileSystemStream(fullPath, access, createNew);
            }
        }
    }
}