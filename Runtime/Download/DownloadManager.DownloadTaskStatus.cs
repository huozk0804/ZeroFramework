﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

namespace ZeroFramework.Download
{
    public sealed partial class DownloadManager : GameFrameworkModule, IDownloadManager
    {
        /// <summary>
        /// 下载任务的状态。
        /// </summary>
        private enum DownloadTaskStatus : byte
        {
            /// <summary>
            /// 准备下载。
            /// </summary>
            Todo = 0,

            /// <summary>
            /// 下载中。
            /// </summary>
            Doing,

            /// <summary>
            /// 下载完成。
            /// </summary>
            Done,

            /// <summary>
            /// 下载错误。
            /// </summary>
            Error
        }
    }
}
