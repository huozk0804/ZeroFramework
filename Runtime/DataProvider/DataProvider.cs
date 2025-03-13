//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;
using ZeroFramework.Resource;

namespace ZeroFramework
{
    /// <summary>
    /// 数据提供者。
    /// </summary>
    /// <typeparam name="T">数据提供者的持有者的类型。</typeparam>
    internal sealed class DataProvider<T> : IDataProvider<T>
    {
        private const int BlockSize = 1024 * 4;
        private static byte[] cachedBytes = null;

        private readonly T _owner;
        private IDataProviderHelper<T> _dataProviderHelper;
        private EventHandler<ReadDataSuccessEventArgs> _readDataSuccessEventHandler;
        private EventHandler<ReadDataFailureEventArgs> _readDataFailureEventHandler;
        private EventHandler<ReadDataUpdateEventArgs> _readDataUpdateEventHandler;
        private EventHandler<ReadDataDependencyAssetEventArgs> _readDataDependencyAssetEventHandler;

        /// <summary>
        /// 初始化数据提供者的新实例。
        /// </summary>
        /// <param name="owner">数据提供者的持有者。</param>
        public DataProvider(T owner)
        {
            _owner = owner;
            _dataProviderHelper = null;
            _readDataSuccessEventHandler = null;
            _readDataFailureEventHandler = null;
            _readDataUpdateEventHandler = null;
            _readDataDependencyAssetEventHandler = null;
        }

        /// <summary>
        /// 获取缓冲二进制流的大小。
        /// </summary>
        public static int CachedBytesSize => cachedBytes?.Length ?? 0;

        /// <summary>
        /// 读取数据成功事件。
        /// </summary>
        public event EventHandler<ReadDataSuccessEventArgs> ReadDataSuccess
        {
            add => _readDataSuccessEventHandler += value;
            remove => _readDataSuccessEventHandler -= value;
        }

        /// <summary>
        /// 读取数据失败事件。
        /// </summary>
        public event EventHandler<ReadDataFailureEventArgs> ReadDataFailure
        {
            add => _readDataFailureEventHandler += value;
            remove => _readDataFailureEventHandler -= value;
        }

        /// <summary>
        /// 读取数据更新事件。
        /// </summary>
        public event EventHandler<ReadDataUpdateEventArgs> ReadDataUpdate
        {
            add => _readDataUpdateEventHandler += value;
            remove => _readDataUpdateEventHandler -= value;
        }

        /// <summary>
        /// 读取数据时加载依赖资源事件。
        /// </summary>
        public event EventHandler<ReadDataDependencyAssetEventArgs> ReadDataDependencyAsset
        {
            add => _readDataDependencyAssetEventHandler += value;
            remove => _readDataDependencyAssetEventHandler -= value;
        }

        /// <summary>
        /// 确保二进制流缓存分配足够大小的内存并缓存。
        /// </summary>
        /// <param name="ensureSize">要确保二进制流缓存分配内存的大小。</param>
        public static void EnsureCachedBytesSize(int ensureSize)
        {
            if (ensureSize < 0)
            {
                throw new GameFrameworkException("Ensure size is invalid.");
            }

            if (cachedBytes == null || cachedBytes.Length < ensureSize)
            {
                FreeCachedBytes();
                int size = (ensureSize - 1 + BlockSize) / BlockSize * BlockSize;
                cachedBytes = new byte[size];
            }
        }

        /// <summary>
        /// 释放缓存的二进制流。
        /// </summary>
        public static void FreeCachedBytes()
        {
            cachedBytes = null;
        }

        /// <summary>
        /// 读取数据。
        /// </summary>
        /// <param name="dataAssetName">内容资源名称。</param>
        public void ReadData(string dataAssetName)
        {
            ReadData(dataAssetName, null);
        }

        /// <summary>
        /// 读取数据。
        /// </summary>
        /// <param name="dataAssetName">内容资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void ReadData(string dataAssetName, object userData)
        {
            if (_dataProviderHelper == null)
            {
                throw new GameFrameworkException("You must set data provider helper first.");
            }

            IResourceManager resource = Zero.resource;
            //HasAssetResult result = resource.HasAsset(dataAssetName);
            //switch (result)
            {
                //TODO:资源框架引用待修改
                // case HasAssetResult.AssetOnDisk:
                // case HasAssetResult.AssetOnFileSystem:
                //     resource.LoadAsset(dataAssetName, priority, m_LoadAssetCallbacks, userData);
                //     break;
                //
                // case HasAssetResult.BinaryOnDisk:
                //     resource.LoadBinary(dataAssetName, m_LoadBinaryCallbacks, userData);
                //     break;
                //
                // case HasAssetResult.BinaryOnFileSystem:
                //     int dataLength = resource.GetBinaryLength(dataAssetName);
                //     EnsureCachedBytesSize(dataLength);
                //     if (dataLength != resource.LoadBinaryFromFileSystem(dataAssetName, s_CachedBytes))
                //     {
                //         throw new GameFrameworkException(Utility.Text.Format("Load binary '{0}' from file system with internal error.", dataAssetName));
                //     }
                //
                //     try
                //     {
                //         if (!m_DataProviderHelper.ReadData(m_Owner, dataAssetName, s_CachedBytes, 0, dataLength, userData))
                //         {
                //             throw new GameFrameworkException(Utility.Text.Format("Load data failure in data provider helper, data asset name '{0}'.", dataAssetName));
                //         }
                //
                //         if (m_ReadDataSuccessEventHandler != null)
                //         {
                //             ReadDataSuccessEventArgs loadDataSuccessEventArgs = ReadDataSuccessEventArgs.Create(dataAssetName, 0f, userData);
                //             m_ReadDataSuccessEventHandler(this, loadDataSuccessEventArgs);
                //             ReferencePool.Release(loadDataSuccessEventArgs);
                //         }
                //     }
                //     catch (Exception exception)
                //     {
                //         if (m_ReadDataFailureEventHandler != null)
                //         {
                //             ReadDataFailureEventArgs loadDataFailureEventArgs = ReadDataFailureEventArgs.Create(dataAssetName, exception.ToString(), userData);
                //             m_ReadDataFailureEventHandler(this, loadDataFailureEventArgs);
                //             ReferencePool.Release(loadDataFailureEventArgs);
                //             return;
                //         }
                //
                //         throw;
                //     }
                //
                //     break;

                //default:
                //    throw new GameFrameworkException(Utility.Text.Format("Data asset '{0}' is '{1}'.", dataAssetName,
                //        result));
            }
        }

        /// <summary>
        /// 解析内容。
        /// </summary>
        /// <param name="dataString">要解析的内容字符串。</param>
        /// <returns>是否解析内容成功。</returns>
        public bool ParseData(string dataString)
        {
            return ParseData(dataString, null);
        }

        /// <summary>
        /// 解析内容。
        /// </summary>
        /// <param name="dataString">要解析的内容字符串。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析内容成功。</returns>
        public bool ParseData(string dataString, object userData)
        {
            if (_dataProviderHelper == null)
            {
                throw new GameFrameworkException("You must set data helper first.");
            }

            if (dataString == null)
            {
                throw new GameFrameworkException("Data string is invalid.");
            }

            try
            {
                return _dataProviderHelper.ParseData(_owner, dataString, userData);
            }
            catch (Exception exception)
            {
                if (exception is GameFrameworkException)
                {
                    throw;
                }

                throw new GameFrameworkException(
                    Utility.Text.Format("Can not parse data string with exception '{0}'.", exception), exception);
            }
        }

        /// <summary>
        /// 解析内容。
        /// </summary>
        /// <param name="dataBytes">要解析的内容二进制流。</param>
        /// <returns>是否解析内容成功。</returns>
        public bool ParseData(byte[] dataBytes)
        {
            if (dataBytes == null)
            {
                throw new GameFrameworkException("Data bytes is invalid.");
            }

            return ParseData(dataBytes, 0, dataBytes.Length, null);
        }

        /// <summary>
        /// 解析内容。
        /// </summary>
        /// <param name="dataBytes">要解析的内容二进制流。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析内容成功。</returns>
        public bool ParseData(byte[] dataBytes, object userData)
        {
            if (dataBytes == null)
            {
                throw new GameFrameworkException("Data bytes is invalid.");
            }

            return ParseData(dataBytes, 0, dataBytes.Length, userData);
        }

        /// <summary>
        /// 解析内容。
        /// </summary>
        /// <param name="dataBytes">要解析的内容二进制流。</param>
        /// <param name="startIndex">内容二进制流的起始位置。</param>
        /// <param name="length">内容二进制流的长度。</param>
        /// <returns>是否解析内容成功。</returns>
        public bool ParseData(byte[] dataBytes, int startIndex, int length)
        {
            return ParseData(dataBytes, startIndex, length, null);
        }

        /// <summary>
        /// 解析内容。
        /// </summary>
        /// <param name="dataBytes">要解析的内容二进制流。</param>
        /// <param name="startIndex">内容二进制流的起始位置。</param>
        /// <param name="length">内容二进制流的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析内容成功。</returns>
        public bool ParseData(byte[] dataBytes, int startIndex, int length, object userData)
        {
            if (_dataProviderHelper == null)
            {
                throw new GameFrameworkException("You must set data helper first.");
            }

            if (dataBytes == null)
            {
                throw new GameFrameworkException("Data bytes is invalid.");
            }

            if (startIndex < 0 || length < 0 || startIndex + length > dataBytes.Length)
            {
                throw new GameFrameworkException("Start index or length is invalid.");
            }

            try
            {
                return _dataProviderHelper.ParseData(_owner, dataBytes, startIndex, length, userData);
            }
            catch (Exception exception)
            {
                if (exception is GameFrameworkException)
                {
                    throw;
                }

                throw new GameFrameworkException(
                    Utility.Text.Format("Can not parse data bytes with exception '{0}'.", exception), exception);
            }
        }

        /// <summary>
        /// 设置数据提供者辅助器。
        /// </summary>
        /// <param name="dataProviderHelper">数据提供者辅助器。</param>
        internal void SetDataProviderHelper(IDataProviderHelper<T> dataProviderHelper)
        {
            if (dataProviderHelper == null)
            {
                throw new GameFrameworkException("Data provider helper is invalid.");
            }

            _dataProviderHelper = dataProviderHelper;
        }

        private void LoadAssetSuccessCallback(string dataAssetName, object dataAsset, float duration, object userData)
        {
            try
            {
                if (!_dataProviderHelper.ReadData(_owner, dataAssetName, dataAsset, userData))
                {
                    throw new GameFrameworkException(Utility.Text.Format(
                        "Load data failure in data provider helper, data asset name '{0}'.", dataAssetName));
                }

                if (_readDataSuccessEventHandler != null)
                {
                    ReadDataSuccessEventArgs loadDataSuccessEventArgs =
                        ReadDataSuccessEventArgs.Create(dataAssetName, duration, userData);
                    _readDataSuccessEventHandler(this, loadDataSuccessEventArgs);
                    ReferencePool.Release(loadDataSuccessEventArgs);
                }
            }
            catch (Exception exception)
            {
                if (_readDataFailureEventHandler != null)
                {
                    ReadDataFailureEventArgs loadDataFailureEventArgs =
                        ReadDataFailureEventArgs.Create(dataAssetName, exception.ToString(), userData);
                    _readDataFailureEventHandler(this, loadDataFailureEventArgs);
                    ReferencePool.Release(loadDataFailureEventArgs);
                    return;
                }

                throw;
            }
            finally
            {
                _dataProviderHelper.ReleaseDataAsset(_owner, dataAsset);
            }
        }

        //private void LoadAssetOrBinaryFailureCallback(string dataAssetName, LoadResourceStatus status,
        //    string errorMessage, object userData)
        //{
        //    string appendErrorMessage =
        //        Utility.Text.Format("Load data failure, data asset name '{0}', status '{1}', error message '{2}'.",
        //            dataAssetName, status, errorMessage);
        //    if (_readDataFailureEventHandler != null)
        //    {
        //        ReadDataFailureEventArgs loadDataFailureEventArgs =
        //            ReadDataFailureEventArgs.Create(dataAssetName, appendErrorMessage, userData);
        //        _readDataFailureEventHandler(this, loadDataFailureEventArgs);
        //        ReferencePool.Release(loadDataFailureEventArgs);
        //        return;
        //    }

        //    throw new GameFrameworkException(appendErrorMessage);
        //}

        private void LoadAssetUpdateCallback(string dataAssetName, float progress, object userData)
        {
            if (_readDataUpdateEventHandler != null)
            {
                ReadDataUpdateEventArgs loadDataUpdateEventArgs =
                    ReadDataUpdateEventArgs.Create(dataAssetName, progress, userData);
                _readDataUpdateEventHandler(this, loadDataUpdateEventArgs);
                ReferencePool.Release(loadDataUpdateEventArgs);
            }
        }

        private void LoadAssetDependencyAssetCallback(string dataAssetName, string dependencyAssetName, int loadedCount,
            int totalCount, object userData)
        {
            if (_readDataDependencyAssetEventHandler != null)
            {
                ReadDataDependencyAssetEventArgs loadDataDependencyAssetEventArgs =
                    ReadDataDependencyAssetEventArgs.Create(dataAssetName, dependencyAssetName, loadedCount, totalCount,
                        userData);
                _readDataDependencyAssetEventHandler(this, loadDataDependencyAssetEventArgs);
                ReferencePool.Release(loadDataDependencyAssetEventArgs);
            }
        }

        private void LoadBinarySuccessCallback(string dataAssetName, byte[] dataBytes, float duration, object userData)
        {
            try
            {
                if (!_dataProviderHelper.ReadData(_owner, dataAssetName, dataBytes, 0, dataBytes.Length, userData))
                {
                    throw new GameFrameworkException(Utility.Text.Format(
                        "Load data failure in data provider helper, data asset name '{0}'.", dataAssetName));
                }

                if (_readDataSuccessEventHandler != null)
                {
                    ReadDataSuccessEventArgs loadDataSuccessEventArgs =
                        ReadDataSuccessEventArgs.Create(dataAssetName, duration, userData);
                    _readDataSuccessEventHandler(this, loadDataSuccessEventArgs);
                    ReferencePool.Release(loadDataSuccessEventArgs);
                }
            }
            catch (Exception exception)
            {
                if (_readDataFailureEventHandler != null)
                {
                    ReadDataFailureEventArgs loadDataFailureEventArgs =
                        ReadDataFailureEventArgs.Create(dataAssetName, exception.ToString(), userData);
                    _readDataFailureEventHandler(this, loadDataFailureEventArgs);
                    ReferencePool.Release(loadDataFailureEventArgs);
                    return;
                }

                throw;
            }
        }
    }
}