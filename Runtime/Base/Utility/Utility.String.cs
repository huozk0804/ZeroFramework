//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using System;
using UnityEngine;
using System.Collections;

namespace ZeroFramework
{
	public static partial class Utility
	{
		/// <summary>
		/// 字符串类型相关的实用函数。
		/// </summary>
		public static partial class String
		{
			private const int UPPER_2_LOWER = 'a' - 'A';
			private const string INVALID_CHAR_SET = ",<.>/?;:'\"[{]}\\|`~!@#$%^&*()-=+ \r\n\t";
			private static StringBuilder _CacheStringBuilder = new StringBuilder();

			public static char ToLower (char c) {
				if (c <= 'Z' && c >= 'A')
					return (char)(c + UPPER_2_LOWER);
				else
					return c;
			}

			public static char ToUpper (char c) {
				if (c <= 'z' && c >= 'a')
					return (char)(c - UPPER_2_LOWER);
				else
					return c;
			}

			// 根据大小写分割单词
			public static void SplitKeyWords (StringBuilder sb) {
				for (int i = sb.Length - 1; i > 0; i--) {
					if (sb[i] >= 'A' && sb[i] <= 'Z' && sb[i - 1] >= 'a' && sb[i - 1] <= 'z') {
						sb.Insert(i, ' ');
					}
				}
			}

			public static void ToSBC (StringBuilder sb) {
				for (int i = 0; i < sb.Length; i++) {
					if (INVALID_CHAR_SET.IndexOf(sb[i]) > -1) {
						if (32 == sb[i]) {
							sb[i] = (char)12288;
						} else if (sb[i] < 127) {
							sb[i] = (char)(sb[i] + 65248);
						}
					}
				}
			}

			public static bool ContainInvalidChar (string text) {
				char[] c = text.ToCharArray();
				for (int i = 0; i < c.Length; i++) {
					if (INVALID_CHAR_SET.IndexOf(c[i]) > -1) {
						return true;
					}
				}

				return false;
			}

			public static int LengthOfUTF8 (string str) {
				int length = 0;
				char[] characters = str.ToCharArray();
				foreach (char c in characters) {
					int cInt = (int)c;
					if (cInt < 256) {
						length++;
					} else {
						length += 2;
					}
				}
				return length;
			}

			public static int GetCharIndexIgnoreCase (string str, char lowerChar) {
				if (str == null)
					return -1;
				int delta = 'a' - 'A';
				for (int i = str.Length - 1; i >= 0; i--) {
					char v = str[i];
					if (v >= 'A' && v <= 'Z')
						v = (char)(v + delta);
					if (v == lowerChar)
						return i;
				}
				return -1;
			}

			public static int GetCharIndex (string str, char w) {
				if (str == null)
					return -1;
				for (int i = str.Length - 1; i >= 0; i--) {
					char v = str[i];
					if (v == w)
						return i;
				}
				return -1;
			}

			public static string ToMD5 (string str) {
				if (string.IsNullOrEmpty(str)) {
					return null;
				}

				_CacheStringBuilder.Clear();
				_CacheStringBuilder.Append(str);
				string ret = null;
				try {
					MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
					byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
					_CacheStringBuilder.Remove(0, _CacheStringBuilder.Length);
					_CacheStringBuilder.Append(BitConverter.ToString(hashBytes));
					int delta = 'A' - 'a';
					for (int i = _CacheStringBuilder.Length - 1; i >= 0; i--) {
						char c = _CacheStringBuilder[i];
						if (c == '-')
							_CacheStringBuilder.Remove(i, 1);
						else if (c >= 'a' && c <= 'a')
							_CacheStringBuilder[i] = (char)(c + delta);
					}
					ret = _CacheStringBuilder.ToString();
				} catch (Exception e) {
					throw new GameFrameworkException(e.ToString());
				}

				return ret;
			}

			public static string Hash (string str, int minsize = 6) {
				string value = ToHash(str).ToString("x");
				int len = value.Length;
				if (len < minsize) {
					_CacheStringBuilder.Clear();
					for (int i = minsize - len; i > 0; i--) {
						_CacheStringBuilder.Append('0');
					}
					_CacheStringBuilder.Append(value);
				}
				return value;
			}

			public static int ToHash (string str) {
				int hash = 0;
				int len = str == null ? 0 : str.Length;
				for (int i = 0; i < len; i++) {
					hash = hash * 31 + str[i];
				}
				return hash;
			}

			public static int ToHash (char[] sequence, int offset, int len) {
				int hash = 0;
				int end = offset + len;
				for (int i = offset; i < end; i++) {
					hash = hash * 31 + sequence[i];
				}
				return hash;
			}

			public static int IgnoreCaseToHash (string str) {
				int hash = 0;
				int len = str == null ? 0 : str.Length;
				for (int i = 0; i < len; i++) {
					hash = hash * 31 + GetCharIgnoreCase(str, i);
				}
				return hash;
			}

			public static int IgnoreCaseToHash (char[] sequence, int offset, int len) {
				int hash = 0;
				int end = offset + len;
				for (int i = offset; i < end; i++) {
					hash = hash * 31 + ToLower(sequence[i]);
				}
				return hash;
			}

			public static char GetCharIgnoreCase (string str, int index) {
				char c = str[index];
				if (c >= 'A' && c <= 'Z')
					return (char)(c + UPPER_2_LOWER);
				else
					return c;
			}

			public static bool StartWithIgnoreCase (string str, string pattern) {
				int len = pattern.Length;
				if (str.Length < len)
					return false;
				for (int i = 0; i < len; i++) {
					if (GetCharIgnoreCase(str, i) != GetCharIgnoreCase(pattern, i))
						return false;
				}
				return true;
			}

			public static bool EqualIgnoreCase (string a, string b) {
				int la = a == null ? 0 : a.Length;
				int lb = b == null ? 0 : b.Length;
				if (la != lb)
					return false;

				for (int i = 0; i < la; i++) {
					if (ToLower(a[i]) != ToLower(b[i]))
						return false;
				}

				return true;
			}

			public static string Concat (params object[] strs) {
				if (strs == null || strs.Length == 0)
					return "";

				_CacheStringBuilder.Clear();
				int len = strs == null ? 0 : strs.Length;
				for (int i = 0; i < len; i++) {
					_CacheStringBuilder.Append(strs[i]);
				}

				return _CacheStringBuilder.ToString();
			}

			public static string Gather<T> (IList<T> lst, int count = -1, string seperator = ",") {
				if (lst == null)
					return "";
				if (count < 0)
					count = lst.Count;

				_CacheStringBuilder.Clear();
				for (int i = 0; i < count; i++) {
					if (i > 0)
						_CacheStringBuilder.Append(seperator);
					_CacheStringBuilder.Append(lst[i]);
				}

				return _CacheStringBuilder.ToString();
			}

			public static string Gather (IEnumerator iter, string seperator = ",") {
				if (iter == null)
					return "";

				_CacheStringBuilder.Clear();
				if (string.IsNullOrEmpty(seperator)) {
					while (iter.MoveNext()) {
						_CacheStringBuilder.Append(iter.Current);
					}
				} else {
					bool first = true;
					while (iter.MoveNext()) {
						if (first)
							first = false;
						else
							_CacheStringBuilder.Append(seperator);
						_CacheStringBuilder.Append(iter.Current);
					}
				}

				return _CacheStringBuilder.ToString();
			}

			public static string ReplaceLast (string format, char oldChar, char newChar) {
				var index = format.LastIndexOf(oldChar);
				if (index >= 0) {
					_CacheStringBuilder.Clear();
					if (index > 0)
						_CacheStringBuilder.Append(format, 0, index);
					_CacheStringBuilder.Append(newChar);
					if (index < format.Length - 1)
						_CacheStringBuilder.Append(format, index + 1, format.Length - index - 1);

					return _CacheStringBuilder.ToString();
				} else {
					return format;
				}
			}

			public static string ReplaceFirst (string format, char oldChar, char newChar) {
				var index = format.IndexOf(oldChar);
				if (index >= 0) {
					_CacheStringBuilder.Clear();
					if (index > 0)
						_CacheStringBuilder.Append(format, 0, index);
					_CacheStringBuilder.Append(newChar);
					if (index < format.Length - 1)
						_CacheStringBuilder.Append(format, index + 1, format.Length - index - 1);
					return _CacheStringBuilder.ToString();
				} else {
					return format;
				}
			}

			public static string Replace (string format, params object[] matches) {
				_CacheStringBuilder.Clear();
				_CacheStringBuilder.Append(format);

				int len = matches == null ? 0 : matches.Length;
				for (int i = 0; i < len; i += 2) {
					var key = matches[i] == null ? null : matches[i].ToString();
					if (string.IsNullOrEmpty(key))
						continue;
					var v = matches[i + 1] == null ? "" : matches[i + 1].ToString();
					_CacheStringBuilder.Replace(key, v);
				}

				return _CacheStringBuilder.ToString();
			}

			public static string WrapString (string text, int length, string replaceStr) {
				int len = text.Length;
				if (len <= length)
					return text;

				int n = replaceStr.Length;
				string s = text.Substring(0, Mathf.Min(length, len) - n);
				s += replaceStr;

				return s;
			}

			public static string WrapRichText (string text, int length, string replaceStr) {
				MatchCollection mats = Regex.Matches(text, @"<quad=[a-zA-Z0-9_]+>|<size=\d+>|<color=[\w\W]+>|</color>|</size>|</?b>|</?i>");
				_CacheStringBuilder.Clear();
				int num = 0;
				int p = 0;
				Match m = mats == null && p < mats.Count ? mats[p++] : null;
				int cp = 0;
				while (num < length && cp < text.Length) {
					if (m == null || m.Index > cp) {
						_CacheStringBuilder.Append(text[cp++]);
						num++;
					} else {
						for (int i = m.Index; i < m.Length + m.Index; i++) {
							_CacheStringBuilder.Append(text[i]);
						}
						cp = m.Length + m.Index;
						m = p < mats.Count ? mats[p++] : null;
					}
				}

				if (cp < text.Length)
					_CacheStringBuilder.Append(replaceStr);

				return _CacheStringBuilder.ToString();
			}

			public static string WrapLinePerChar (string text, int charCount) {
				char l = '\n';
				_CacheStringBuilder.Clear();
				_CacheStringBuilder.Append(text);

				for (int i = charCount; i < _CacheStringBuilder.Length; i += charCount) {
					_CacheStringBuilder.Insert(i++, l);
				}

				return _CacheStringBuilder.ToString();
			}

			public static bool ParseArray (string str, ICollection<string> arr, char split = ',', char bracketl = '\0', char bracketr = '\0') {
				int p = 0;
				int offset = 0;

				_CacheStringBuilder.Clear();
				_CacheStringBuilder.Append(str);
				if (bracketl == '\0' && bracketr == '\0') {
					for (int i = 0; i < _CacheStringBuilder.Length; i++) {
						char c = _CacheStringBuilder[i];
						if (c == split) {
							arr.Add(_CacheStringBuilder.ToString(offset, i - offset));
							offset = i + 1;
						}
					}
					if (offset < _CacheStringBuilder.Length)
						arr.Add(_CacheStringBuilder.ToString(offset, _CacheStringBuilder.Length - offset));
					return true;
				}

				for (int i = 0; i < _CacheStringBuilder.Length; i++) {
					char c = _CacheStringBuilder[i];
					if (c == bracketl) {
						p++;
						if (p == 1)
							offset = i + 1;
					} else if (c == bracketr) {
						p--;
						if (p == 0) {
							if (offset < i)
								arr.Add(_CacheStringBuilder.ToString(offset, i - offset));
							offset = i + 1;
						}
					} else if (c == split && p == 1) {
						arr.Add(_CacheStringBuilder.ToString(offset, i - offset));
						offset = i + 1;
					} else if (p < 1) {
						return false;
					}
				}

				return p == 0;
			}

			public static string[][] ParseMatrix (string s, char split = ',', char bracketl = '[', char bracketr = ']') {
				if (string.IsNullOrEmpty(s))
					return new string[0][];

				List<string> tmp = new List<string>();
				string[][] matrix;
				if (!ParseArray(s, tmp, split, bracketl, bracketr))
					return null;

				matrix = new string[tmp.Count][];
				List<string> tmp2 = new List<string>();
				for (int i = 0; i < matrix.Length; i++) {
					tmp2.Clear();
					if (!ParseArray(tmp[i], tmp2, split, bracketl, bracketr))
						return null;
					var arr = new string[tmp2.Count];
					for (int j = 0; j < arr.Length; j++) {
						arr[j] = tmp2[j];
					}
					matrix[i] = arr;
				}

				return matrix;
			}

			public static int[] ParseArray (string s, char split = ',', char bracketl = '\0', char bracketr = '\0') {
				List<string> arr = new List<string>(5);
				arr.Clear();
				if (!ParseArray(s, arr, split, bracketl, bracketr)) {
					return null;
				}
				var result = new int[arr.Count];
				int num;
				for (int i = 0; i < result.Length; i++) {
					if (!int.TryParse(arr[i].Trim(), out num))
						return null;
					result[i] = num;
				}
				return result;
			}

			public static float[] ParseFloatArray (string s, char split = ',', char bracketl = '\0', char bracketr = '\0') {
				List<string> arr = new List<string>(5);
				if (!ParseArray(s, arr, split, bracketl, bracketr)) {
					return null;
				}
				var result = new float[arr.Count];
				float num;
				for (int i = 0; i < result.Length; i++) {
					if (!float.TryParse(arr[i].Trim(), out num))
						return null;
					result[i] = num;
				}
				return result;
			}

			public static string ChineseNumber (int num, bool upcase) {
				num = Mathf.Clamp(num, 0, 99999);
				string number;
				if (upcase)
					number = "零壹贰叁肆五陆柒捌玖拾佰仟萬";
				else
					number = "〇一二三四五六七八九十百千万";
				if (num == 0)
					return number[0].ToString();

				_CacheStringBuilder.Clear();
				if (num < 20) {
					int n = num / 10;
					if (n > 0)
						_CacheStringBuilder.Append(number[10]);
					n = num % 10;
					if (num < 10 || n > 0)
						_CacheStringBuilder.Append(number[n]);
				} else {
					bool zero = true;
					string unit = "";
					int uindex = 9;
					while (num > 0) {
						int n = num % 10;
						num /= 10;
						if (n > 0) {
							_CacheStringBuilder.Insert(0, unit).Insert(0, number[n]);
						} else if (num > 0 && !zero) {
							_CacheStringBuilder.Insert(0, number[0]);
						}
						if (num > 0)
							unit = number[++uindex].ToString();
						zero = n == 0;
					}
				}

				return _CacheStringBuilder.ToString();
			}


			public static string ToMask (int value, string connector, string[] names) {
				_CacheStringBuilder.Clear();

				for (int i = 0; i < Mathf.Min(32, names.Length); i++) {
					if ((value & (1 << i)) != 0) {
						if (_CacheStringBuilder.Length > 0)
							_CacheStringBuilder.Append(connector);
						_CacheStringBuilder.Append(names[i]);
					}
				}

				return _CacheStringBuilder.ToString();
			}
		}
	}
}