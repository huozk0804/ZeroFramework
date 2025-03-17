//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;
using System.Reflection;

namespace ZeroFramework
{
	public static partial class Utility
	{
		public static class GameObject
		{
			/// <summary>
			/// 比较并更新目标对象的差异属性(利用反射注意性能问题)
			/// </summary>
			/// <typeparam name="T">对象类型</typeparam>
			/// <param name="source">数据源</param>
			/// <param name="target">待更新对象</param>
			public static void UpdateIfDifferent<T> (T source, T target) {
				if (source == null || target == null)
					return;

				Type type = typeof(T);
				PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

				/// <summary>
				/// 判断两个值是否相等（支持null和值类型）
				/// </summary>
				bool AreEqual (object a, object b) {
					if (a == null && b == null)
						return true;
					if (a == null || b == null)
						return false;
					return a.Equals(b);
				}

				/// <summary>
				/// 处理类型转换（如可空类型、泛型等）
				/// </summary>
				object ConvertType (object value, Type targetType) {
					if (value == null)
						return null;

					// 处理可空类型（如int?）
					Type underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;
					return Convert.ChangeType(value, underlyingType);
				}

				foreach (PropertyInfo prop in properties) {
					// 跳过只读/不可写属性
					if (!prop.CanWrite)
						continue;

					object sourceValue = prop.GetValue(source);
					object targetValue = prop.GetValue(target);

					// 差异判断逻辑
					if (!AreEqual(sourceValue, targetValue)) {
						// 类型兼容性处理
						object convertedValue = ConvertType(sourceValue, prop.PropertyType);
						prop.SetValue(target, convertedValue);
					}
				}
			}
		}
	}
}