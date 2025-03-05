//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ZeroFramework
{
    /// <summary>
    /// 对 string 的扩展方法。
    /// </summary>
    public static class StringExtension
    {
        private static readonly char[] CachedSplitCharArray = { '.' };
        
        /// <summary>
        /// 从指定字符串中的指定位置处开始读取一行。
        /// </summary>
        /// <param name="rawString">指定的字符串。</param>
        /// <param name="position">从指定位置处开始读取一行，读取后将返回下一行开始的位置。</param>
        /// <returns>读取的一行字符串。</returns>
        public static string ReadLine(this string rawString, ref int position)
        {
            if (position < 0)
            {
                return null;
            }

            int length = rawString.Length;
            int offset = position;
            while (offset < length)
            {
                char ch = rawString[offset];
                switch (ch)
                {
                    case '\r':
                    case '\n':
                        if (offset > position)
                        {
                            string line = rawString.Substring(position, offset - position);
                            position = offset + 1;
                            if ((ch == '\r') && (position < length) && (rawString[position] == '\n'))
                            {
                                position++;
                            }

                            return line;
                        }

                        offset++;
                        position++;
                        break;

                    default:
                        offset++;
                        break;
                }
            }

            if (offset > position)
            {
                string line = rawString.Substring(position, offset - position);
                position = offset;
                return line;
            }

            return null;
        }

        public static bool IsNullOrEmpty(this string selfStr)
        {
            return string.IsNullOrEmpty(selfStr);
        }

        public static bool IsNotNullAndEmpty(this string selfStr)
        {
            return !string.IsNullOrEmpty(selfStr);
        }

        public static bool IsTrimNullOrEmpty(this string selfStr)
        {
            return selfStr == null || string.IsNullOrEmpty(selfStr.Trim());
        }

        public static bool IsTrimNotNullAndEmpty(this string selfStr)
        {
            return selfStr != null && !string.IsNullOrEmpty(selfStr.Trim());
        }
        
        public static string[] Split(this string selfStr, char splitSymbol)
        {
            CachedSplitCharArray[0] = splitSymbol;
            return selfStr.Split(CachedSplitCharArray);
        }

        public static string FillFormat(this string selfStr, params object[] args)
        {
            return string.Format(selfStr, args);
        }

        public static StringBuilder Builder(this string selfStr)
        {
            return new StringBuilder(selfStr);
        }

        public static StringBuilder AddPrefix(this StringBuilder self, string prefixString)
        {
            self.Insert(0, prefixString);
            return self;
        }

        public static int ToInt(this string selfStr, int defaultValue = 0)
        {
            return int.TryParse(selfStr, out var retValue) ? retValue : defaultValue;
        }

        public static DateTime ToDateTime(this string selfStr, DateTime defaultValue = default(DateTime))
        {
            return DateTime.TryParse(selfStr, out var retValue) ? retValue : defaultValue;
        }


        public static float ToFloat(this string selfStr, float defaultValue = 0)
        {
            return float.TryParse(selfStr, out var retValue) ? retValue : defaultValue;
        }

        public static bool HasChinese(this string input)
        {
            return Regex.IsMatch(input, @"[\u4e00-\u9fa5]");
        }

        public static bool HasSpace(this string input)
        {
            return input.Contains(" ");
        }

        public static string RemoveString(this string str, params string[] targets)
        {
            return targets.Aggregate(str, (current, t) => current.Replace(t, string.Empty));
        }

        public static string StringJoin(this IEnumerable<string> self, string separator)
        {
            return string.Join(separator, self);
        }
    }
}