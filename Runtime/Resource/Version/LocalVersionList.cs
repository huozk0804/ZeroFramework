﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

using System.Runtime.InteropServices;

namespace ZeroFramework.Resource
{
    /// <summary>
    /// 本地版本资源列表。
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public partial struct LocalVersionList
    {
        private static readonly Resource[] EmptyResourceArray = new Resource[] { };
        private static readonly FileSystem[] EmptyFileSystemArray = new FileSystem[] { };

        private readonly bool m_IsValid;
        private readonly Resource[] m_Resources;
        private readonly FileSystem[] m_FileSystems;

        /// <summary>
        /// 初始化本地版本资源列表的新实例。
        /// </summary>
        /// <param name="resources">包含的资源集合。</param>
        /// <param name="fileSystems">包含的文件系统集合。</param>
        public LocalVersionList(Resource[] resources, FileSystem[] fileSystems)
        {
            m_IsValid = true;
            m_Resources = resources ?? EmptyResourceArray;
            m_FileSystems = fileSystems ?? EmptyFileSystemArray;
        }

        /// <summary>
        /// 获取本地版本资源列表是否有效。
        /// </summary>
        public bool IsValid => m_IsValid;

        /// <summary>
        /// 获取包含的资源集合。
        /// </summary>
        /// <returns>包含的资源集合。</returns>
        public Resource[] GetResources()
        {
            if (!m_IsValid)
            {
                throw new GameFrameworkException("Data is invalid.");
            }

            return m_Resources;
        }

        /// <summary>
        /// 获取包含的文件系统集合。
        /// </summary>
        /// <returns>包含的文件系统集合。</returns>
        public FileSystem[] GetFileSystems()
        {
            if (!m_IsValid)
            {
                throw new GameFrameworkException("Data is invalid.");
            }

            return m_FileSystems;
        }
    }
}