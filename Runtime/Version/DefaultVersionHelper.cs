//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroFramework
{
    /// <summary>
    /// 默认版本号辅助器。
    /// </summary>
    public class DefaultVersionHelper : Version.IVersionHelper
    {
        public string VersionFileName => "version.txt";
        private StringBuilder _stringBuilder = new StringBuilder();

        /// <summary>
        /// 从本地路径加载版本文件(version.txt)
        /// </summary>
        /// <param name="localPath">默认 persistentDataPath -> streamingAssetsPath 填写相对路径</param>
        /// <returns></returns>
        public Version.VersionInfo LoadLocalVersion(string localPath = "")
        {
            string path;
            string content;
#if UNITY_EDITOR
            path = Path.Combine(localPath.IsNullOrEmpty() ? Application.streamingAssetsPath : localPath,
                VersionFileName);
#else
            if (localPath.IsNullOrEmpty())
                localPath = Path.Combine(Application.persistentDataPath, VersionFileName);
            
            if (!File.Exists(localPath))
            {
                // 首次运行，从StreamingAssets复制初始版本
                string initialPath = Path.Combine(Application.streamingAssetsPath, VersionFileName);
                File.Copy(initialPath, localPath);
            }
            
            path = Path.Combine(Application.persistentDataPath, VersionFileName);
#endif

#if UNITY_ANDROID
			// Android平台需通过UnityWebRequest读取StreamingAssets
            string url = Path.Combine(Application.streamingAssetsPath, VersionFileName);
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SendWebRequest().completed += (op) => 
            {
                if (request.result == UnityWebRequest.Result.Success) 
                {
                    content = request.downloadHandler.text;
                }
            };
#else
            content = File.ReadAllText(path).Trim();
#endif
            var data = ParseCustomString(content);
            return data;
        }

        /// <summary>
        /// 获取远端的版本文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async UniTask<Version.VersionInfo> GetRemoteVersion(string url)
        {
            using var request = UnityWebRequest.Get(url);
            try
            {
                request.timeout = 15;
                await request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ProtocolError)
                    throw new GameFrameworkException(
                        $"Load remote version http error {request.responseCode} : {request.error}");

                if (request.result != UnityWebRequest.Result.Success)
                    throw new GameFrameworkException($"Load remote version error : {request.error}");

                if (string.IsNullOrEmpty(request.downloadHandler.text))
                    throw new GameFrameworkException("Empty version data.");

                var data = ParseCustomString(request.downloadHandler.text);
                return data;
            }
            finally
            {
                if (!request.isDone)
                    request.Abort();
            }
        }

        /// <summary>
        /// 把新的版本文件写入到persistentDataPath
        /// </summary>
        /// <param name="path">自定义相对路径</param>
        public void SaveNewVersion(Version.VersionInfo data, string path = null)
        {
            if (data == null)
                throw new GameFrameworkException($"save new version not null. {nameof(data)}");

            try
            {
                string jsonContent = ParseCustomVersion(data);
                string targetDirectory = Application.persistentDataPath;
                if (!string.IsNullOrEmpty(path))
                {
                    targetDirectory = Path.Combine(targetDirectory, path);
                    if (!Directory.Exists(targetDirectory))
                        Directory.CreateDirectory(targetDirectory);
                }

                string fullPath = Path.Combine(targetDirectory, VersionFileName);
                string tempPath = Path.Combine(targetDirectory, $"_{VersionFileName}.tmp");

                File.WriteAllText(tempPath, jsonContent, Encoding.UTF8);
                File.Move(tempPath, fullPath);
            }
            catch (Exception ex)
            {
                throw new GameFrameworkException("Version file save failed", ex);
            }
        }


        /// <summary>
        /// 是否需要强制更新
        /// </summary>
        /// <param name="localVer">本地版本</param>
        /// <param name="remoteVer">远程最新版本</param>
        /// <returns></returns>
        public bool IsForceUpdate(string localVer, string remoteVer)
        {
            int[] localParts = localVer.Split('.').Select(int.Parse).ToArray();
            int[] remoteParts = remoteVer.Split('.').Select(int.Parse).ToArray();

            // 主版本号（第一位/第二位）不同则强制更新
            if (remoteParts[0] > localParts[0] || remoteParts[1] > localParts[1])
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 自定义转换版本文件
        /// </summary>
        /// <param name="content">读取的字符串内容</param>
        public Version.VersionInfo ParseCustomString(string content)
        {
            var info = new Version.VersionInfo();
            var lines = content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ':' }, 2);
                if (parts.Length < 2) continue;

                string key = parts[0].Trim();
                string value = parts[1].Trim().Trim('"', ' ', '[', ']');

                switch (key)
                {
                    case "gameVersion":
                        info.gameVersion = value;
                        break;
                    case "resVersion":
                        info.resVersion = value;
                        break;
                    case "cdnUrl":
                        info.cdnUrl = value;
                        break;
                    case "serverUrl":
                        info.serverUrl = value;
                        break;
                    case "noticeUrl":
                        info.noticeUrl = value;
                        break;
                    case "platform":
                        info.platform = int.Parse(value);
                        break;
                    case "channel":
                        info.channel = int.Parse(value);
                        break;
                    case "resPackage":
                        info.resPackage = value.Split(',')
                            .Select(s => s.Trim().Trim('"'))
                            .ToArray();
                        break;
                    case "dllName":
                        info.dllName = value.Split(',')
                            .Select(s => s.Trim().Trim('"'))
                            .ToArray();
                        break;
                    case "aotDllName":
                        info.aotDllName = value.Split(',')
                            .Select(s => s.Trim().Trim('"'))
                            .ToArray();
                        break;
                }
            }

            return info;
        }

        /// <summary>
        /// 把实例转换为string
        /// </summary>
        /// <param name="data"></param>
        public string ParseCustomVersion(Version.VersionInfo data)
        {
            string AppendArray(string[] array)
            {
                return array == null || array.Length == 0 ? "[]" : $"\"{string.Join("\",\"", array)}\"";
            }

            _stringBuilder.Clear();
            _stringBuilder.AppendLine($"gameVersion:{data.gameVersion}");
            _stringBuilder.AppendLine($"resVersion:{data.resVersion}");
            _stringBuilder.AppendLine($"cdnUrl:{data.cdnUrl}");
            _stringBuilder.AppendLine($"serverUrl:{data.serverUrl}");
            _stringBuilder.AppendLine($"noticeUrl:{data.noticeUrl}");
            _stringBuilder.AppendLine($"platform:{data.platform}");
            _stringBuilder.AppendLine($"channel:{data.channel}");
            _stringBuilder.AppendLine($"resPackage:{AppendArray(data.resPackage)}");
            _stringBuilder.AppendLine($"dllName:{AppendArray(data.dllName)}");
            _stringBuilder.AppendLine($"aotDllName:{AppendArray(data.aotDllName)}");
            return _stringBuilder.ToString();
        }
    }
}