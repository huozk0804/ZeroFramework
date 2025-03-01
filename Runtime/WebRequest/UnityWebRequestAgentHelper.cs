//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;
#if UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#else
using UnityEngine.Experimental.Networking;
#endif
using Utility = ZeroFramework.Utility;

namespace ZeroFramework.WebRequest
{
    /// <summary>
    /// 使用 UnityWebRequest 实现的 Web 请求代理辅助器。
    /// </summary>
    public class UnityWebRequestAgentHelper : WebRequestAgentHelperBase, IDisposable
    {
        private UnityWebRequest _unityWebRequest = null;
        private bool _disposed = false;

        private EventHandler<WebRequestAgentHelperCompleteEventArgs> _completeEventHandler = null;
        private EventHandler<WebRequestAgentHelperErrorEventArgs> _errorEventHandler = null;

        /// <summary>
        /// Web 请求代理辅助器完成事件。
        /// </summary>
        public override event EventHandler<WebRequestAgentHelperCompleteEventArgs> WebRequestAgentHelperComplete
        {
            add => _completeEventHandler += value;
            remove => _completeEventHandler -= value;
        }

        /// <summary>
        /// Web 请求代理辅助器错误事件。
        /// </summary>
        public override event EventHandler<WebRequestAgentHelperErrorEventArgs> WebRequestAgentHelperError
        {
            add => _errorEventHandler += value;
            remove => _errorEventHandler -= value;
        }

        /// <summary>
        /// 通过 Web 请求代理辅助器发送请求。
        /// </summary>
        /// <param name="webRequestUri">要发送的远程地址。</param>
        /// <param name="userData">用户自定义数据。</param>
        public override void Request(string webRequestUri, object userData)
        {
            if (_completeEventHandler == null || _errorEventHandler == null)
            {
                Log.Fatal("Web request agent helper handler is invalid.");
                return;
            }

            WWWFormInfo wwwFormInfo = (WWWFormInfo)userData;
            if (wwwFormInfo.WWWForm == null)
            {
                _unityWebRequest = UnityWebRequest.Get(webRequestUri);
            }
            else
            {
                _unityWebRequest = UnityWebRequest.Post(webRequestUri, wwwFormInfo.WWWForm);
            }

#if UNITY_2017_2_OR_NEWER
            _unityWebRequest.SendWebRequest();
#else
            _unityWebRequest.Send();
#endif
        }

        /// <summary>
        /// 通过 Web 请求代理辅助器发送请求。
        /// </summary>
        /// <param name="webRequestUri">要发送的远程地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="userData">用户自定义数据。</param>
        public override void Request(string webRequestUri, byte[] postData, object userData)
        {
            if (_completeEventHandler == null || _errorEventHandler == null)
            {
                Log.Fatal("Web request agent helper handler is invalid.");
                return;
            }

            _unityWebRequest = UnityWebRequest.PostWwwForm(webRequestUri, Utility.Converter.GetString(postData));
#if UNITY_2017_2_OR_NEWER
            _unityWebRequest.SendWebRequest();
#else
            _unityWebRequest.Send();
#endif
        }

        /// <summary>
        /// 重置 Web 请求代理辅助器。
        /// </summary>
        public override void Reset()
        {
            if (_unityWebRequest != null)
            {
                _unityWebRequest.Dispose();
                _unityWebRequest = null;
            }
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        /// <param name="disposing">释放资源标记。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (_unityWebRequest != null)
                {
                    _unityWebRequest.Dispose();
                    _unityWebRequest = null;
                }
            }

            _disposed = true;
        }

        private void Update()
        {
            if (_unityWebRequest == null || !_unityWebRequest.isDone)
            {
                return;
            }

            bool isError = false;
#if UNITY_2020_2_OR_NEWER
            isError = _unityWebRequest.result != UnityWebRequest.Result.Success;
#elif UNITY_2017_1_OR_NEWER
            isError = _unityWebRequest.isNetworkError || _unityWebRequest.isHttpError;
#else
            isError = _unityWebRequest.isError;
#endif
            if (isError)
            {
                var eventArgs = WebRequestAgentHelperErrorEventArgs.Create(_unityWebRequest.error);
                _errorEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
            else if (_unityWebRequest.downloadHandler.isDone)
            {
                var eventArgs = WebRequestAgentHelperCompleteEventArgs.Create(_unityWebRequest.downloadHandler.data);
                _completeEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }
    }
}