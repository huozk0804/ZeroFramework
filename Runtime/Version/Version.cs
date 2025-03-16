//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;
using Cysharp.Threading.Tasks;

namespace ZeroFramework
{
    /// <summary>
    /// 版本号类。
    /// </summary>
    public static partial class Version
    {
        private const string FrameworkVersionString = "2021.12.29";
        private static VersionInfo _LocalVersion = null;
        private static VersionInfo _RemoteVersion = null;
        private static IVersionHelper _VersionHelper;
        
        /// <summary>
        /// 是否已经加载完本地版本文件
        /// </summary>
        public static bool LocalLoaded => _LocalVersion != null;

        /// <summary>
        /// 是否已经加载完远程版本文件
        /// </summary>
        public static bool RemoteLoaded => _RemoteVersion != null;

        /// <summary>
        /// 游戏框架版本号。
        /// </summary>
        public static string ZeroFrameworkVersion => FrameworkVersionString;

        /// <summary>
        /// 游戏版本号(major.module.hotupdate)
        /// (会有变动，如果没有加载远程版本文件则为本地版本配置,否则为远程版本配置)
        /// </summary>
        public static string GameVersion =>
            _RemoteVersion != null ? _RemoteVersion.gameVersion : _LocalVersion.gameVersion;

        /// <summary>
        /// 资源版本号(auto-in)
        /// (会有变动，如果没有加载远程版本文件则为本地版本配置,否则为远程版本配置)
        /// </summary>
        public static string ResVersion =>
            _RemoteVersion != null ? _RemoteVersion.resVersion : _LocalVersion.resVersion;

        /// <summary>
        /// 远程CDN地址
        /// (会有变动，如果没有加载远程版本文件则为本地版本配置,否则为远程版本配置)
        /// </summary>
        public static string CdnUrl => _RemoteVersion != null ? _RemoteVersion.cdnUrl : _LocalVersion.cdnUrl;

        /// <summary>
        /// 服务器地址
        /// (会有变动，如果没有加载远程版本文件则为本地版本配置,否则为远程版本配置)
        /// </summary>
        public static string ServerUrl => _RemoteVersion != null ? _RemoteVersion.serverUrl : _LocalVersion.serverUrl;
        
        /// <summary>
        /// 公告地址
        /// (会有变动，如果没有加载远程版本文件则为本地版本配置,否则为远程版本配置)
        /// </summary>
        public static string NoticeUrl => _RemoteVersion != null ? _RemoteVersion.noticeUrl : _LocalVersion.noticeUrl;
        
        /// <summary>
        /// 平台索引
        /// (会有变动，如果没有加载远程版本文件则为本地版本配置,否则为远程版本配置)
        /// </summary>
        public static int Platform => _RemoteVersion != null ? _RemoteVersion.platform : _LocalVersion.platform;
        
        /// <summary>
        /// 频道索引
        /// (会有变动，如果没有加载远程版本文件则为本地版本配置,否则为远程版本配置)
        /// </summary>
        public static int Channel => _RemoteVersion != null ? _RemoteVersion.channel : _LocalVersion.channel;

        /// <summary>
        /// 资源包名单
        /// (会有变动，如果没有加载远程版本文件则为本地版本配置,否则为远程版本配置)
        /// </summary>
        public static string[] ResourcePackages =>
            _RemoteVersion != null ? _RemoteVersion.resPackage : _LocalVersion.resPackage;

        /// <summary>
        /// 资源包名单
        /// (会有变动，如果没有加载远程版本文件则为本地版本配置,否则为远程版本配置)
        /// </summary>
        public static string[] DLLNames =>
            _RemoteVersion != null ? _RemoteVersion.resPackage : _LocalVersion.resPackage;

        /// <summary>
        /// 资源包名单
        /// (会有变动，如果没有加载远程版本文件则为本地版本配置,否则为远程版本配置)
        /// </summary>
        public static string[] AotDLLNames =>
            _RemoteVersion != null ? _RemoteVersion.resPackage : _LocalVersion.resPackage;

        /// <summary>
        /// 设置版本号辅助器。
        /// </summary>
        /// <param name="versionHelper">要设置的版本号辅助器。</param>
        /// <param name="isLoadLocalVersion">是否需要立即加载本地版本(默认不加载)</param>
        public static void SetVersionHelper(IVersionHelper versionHelper, bool isLoadLocalVersion = false)
        {
            _VersionHelper = versionHelper;
            if (isLoadLocalVersion)
                _VersionHelper.LoadLocalVersion();
        }

        /// <summary>
        /// 根据路径加载本地版本文件
        /// </summary>
        /// <param name="localPath">相对路径</param>
        public static void LoadLocalVersion(string localPath = null)
        {
            if (_VersionHelper == null)
                throw new GameFrameworkException("VersionHelper is null.");

            _LocalVersion = _VersionHelper.LoadLocalVersion(localPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public static async UniTask GetRemoteVersion(string url)
        {
            if (_VersionHelper == null)
                throw new GameFrameworkException("VersionHelper is null.");

            try
            {
                var result = await _VersionHelper.GetRemoteVersion(url);
                _RemoteVersion = result;
            }
            catch (Exception e)
            {
                throw new GameFrameworkException(e.Message);
            }
        }

        /// <summary>
        /// 是否需要强制更新
        /// </summary>
        public static bool IsForceUpdate()
        {
            if (_VersionHelper == null)
                throw new GameFrameworkException("VersionHelper is null.");

            return _VersionHelper.IsForceUpdate(_LocalVersion.gameVersion, _RemoteVersion.gameVersion);
        }

        /// <summary>
        /// 保存远程最新的版本文件到本地
        /// </summary>
        public static void SaveRemoteVersion()
        {
            if (_VersionHelper == null)
                throw new GameFrameworkException("VersionHelper is null.");

            _VersionHelper.SaveNewVersion(_RemoteVersion);
        }

        /// <summary>
        /// 更新最新的版本信息
        /// (一般用于编辑器打包时的配置)
        /// </summary>
        public static void SaveNewestVersion(VersionInfo newInfo)
        {
            /*
             * 1.更新内存数据
             * 2.更新本地txt数据
             */
            if (!LocalLoaded)
            {
                _LocalVersion = newInfo;
            }
            else
            {
                if (_LocalVersion.gameVersion != newInfo.gameVersion)
                    _LocalVersion.gameVersion = newInfo.gameVersion;
            }

            _LocalVersion = newInfo;
        }

        
    }
}