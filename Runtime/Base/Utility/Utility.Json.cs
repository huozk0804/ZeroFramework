//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;

namespace ZeroFramework
{
    public static partial class Utility
    {
        /// <summary>
        /// JSON 相关的实用函数。
        /// </summary>
        public static partial class Json
        {
            private static IJsonHelper JsonHelper = null;

            /// <summary>
            /// 设置 JSON 辅助器。
            /// </summary>
            /// <param name="jsonHelper">要设置的 JSON 辅助器。</param>
            public static void SetJsonHelper(IJsonHelper jsonHelper)
            {
                JsonHelper = jsonHelper;
            }

            /// <summary>
            /// 将对象序列化为 JSON 字符串。
            /// </summary>
            /// <param name="obj">要序列化的对象。</param>
            /// <returns>序列化后的 JSON 字符串。</returns>
            public static string ToJson(object obj)
            {
                if (JsonHelper == null)
                {
                    throw new GameFrameworkException("JSON helper is invalid.");
                }

                try
                {
                    return JsonHelper.ToJson(obj);
                }
                catch (Exception exception)
                {
                    if (exception is GameFrameworkException)
                    {
                        throw;
                    }

                    throw new GameFrameworkException(Text.Format("Can not convert to JSON with exception '{0}'.", exception), exception);
                }
            }

            /// <summary>
            /// 将 JSON 字符串反序列化为对象。
            /// </summary>
            /// <typeparam name="T">对象类型。</typeparam>
            /// <param name="json">要反序列化的 JSON 字符串。</param>
            /// <returns>反序列化后的对象。</returns>
            public static T ToObject<T>(string json)
            {
                if (JsonHelper == null)
                {
                    throw new GameFrameworkException("JSON helper is invalid.");
                }

                try
                {
                    return JsonHelper.ToObject<T>(json);
                }
                catch (Exception exception)
                {
                    if (exception is GameFrameworkException)
                    {
                        throw;
                    }

                    throw new GameFrameworkException(Text.Format("Can not convert to object with exception '{0}'.", exception), exception);
                }
            }

            /// <summary>
            /// 将 JSON 字符串反序列化为对象。
            /// </summary>
            /// <param name="objectType">对象类型。</param>
            /// <param name="json">要反序列化的 JSON 字符串。</param>
            /// <returns>反序列化后的对象。</returns>
            public static object ToObject(Type objectType, string json)
            {
                if (JsonHelper == null)
                {
                    throw new GameFrameworkException("JSON helper is invalid.");
                }

                if (objectType == null)
                {
                    throw new GameFrameworkException("Object type is invalid.");
                }

                try
                {
                    return JsonHelper.ToObject(objectType, json);
                }
                catch (Exception exception)
                {
                    if (exception is GameFrameworkException)
                    {
                        throw;
                    }

                    throw new GameFrameworkException(Text.Format("Can not convert to object with exception '{0}'.", exception), exception);
                }
            }
        }
    }
}
