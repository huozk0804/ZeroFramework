﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

using System.Runtime.InteropServices;

namespace ZeroFramework.Resource
{
    public partial struct PackageVersionList
    {
        /// <summary>
        /// 资源组。
        /// </summary>
        [StructLayout(LayoutKind.Auto)]
        public struct ResourceGroup
        {
            private static readonly int[] EmptyIntArray = new int[] { };

            private readonly string m_Name;
            private readonly int[] m_ResourceIndexes;

            /// <summary>
            /// 初始化资源组的新实例。
            /// </summary>
            /// <param name="name">资源组名称。</param>
            /// <param name="resourceIndexes">资源组包含的资源索引集合。</param>
            public ResourceGroup(string name, int[] resourceIndexes)
            {
                if (name == null)
                {
                    throw new GameFrameworkException("Name is invalid.");
                }

                m_Name = name;
                m_ResourceIndexes = resourceIndexes ?? EmptyIntArray;
            }

            /// <summary>
            /// 获取资源组名称。
            /// </summary>
            public string Name => m_Name;

            /// <summary>
            /// 获取资源组包含的资源索引集合。
            /// </summary>
            /// <returns>资源组包含的资源索引集合。</returns>
            public int[] GetResourceIndexes()
            {
                return m_ResourceIndexes;
            }
        }
    }
}