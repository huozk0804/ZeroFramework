//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using UnityEngine;

namespace ZeroFramework
{
    /// <summary>
    /// 默认游戏框架日志辅助器。
    /// </summary>
    public class DefaultLogHelper : GameFrameworkLog.ILogHelper
    {
        private const int BUFFER_SIZE = 8192;
        private const int FLUSH_INTERVAL = 10;

        private readonly StringBuilder _buffer = new StringBuilder(BUFFER_SIZE);
        private readonly ConcurrentQueue<string> _logQueue = new ConcurrentQueue<string>();
        private readonly object _fileLock = new object();
        private readonly Timer _flushTimer;

        private FileStream _fileStream;
        private StreamWriter _writer;

        public DefaultLogHelper()
        {
            var logFileName = $"{Application.productName}.log";
            var logFilePath = Path.Combine(Application.persistentDataPath, logFileName);

            if (File.Exists(logFilePath))
                File.Delete(logFilePath);

            try
            {
                _fileStream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
                _writer = new StreamWriter(_fileStream, Encoding.UTF8, BUFFER_SIZE);
            }
            catch (Exception ex)
            {
                Debug.LogError($"日志文件初始化失败: {ex}");
            }

            _flushTimer = Timer.Register(FLUSH_INTERVAL, FlushCallback, null, true);
        }

        /// <summary>
        /// 记录日志。
        /// </summary>
        /// <param name="level">日志等级。</param>
        /// <param name="message">日志内容。</param>
        public void Log(GameFrameworkLogLevel level, object message)
        {
            switch (level)
            {
                case GameFrameworkLogLevel.Debug:
                    Debug.Log(Utility.Text.Format("<color=#888888>{0}</color>", message));
                    break;

                case GameFrameworkLogLevel.Info:
                    Debug.Log(message.ToString());
                    break;

                case GameFrameworkLogLevel.Warning:
                    Debug.LogWarning(message.ToString());
                    break;

                case GameFrameworkLogLevel.Error:
                    Debug.LogError(message.ToString());
                    break;

                default:
                    throw new GameFrameworkException(message.ToString());
            }

            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{level}] {message}\n";
            if (_buffer.Length + logEntry.Length > BUFFER_SIZE)
            {
                FlushBuffer();
            }

            _buffer.Append(logEntry);
        }

        public void Flush()
        {
            FlushBuffer();
            _writer?.Flush();
        }

        private void FlushBuffer()
        {
            if (_buffer.Length == 0)
                return;

            lock (_fileLock)
            {
                try
                {
                    _writer?.Write(_buffer.ToString());
                    _buffer.Clear();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"项目日志写入异常: {ex}");
                }
            }
        }

        private void FlushCallback()
        {
            Flush();
        }

        ~DefaultLogHelper()
        {
            _flushTimer?.Cancel();
            Flush();
            _writer?.Dispose();
            _fileStream?.Dispose();
        }
    }
}