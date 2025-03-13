﻿//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ZeroFramework.Config
{
    /// <summary>
    /// 全局配置管理器。
    /// </summary>
    public sealed partial class ConfigManager : GameFrameworkModule, IConfigManager
    {
        private readonly Dictionary<string, ConfigData> _configDatas;
        private readonly DataProvider<IConfigManager> _dataProvider;
        private IConfigHelper _configHelper;
        private bool _enableLoadConfigUpdateEvent;
        private bool _enableLoadConfigDependencyAssetEvent;

        /// <summary>
        /// 初始化全局配置管理器的新实例。
        /// </summary>
        public ConfigManager()
        {
            _configDatas = new Dictionary<string, ConfigData>(StringComparer.Ordinal);
            _dataProvider = new DataProvider<IConfigManager>(this);
            _configHelper = null;

            ConfigHelperBase configHelper = Helper.CreateHelper(GameFrameworkConfig.Instance.configHelperTypeName,
                GameFrameworkConfig.Instance.configCustomHelper);
            if (configHelper == null)
            {
                Log.Error("Can not create config helper.");
                return;
            }

            SetConfigHelper(configHelper);
            SetDataProviderHelper(configHelper);

            var size = GameFrameworkConfig.Instance.configCachedBytesSize;
            if (size > 0)
            {
                EnsureCachedBytesSize(size);
                EnsureCachedBytesSize(size);
            }
            
            _enableLoadConfigUpdateEvent = GameFrameworkConfig.Instance.enableLoadConfigUpdateEvent;
            _enableLoadConfigDependencyAssetEvent = GameFrameworkConfig.Instance.enableLoadConfigDependencyAssetEvent;
        }

        /// <summary>
        /// 获取全局配置项数量。
        /// </summary>
        public int Count => _configDatas.Count;

        /// <summary>
        /// 获取缓冲二进制流的大小。
        /// </summary>
        public int CachedBytesSize => DataProvider<IConfigManager>.CachedBytesSize;

        /// <summary>
        /// 读取全局配置成功事件。
        /// </summary>
        public event EventHandler<ReadDataSuccessEventArgs> ReadDataSuccess
        {
            add => _dataProvider.ReadDataSuccess += value;
            remove => _dataProvider.ReadDataSuccess -= value;
        }

        /// <summary>
        /// 读取全局配置失败事件。
        /// </summary>
        public event EventHandler<ReadDataFailureEventArgs> ReadDataFailure
        {
            add => _dataProvider.ReadDataFailure += value;
            remove => _dataProvider.ReadDataFailure -= value;
        }

        /// <summary>
        /// 读取全局配置更新事件。
        /// </summary>
        public event EventHandler<ReadDataUpdateEventArgs> ReadDataUpdate
        {
            add => _dataProvider.ReadDataUpdate += value;
            remove => _dataProvider.ReadDataUpdate -= value;
        }

        /// <summary>
        /// 读取全局配置时加载依赖资源事件。
        /// </summary>
        public event EventHandler<ReadDataDependencyAssetEventArgs> ReadDataDependencyAsset
        {
            add => _dataProvider.ReadDataDependencyAsset += value;
            remove => _dataProvider.ReadDataDependencyAsset -= value;
        }

		protected internal override int Priority => 10;

		/// <summary>
		/// 全局配置管理器轮询。
		/// </summary>
		/// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
		/// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
		protected internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

		/// <summary>
		/// 关闭并清理全局配置管理器。
		/// </summary>
		protected internal override void Shutdown()
        {
        }

        /// <summary>
        /// 设置全局配置数据提供者辅助器。
        /// </summary>
        /// <param name="dataProviderHelper">全局配置数据提供者辅助器。</param>
        public void SetDataProviderHelper(IDataProviderHelper<IConfigManager> dataProviderHelper)
        {
            _dataProvider.SetDataProviderHelper(dataProviderHelper);
        }

        /// <summary>
        /// 设置全局配置辅助器。
        /// </summary>
        /// <param name="configHelper">全局配置辅助器。</param>
        public void SetConfigHelper(IConfigHelper configHelper)
        {
            if (configHelper == null)
            {
                throw new GameFrameworkException("Config helper is invalid.");
            }

            _configHelper = configHelper;
        }

        /// <summary>
        /// 确保二进制流缓存分配足够大小的内存并缓存。
        /// </summary>
        /// <param name="ensureSize">要确保二进制流缓存分配内存的大小。</param>
        public void EnsureCachedBytesSize(int ensureSize)
        {
            DataProvider<IConfigManager>.EnsureCachedBytesSize(ensureSize);
        }

        /// <summary>
        /// 释放缓存的二进制流。
        /// </summary>
        public void FreeCachedBytes()
        {
            DataProvider<IConfigManager>.FreeCachedBytes();
        }

        /// <summary>
        /// 读取全局配置。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        public void ReadData(string configAssetName)
        {
            _dataProvider.ReadData(configAssetName);
        }

        /// <summary>
        /// 读取全局配置。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="priority">加载全局配置资源的优先级。</param>
        public void ReadData(string configAssetName, int priority)
        {
            _dataProvider.ReadData(configAssetName, priority);
        }

        /// <summary>
        /// 读取全局配置。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void ReadData(string configAssetName, object userData)
        {
            _dataProvider.ReadData(configAssetName, userData);
        }

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configString">要解析的全局配置字符串。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public bool ParseData(string configString)
        {
            return _dataProvider.ParseData(configString);
        }

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configString">要解析的全局配置字符串。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public bool ParseData(string configString, object userData)
        {
            return _dataProvider.ParseData(configString, userData);
        }

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configBytes">要解析的全局配置二进制流。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public bool ParseData(byte[] configBytes)
        {
            return _dataProvider.ParseData(configBytes);
        }

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configBytes">要解析的全局配置二进制流。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public bool ParseData(byte[] configBytes, object userData)
        {
            return _dataProvider.ParseData(configBytes, userData);
        }

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configBytes">要解析的全局配置二进制流。</param>
        /// <param name="startIndex">全局配置二进制流的起始位置。</param>
        /// <param name="length">全局配置二进制流的长度。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public bool ParseData(byte[] configBytes, int startIndex, int length)
        {
            return _dataProvider.ParseData(configBytes, startIndex, length);
        }

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configBytes">要解析的全局配置二进制流。</param>
        /// <param name="startIndex">全局配置二进制流的起始位置。</param>
        /// <param name="length">全局配置二进制流的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public bool ParseData(byte[] configBytes, int startIndex, int length, object userData)
        {
            return _dataProvider.ParseData(configBytes, startIndex, length, userData);
        }

        /// <summary>
        /// 检查是否存在指定全局配置项。
        /// </summary>
        /// <param name="configName">要检查全局配置项的名称。</param>
        /// <returns>指定的全局配置项是否存在。</returns>
        public bool HasConfig(string configName)
        {
            return GetConfigData(configName).HasValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取布尔值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string configName)
        {
            ConfigData? configData = GetConfigData(configName);
            if (!configData.HasValue)
            {
                throw new GameFrameworkException(Utility.Text.Format("Config name '{0}' is not exist.", configName));
            }

            return configData.Value.BoolValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取布尔值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <param name="defaultValue">当指定的全局配置项不存在时，返回此默认值。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string configName, bool defaultValue)
        {
            ConfigData? configData = GetConfigData(configName);
            return configData.HasValue ? configData.Value.BoolValue : defaultValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取整数值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string configName)
        {
            ConfigData? configData = GetConfigData(configName);
            if (!configData.HasValue)
            {
                throw new GameFrameworkException(Utility.Text.Format("Config name '{0}' is not exist.", configName));
            }

            return configData.Value.IntValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取整数值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <param name="defaultValue">当指定的全局配置项不存在时，返回此默认值。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string configName, int defaultValue)
        {
            ConfigData? configData = GetConfigData(configName);
            return configData.HasValue ? configData.Value.IntValue : defaultValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取浮点数值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string configName)
        {
            ConfigData? configData = GetConfigData(configName);
            if (!configData.HasValue)
            {
                throw new GameFrameworkException(Utility.Text.Format("Config name '{0}' is not exist.", configName));
            }

            return configData.Value.FloatValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取浮点数值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <param name="defaultValue">当指定的全局配置项不存在时，返回此默认值。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string configName, float defaultValue)
        {
            ConfigData? configData = GetConfigData(configName);
            return configData.HasValue ? configData.Value.FloatValue : defaultValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取字符串值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string configName)
        {
            ConfigData? configData = GetConfigData(configName);
            if (!configData.HasValue)
            {
                throw new GameFrameworkException(Utility.Text.Format("Config name '{0}' is not exist.", configName));
            }

            return configData.Value.StringValue;
        }

        /// <summary>
        /// 从指定全局配置项中读取字符串值。
        /// </summary>
        /// <param name="configName">要获取全局配置项的名称。</param>
        /// <param name="defaultValue">当指定的全局配置项不存在时，返回此默认值。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string configName, string defaultValue)
        {
            ConfigData? configData = GetConfigData(configName);
            return configData.HasValue ? configData.Value.StringValue : defaultValue;
        }

        /// <summary>
        /// 增加指定全局配置项。
        /// </summary>
        /// <param name="configName">要增加全局配置项的名称。</param>
        /// <param name="configValue">全局配置项的值。</param>
        /// <returns>是否增加全局配置项成功。</returns>
        public bool AddConfig(string configName, string configValue)
        {
            bool.TryParse(configValue, out var boolValue);
            int.TryParse(configValue, out var intValue);
            float.TryParse(configValue, out var floatValue);
            return AddConfig(configName, boolValue, intValue, floatValue, configValue);
        }

        /// <summary>
        /// 增加指定全局配置项。
        /// </summary>
        /// <param name="configName">要增加全局配置项的名称。</param>
        /// <param name="boolValue">全局配置项布尔值。</param>
        /// <param name="intValue">全局配置项整数值。</param>
        /// <param name="floatValue">全局配置项浮点数值。</param>
        /// <param name="stringValue">全局配置项字符串值。</param>
        /// <returns>是否增加全局配置项成功。</returns>
        public bool AddConfig(string configName, bool boolValue, int intValue, float floatValue, string stringValue)
        {
            if (HasConfig(configName))
            {
                return false;
            }

            _configDatas.Add(configName, new ConfigData(boolValue, intValue, floatValue, stringValue));
            return true;
        }

        /// <summary>
        /// 移除指定全局配置项。
        /// </summary>
        /// <param name="configName">要移除全局配置项的名称。</param>
        public bool RemoveConfig(string configName)
        {
            if (!HasConfig(configName))
            {
                return false;
            }

            return _configDatas.Remove(configName);
        }

        /// <summary>
        /// 清空所有全局配置项。
        /// </summary>
        public void RemoveAllConfigs()
        {
            _configDatas.Clear();
        }

        private ConfigData? GetConfigData(string configName)
        {
            if (string.IsNullOrEmpty(configName))
            {
                throw new GameFrameworkException("Config name is invalid.");
            }

            ConfigData configData = default(ConfigData);
            if (_configDatas.TryGetValue(configName, out configData))
            {
                return configData;
            }

            return null;
        }
    }
}