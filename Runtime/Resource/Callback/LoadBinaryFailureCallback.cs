﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

namespace ZeroFramework.Resource
{
    /// <summary>
    /// 加载二进制资源失败回调函数。
    /// </summary>
    /// <param name="binaryAssetName">要加载的二进制资源名称。</param>
    /// <param name="status">加载二进制资源状态。</param>
    /// <param name="errorMessage">错误信息。</param>
    /// <param name="userData">用户自定义数据。</param>
    public delegate void LoadBinaryFailureCallback(string binaryAssetName, LoadResourceStatus status,
        string errorMessage, object userData);
}