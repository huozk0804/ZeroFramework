﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

using System;
using UnityEngine;

namespace ZeroFramework.Debugger
{
    /// <summary>
    /// 日志记录结点。
    /// </summary>
    public sealed class LogNode : IReference
    {
        private DateTime m_LogTime;
        private int m_LogFrameCount;
        private LogType m_LogType;
        private string m_LogMessage;
        private string m_StackTrack;

        /// <summary>
        /// 初始化日志记录结点的新实例。
        /// </summary>
        public LogNode()
        {
            m_LogTime = default(DateTime);
            m_LogFrameCount = 0;
            m_LogType = LogType.Error;
            m_LogMessage = null;
            m_StackTrack = null;
        }

        /// <summary>
        /// 获取日志时间。
        /// </summary>
        public DateTime LogTime => m_LogTime;

        /// <summary>
        /// 获取日志帧计数。
        /// </summary>
        public int LogFrameCount => m_LogFrameCount;

        /// <summary>
        /// 获取日志类型。
        /// </summary>
        public LogType LogType => m_LogType;

        /// <summary>
        /// 获取日志内容。
        /// </summary>
        public string LogMessage => m_LogMessage;

        /// <summary>
        /// 获取日志堆栈信息。
        /// </summary>
        public string StackTrack => m_StackTrack;

        /// <summary>
        /// 创建日志记录结点。
        /// </summary>
        /// <param name="logType">日志类型。</param>
        /// <param name="logMessage">日志内容。</param>
        /// <param name="stackTrack">日志堆栈信息。</param>
        /// <returns>创建的日志记录结点。</returns>
        public static LogNode Create(LogType logType, string logMessage, string stackTrack)
        {
            LogNode logNode = ReferencePool.Acquire<LogNode>();
            logNode.m_LogTime = DateTime.UtcNow;
            logNode.m_LogFrameCount = Time.frameCount;
            logNode.m_LogType = logType;
            logNode.m_LogMessage = logMessage;
            logNode.m_StackTrack = stackTrack;
            return logNode;
        }

        /// <summary>
        /// 清理日志记录结点。
        /// </summary>
        public void Clear()
        {
            m_LogTime = default(DateTime);
            m_LogFrameCount = 0;
            m_LogType = LogType.Error;
            m_LogMessage = null;
            m_StackTrack = null;
        }
    }
}