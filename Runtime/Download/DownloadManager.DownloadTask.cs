//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework
{
    public sealed partial class DownloadManager : GameFrameworkModule, IDownloadManager
    {
        /// <summary>
        /// 下载任务。
        /// </summary>
        private sealed class DownloadTask : TaskBase
        {
            private static int Serial = 0;

            private DownloadTaskStatus _status;
            private string _downloadPath;
            private string _downloadUri;
            private int _flushSize;
            private float _timeout;

            /// <summary>
            /// 初始化下载任务的新实例。
            /// </summary>
            public DownloadTask()
            {
                _status = DownloadTaskStatus.Todo;
                _downloadPath = null;
                _downloadUri = null;
                _flushSize = 0;
                _timeout = 0f;
            }

            /// <summary>
            /// 获取或设置下载任务的状态。
            /// </summary>
            public DownloadTaskStatus Status
            {
                get => _status;
                set => _status = value;
            }

            /// <summary>
            /// 获取下载后存放路径。
            /// </summary>
            public string DownloadPath => _downloadPath;

            /// <summary>
            /// 获取原始下载地址。
            /// </summary>
            public string DownloadUri => _downloadUri;

            /// <summary>
            /// 获取将缓冲区写入磁盘的临界大小。
            /// </summary>
            public int FlushSize => _flushSize;

            /// <summary>
            /// 获取下载超时时长，以秒为单位。
            /// </summary>
            public float Timeout => _timeout;

            /// <summary>
            /// 获取下载任务的描述。
            /// </summary>
            public override string Description => _downloadPath;

            /// <summary>
            /// 创建下载任务。
            /// </summary>
            /// <param name="downloadPath">下载后存放路径。</param>
            /// <param name="downloadUri">原始下载地址。</param>
            /// <param name="tag">下载任务的标签。</param>
            /// <param name="priority">下载任务的优先级。</param>
            /// <param name="flushSize">将缓冲区写入磁盘的临界大小。</param>
            /// <param name="timeout">下载超时时长，以秒为单位。</param>
            /// <param name="userData">用户自定义数据。</param>
            /// <returns>创建的下载任务。</returns>
            public static DownloadTask Create(string downloadPath, string downloadUri, string tag, int priority, int flushSize, float timeout, object userData)
            {
                DownloadTask downloadTask = ReferencePool.Acquire<DownloadTask>();
                downloadTask.Initialize(++Serial, tag, priority, userData);
                downloadTask._downloadPath = downloadPath;
                downloadTask._downloadUri = downloadUri;
                downloadTask._flushSize = flushSize;
                downloadTask._timeout = timeout;
                return downloadTask;
            }

            /// <summary>
            /// 清理下载任务。
            /// </summary>
            public override void Clear()
            {
                base.Clear();
                _status = DownloadTaskStatus.Todo;
                _downloadPath = null;
                _downloadUri = null;
                _flushSize = 0;
                _timeout = 0f;
            }
        }
    }
}
