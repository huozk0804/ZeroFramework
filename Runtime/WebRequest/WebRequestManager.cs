//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ZeroFramework.WebRequest
{
    /// <summary>
    /// Web 请求管理器。
    /// </summary>
    public sealed partial class WebRequestManager : GameFrameworkModule, IWebRequestManager
    {
        private readonly TaskPool<WebRequestTask> _taskPool;
        private float _timeout;
        private EventHandler<WebRequestStartEventArgs> _startEventHandler;
        private EventHandler<WebRequestSuccessEventArgs> _successEventHandler;
        private EventHandler<WebRequestFailureEventArgs> _failureEventHandler;

        /// <summary>
        /// 初始化 Web 请求管理器的新实例。
        /// </summary>
        public WebRequestManager()
        {
            _taskPool = new TaskPool<WebRequestTask>();
            _startEventHandler = null;
            _successEventHandler = null;
            _failureEventHandler = null;

            _timeout = GameFrameworkConfig.Instance.webRequestTimeout;
            var count = GameFrameworkConfig.Instance.webRequestAgentHelperCount;
            var name = GameFrameworkConfig.Instance.webRequestAgentHelperTypeName;
            var baseHelper = GameFrameworkConfig.Instance.webRequestAgentCustomHelper;
            for (int i = 0; i < count; i++)
            {
                WebRequestAgentHelperBase webRequestAgentHelper = Helper.CreateHelper(name, baseHelper, i);
                if (webRequestAgentHelper == null)
                {
                    Log.Error("Can not create web request agent helper.");
                    return;
                }

                webRequestAgentHelper.name = Utility.Text.Format("Web Request Agent Helper - {0}", i);
                AddWebRequestAgentHelper(webRequestAgentHelper);
            }
        }

        /// <summary>
        /// 获取 Web 请求代理总数量。
        /// </summary>
        public int TotalAgentCount => _taskPool.TotalAgentCount;

        /// <summary>
        /// 获取可用 Web 请求代理数量。
        /// </summary>
        public int FreeAgentCount => _taskPool.FreeAgentCount;

        /// <summary>
        /// 获取工作中 Web 请求代理数量。
        /// </summary>
        public int WorkingAgentCount => _taskPool.WorkingAgentCount;

        /// <summary>
        /// 获取等待 Web 请求数量。
        /// </summary>
        public int WaitingTaskCount => _taskPool.WaitingTaskCount;

        /// <summary>
        /// 获取或设置 Web 请求超时时长，以秒为单位。
        /// </summary>
        public float Timeout
        {
            get => _timeout;
            set => _timeout = value;
        }

        /// <summary>
        /// Web 请求开始事件。
        /// </summary>
        public event EventHandler<WebRequestStartEventArgs> WebRequestStart
        {
            add => _startEventHandler += value;
            remove => _startEventHandler -= value;
        }

        /// <summary>
        /// Web 请求成功事件。
        /// </summary>
        public event EventHandler<WebRequestSuccessEventArgs> WebRequestSuccess
        {
            add => _successEventHandler += value;
            remove => _successEventHandler -= value;
        }

        /// <summary>
        /// Web 请求失败事件。
        /// </summary>
        public event EventHandler<WebRequestFailureEventArgs> WebRequestFailure
        {
            add => _failureEventHandler += value;
            remove => _failureEventHandler -= value;
        }

        /// <summary>
        /// Web 请求管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            _taskPool.Update(elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 关闭并清理 Web 请求管理器。
        /// </summary>
        public override void Shutdown()
        {
            _taskPool.Shutdown();
        }

        /// <summary>
        /// 增加 Web 请求代理辅助器。
        /// </summary>
        /// <param name="webRequestAgentHelper">要增加的 Web 请求代理辅助器。</param>
        public void AddWebRequestAgentHelper(IWebRequestAgentHelper webRequestAgentHelper)
        {
            WebRequestAgent agent = new WebRequestAgent(webRequestAgentHelper);
            agent.WebRequestAgentStart += OnWebRequestAgentStart;
            agent.WebRequestAgentSuccess += OnWebRequestAgentSuccess;
            agent.WebRequestAgentFailure += OnWebRequestAgentFailure;

            _taskPool.AddAgent(agent);
        }

        /// <summary>
        /// 根据 Web 请求任务的序列编号获取 Web 请求任务的信息。
        /// </summary>
        /// <param name="serialId">要获取信息的 Web 请求任务的序列编号。</param>
        /// <returns>Web 请求任务的信息。</returns>
        public TaskInfo GetWebRequestInfo(int serialId)
        {
            return _taskPool.GetTaskInfo(serialId);
        }

        /// <summary>
        /// 根据 Web 请求任务的标签获取 Web 请求任务的信息。
        /// </summary>
        /// <param name="tag">要获取信息的 Web 请求任务的标签。</param>
        /// <returns>Web 请求任务的信息。</returns>
        public TaskInfo[] GetWebRequestInfos(string tag)
        {
            return _taskPool.GetTaskInfos(tag);
        }

        /// <summary>
        /// 根据 Web 请求任务的标签获取 Web 请求任务的信息。
        /// </summary>
        /// <param name="tag">要获取信息的 Web 请求任务的标签。</param>
        /// <param name="results">Web 请求任务的信息。</param>
        public void GetAllWebRequestInfos(string tag, List<TaskInfo> results)
        {
            _taskPool.GetTaskInfos(tag, results);
        }

        /// <summary>
        /// 获取所有 Web 请求任务的信息。
        /// </summary>
        /// <returns>所有 Web 请求任务的信息。</returns>
        public TaskInfo[] GetAllWebRequestInfos()
        {
            return _taskPool.GetAllTaskInfos();
        }

        /// <summary>
        /// 获取所有 Web 请求任务的信息。
        /// </summary>
        /// <param name="results">所有 Web 请求任务的信息。</param>
        public void GetAllWebRequestInfos(List<TaskInfo> results)
        {
            _taskPool.GetAllTaskInfos(results);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri)
        {
            return AddWebRequest(webRequestUri, null, null, Constant.DefaultPriority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData)
        {
            return AddWebRequest(webRequestUri, postData, null, Constant.DefaultPriority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, string tag)
        {
            return AddWebRequest(webRequestUri, null, tag, Constant.DefaultPriority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, int priority)
        {
            return AddWebRequest(webRequestUri, null, null, priority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, object userData)
        {
            return AddWebRequest(webRequestUri, null, null, Constant.DefaultPriority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, string tag)
        {
            return AddWebRequest(webRequestUri, postData, tag, Constant.DefaultPriority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, int priority)
        {
            return AddWebRequest(webRequestUri, postData, null, priority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, object userData)
        {
            return AddWebRequest(webRequestUri, postData, null, Constant.DefaultPriority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, string tag, int priority)
        {
            return AddWebRequest(webRequestUri, null, tag, priority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, string tag, object userData)
        {
            return AddWebRequest(webRequestUri, null, tag, Constant.DefaultPriority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, int priority, object userData)
        {
            return AddWebRequest(webRequestUri, null, null, priority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, string tag, int priority)
        {
            return AddWebRequest(webRequestUri, postData, tag, priority, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, string tag, object userData)
        {
            return AddWebRequest(webRequestUri, postData, tag, Constant.DefaultPriority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, int priority, object userData)
        {
            return AddWebRequest(webRequestUri, postData, null, priority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, string tag, int priority, object userData)
        {
            return AddWebRequest(webRequestUri, null, tag, priority, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="tag">Web 请求任务的标签。</param>
        /// <param name="priority">Web 请求任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, string tag, int priority, object userData)
        {
            if (string.IsNullOrEmpty(webRequestUri))
            {
                throw new GameFrameworkException("Web request uri is invalid.");
            }

            if (TotalAgentCount <= 0)
            {
                throw new GameFrameworkException("You must add web request agent first.");
            }

            WebRequestTask webRequestTask =
                WebRequestTask.Create(webRequestUri, postData, tag, priority, _timeout, userData);
            _taskPool.AddTask(webRequestTask);
            return webRequestTask.SerialId;
        }

        /// <summary>
        /// 根据 Web 请求任务的序列编号移除 Web 请求任务。
        /// </summary>
        /// <param name="serialId">要移除 Web 请求任务的序列编号。</param>
        /// <returns>是否移除 Web 请求任务成功。</returns>
        public bool RemoveWebRequest(int serialId)
        {
            return _taskPool.RemoveTask(serialId);
        }

        /// <summary>
        /// 根据 Web 请求任务的标签移除 Web 请求任务。
        /// </summary>
        /// <param name="tag">要移除 Web 请求任务的标签。</param>
        /// <returns>移除 Web 请求任务的数量。</returns>
        public int RemoveWebRequests(string tag)
        {
            return _taskPool.RemoveTasks(tag);
        }

        /// <summary>
        /// 移除所有 Web 请求任务。
        /// </summary>
        /// <returns>移除 Web 请求任务的数量。</returns>
        public int RemoveAllWebRequests()
        {
            return _taskPool.RemoveAllTasks();
        }

        private void OnWebRequestAgentStart(WebRequestAgent sender)
        {
            if (_startEventHandler != null)
            {
                WebRequestStartEventArgs webRequestStartEventArgs =
                    WebRequestStartEventArgs.Create(sender.Task.SerialId, sender.Task.WebRequestUri,
                        sender.Task.UserData);
                _startEventHandler(this, webRequestStartEventArgs);
                ReferencePool.Release(webRequestStartEventArgs);
            }
        }

        private void OnWebRequestAgentSuccess(WebRequestAgent sender, byte[] webResponseBytes)
        {
            if (_successEventHandler != null)
            {
                WebRequestSuccessEventArgs webRequestSuccessEventArgs =
                    WebRequestSuccessEventArgs.Create(sender.Task.SerialId, sender.Task.WebRequestUri, webResponseBytes,
                        sender.Task.UserData);
                _successEventHandler(this, webRequestSuccessEventArgs);
                ReferencePool.Release(webRequestSuccessEventArgs);
            }
        }

        private void OnWebRequestAgentFailure(WebRequestAgent sender, string errorMessage)
        {
            if (_failureEventHandler != null)
            {
                WebRequestFailureEventArgs webRequestFailureEventArgs =
                    WebRequestFailureEventArgs.Create(sender.Task.SerialId, sender.Task.WebRequestUri, errorMessage,
                        sender.Task.UserData);
                _failureEventHandler(this, webRequestFailureEventArgs);
                ReferencePool.Release(webRequestFailureEventArgs);
            }
        }
    }
}