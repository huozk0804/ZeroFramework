﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

namespace ZeroFramework.Resource
{
    /// <summary>
    /// 版本资源列表更新成功回调函数。
    /// </summary>
    /// <param name="downloadPath">版本资源列表更新后存放路径。</param>
    /// <param name="downloadUri">版本资源列表更新地址。</param>
    public delegate void UpdateVersionListSuccessCallback(string downloadPath, string downloadUri);
}