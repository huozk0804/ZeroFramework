/*
    URL: https://github.com/Misaka-Mikoto-Tech/UIControlBinding
    使用方法:
    UE: 将此脚本添加到UI根节点，与程序协商好需要绑定的控件及其变量名后，将需要绑定的控件拖到脚本上
    程序: 点此脚本右上角的齿轮，点 "复制代码到剪贴板" 按钮

    UIManager 加载示例：
    `` C#
        IBindableUI uiA = Activator.CreateInstance(Type.GetType("UIA")) as IBindableUI;
        GameObject prefab = Resources.Load<GameObject>("UI/UIA"); // you can get ui config from config file
        GameObject go = Instantiate(prefab);
        UIControlData ctrlData = go.GetComponent<UIControlData>();
        if(ctrlData != null)
        {
            ctrlData.BindDataTo(uiA);
        }
    ``
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Profiling;
using UnityEngine.Video;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ZeroFramework.UI
{
	/// <summary>
	/// 单个控件数据
	/// </summary>
	[Serializable]
	public class CtrlItemData
	{
		public string name = string.Empty;
#if UNITY_EDITOR
		[HideInInspector] public string type = string.Empty;
#endif
		public UnityEngine.Object[] targets = new UnityEngine.Object[1];

		public override string ToString () {
			return name;
		}
	}

	/// <summary>
	/// 当前UI所有的绑定数据以及子UI指定
	/// </summary>
	[DisallowMultipleComponent]
	public class UIControlData : MonoBehaviour
	{
		/// <summary>
		/// 所有绑定的组件，不允许重名
		/// </summary>
		public List<CtrlItemData> ctrlItemData;

#if UNITY_EDITOR
		/// <summary>
		/// 已知类型列表，自定义类型可以添加到下面指定区域
		/// </summary>
		private static readonly Dictionary<string, Type> _DefaultTypeMap = new Dictionary<string, Type>()
		{
			{ "Text", typeof(Text) },
			{ "RawImage", typeof(RawImage) },
			{ "Button", typeof(Button) },
			{ "Toggle", typeof(Toggle) },
			{ "Slider", typeof(Slider) },
			{ "Scrollbar", typeof(Scrollbar) },
			{ "Dropdown", typeof(Dropdown) },
			{ "InputField", typeof(InputField) },
			{ "Canvas", typeof(Canvas) },
			{ "ScrollRect", typeof(ScrollRect) },
			{ "SpriteRenderer", typeof(SpriteRenderer) },
			{ "GridLayoutGroup", typeof(GridLayoutGroup) },
			{ "Animation", typeof(Animation) },
			{ "VideoPlayer", typeof(VideoPlayer) },
			{ "CanvasGroup", typeof(CanvasGroup) },
			{ "Image", typeof(Image) },
			{ "RectTransform", typeof(RectTransform) },
			{ "Transform", typeof(Transform) },
			{ "GameObject", typeof(GameObject) },
			{ "UIControlData", typeof(UIControlData) },
		};

		public static string[] GetAllTypeNames () {
			string[] keys = new string[_DefaultTypeMap.Count + 1];
			keys[0] = "自动";
			_DefaultTypeMap.Keys.CopyTo(keys, 1);
			return keys;
		}

		public static Type[] GetAllTypes () {
			Type[] types = new Type[_DefaultTypeMap.Count + 1];
			types[0] = typeof(UnityEngine.Object);
			_DefaultTypeMap.Values.CopyTo(types, 1);
			return types;
		}
#endif

		#region Get

		/// <summary>
		/// 找到指定名称的第一个组件, 不存在返回 null
		/// </summary>
		/// <typeparam name="T">预期类型</typeparam>
		/// <param name="comName">组件名称</param>
		/// <returns>组件实例</returns>
		public T GetComponent<T> (string comName) where T : Component {
			int idx = GetCtrlIndex(comName);
			if (idx == -1)
				return null;

			var targets = ctrlItemData[idx].targets;
			if (targets.Length == 0)
				return null;

			return targets[0] as T;
		}

		/// <summary>
		/// 找到指定名称的第一个组件, 不存在返回 null
		/// </summary>
		/// <param name="comName">组件名称</param>
		/// <returns>组件实例</returns>
		public new UnityEngine.Object GetComponent (string comName) {
			int idx = GetCtrlIndex(comName);
			if (idx == -1)
				return null;

			var targets = ctrlItemData[idx].targets;
			if (targets.Length == 0)
				return null;

			return targets[0];
		}

		/// <summary>
		/// 找到指定名称的第一个组件, 不存在返回 null
		/// </summary>
		/// <param name="idx">索引</param>
		/// <returns>组件实例</returns>
		public UnityEngine.Object GetComponent (int idx) {
			if (idx == -1 || idx >= ctrlItemData.Count)
				return null;

			var targets = ctrlItemData[idx].targets;
			if (targets.Length == 0)
				return null;

			return targets[0];
		}

		/// <summary>
		/// 获取名称一致的组件索引
		/// </summary>
		/// <param name="comName">组件名称</param>
		/// <returns></returns>
		private int GetCtrlIndex (string comName) {
			for (int i = 0, imax = ctrlItemData.Count; i < imax; i++) {
				CtrlItemData item = ctrlItemData[i];
				if (item.name == comName)
					return i;
			}

			return -1;
		}

		#endregion

		#region For Editor

#if UNITY_EDITOR

		public bool dataHasChanged = false;

		public bool CorrectComponents () {
			bool isOk = true;
			for (int i = 0, imax = ctrlItemData.Count; i < imax; i++) {
				if (ctrlItemData[i].name.IsNullOrEmpty()) {
					Log.Error($"{gameObject}, [{gameObject.name}]第 {i + 1} 个控件没有名字，请修正");
					return false;
				}

				for (int j = ctrlItemData.Count - 1; j >= 0; j--) {
					if (ctrlItemData[i].name == ctrlItemData[j].name && i != j) {
						Log.Error(
							$"{gameObject} [{gameObject.name}控件名字 [{ctrlItemData[i].name}] 第 {i + 1} 项与第 {j + 1} 项重复，请修正]");
						return false;
					}
				}
			}

			isOk = ReplaceTargetsToUIComponent();
			if (isOk)
				Log.Info($"{gameObject} [{gameObject.name}]控件绑定修正完毕");

			return isOk;
		}

		/// <summary>
		/// 由于自动拖上去的对象永远都是 GameObject，所以我们需要把它修正为正确的对象类型
		/// </summary>
		private bool ReplaceTargetsToUIComponent () {
			for (int i = 0, imax = ctrlItemData.Count; i < imax; i++) {
				var objs = ctrlItemData[i].targets;
				Type type = null;
				for (int j = 0, jmax = objs.Length; j < jmax; j++) {
					if (objs[j] == null) {
						Log.Error($"{gameObject} [{gameObject.name}]控件名字 [{ctrlItemData[i].name}] 第 {j + 1} 项为空，请修正");
						return false;
					}

					GameObject go = objs[j] as GameObject;
					if (go == null)
						go = (objs[j] as Component)?.gameObject;

					// 必须拖当前 Prefab 下的控件
					if (!IsInCurrentPrefab(go.transform)) {
						Log.Error(
							$"{gameObject} [{gameObject.name}]控件名字 [{ctrlItemData[i].name}] 第 {j + 1} 项不是当前 Prefab 下的控件，请修正");
						return false;
					}

					UnityEngine.Object correctComponent = FindCorrectComponent(go, ctrlItemData[i].type);
					if (correctComponent == null) {
						Log.Error(
							$"{gameObject} [{gameObject.name}]控件 [{ctrlItemData[i].name}] 第 {j + 1} 项不是 {ctrlItemData[i].type} 类型，请修正");
						return false;
					}

					if (type == null) // 当前变量的第一个控件时执行
					{
						if (string.IsNullOrEmpty(ctrlItemData[i].type)) {
							type = correctComponent.GetType();
						} else {
							if (!_DefaultTypeMap.TryGetValue(ctrlItemData[i].type, out type)) {
								Log.Error("Internal Error, pls contact author");
								return false;
							}
						}
					} else if (correctComponent.GetType() != type && !correctComponent.GetType().IsSubclassOf(type)) {
						Log.Error(
							$"{gameObject} [{gameObject.name}]控件名字 [{ctrlItemData[i].name}] 第 {j + 1} 项与第 1 项的类型不同，请修正");
						return false;
					}

					if (objs[j] != correctComponent)
						dataHasChanged = true;

					objs[j] = correctComponent;
				}

				if (type.Name != ctrlItemData[i].type) {
					ctrlItemData[i].type = type.Name;
#if UNITY_2019_1_OR_NEWER
					EditorUtility.ClearDirty(this);
#endif
					EditorUtility.SetDirty(this);
					PrefabUtility.RecordPrefabInstancePropertyModifications(this);
				}

				ctrlItemData[i].type = type.Name;
			}

			return true;
		}

		private bool IsInCurrentPrefab (Transform t) {
			do {
				if (t == transform)
					return true;
				t = t.parent;
			} while (t != null);

			return false;
		}

		private UnityEngine.Object FindCorrectComponent (GameObject go, string typename) {
			if (typename == "GameObject")
				return go;

			List<Component> components = new List<Component>();
			go.GetComponents(components);

			Component GetSpecialTypeComp (Type t) {
				foreach (var comp in components) {
					Type compType = comp.GetType();
					if (compType == t || compType.IsSubclassOf(t)) {
						return comp;
					}
				}

				return null;
			}

			Component newComp = null;
			if (string.IsNullOrEmpty(typename)) {
				// 类型名为空则为自动类型，在 _typeMap 里从上往下找
				foreach (var kv in _DefaultTypeMap) {
					newComp = GetSpecialTypeComp(kv.Value);
					if (newComp != null)
						break;
				}
			} else {
				// 指定了类型名则只找指定类型的控件
				if (_DefaultTypeMap.TryGetValue(typename, out var type)) {
					newComp = GetSpecialTypeComp(type);
				}
			}

			return newComp;
		}

		private bool IsNeedSave () {
			foreach (var ctrl in ctrlItemData) {
				if (string.IsNullOrEmpty(ctrl.type))
					return true;
			}

			return false;
		}

#endif
		#endregion

		#region Binding Tool

#if UNITY_EDITOR

		public void SavePrefabe () {

		}

		public void GenerateCodeTemplate (string path) {

		}

		[ContextMenu("复制代码变量声明到剪贴板")]
		public void CopyCodeDefineToClipBoardPrivate () {
			if (IsNeedSave())
				UIBindingPrefabSaveHelper.SavePrefab(gameObject);

			StringBuilder sb = new StringBuilder(1024);
			sb.AppendLine("#region 控件绑定变量声明，自动生成请勿手改");

			foreach (var ctrl in ctrlItemData) {
				if (ctrl.targets.Length == 0)
					continue;

				if (ctrl.targets.Length == 1)
					sb.AppendFormat($"\t\t[ControlBinding]\r\n\t\tprivate {ctrl.type} {ctrl.name};\r\n");
				else
					sb.AppendFormat($"\t\t[ControlBinding]\r\n\t\tprivate {ctrl.type}[] {ctrl.name};\r\n");
			}
			sb.Append("#endregion\r\n\r\n");

			GUIUtility.systemCopyBuffer = sb.ToString();
		}

		[ContextMenu("复制代码变量赋值到剪贴板")]
		public void CopyCodeFindToClipBoard () {
			StringBuilder sb = new StringBuilder(1024);
			sb.AppendLine("#region 控件绑定变量赋值，自动生成请勿手改");
			sb.AppendFormat($"\t\tUIControlData uiData = GetComponent<UIControlData>();\r\n");

			foreach (var ctrl in ctrlItemData) {
				if (ctrl.targets.Length == 0)
					continue;

				if (ctrl.targets.Length == 1)
					sb.AppendFormat($"\t\t{ctrl.name} = uiData.GetComponent<{ctrl.type}>();\r\n");
				else
					sb.AppendFormat($"\t\t{ctrl.type}[] {ctrl.name};\r\n");
			}

			sb.Append("#endregion\r\n\r\n");

			GUIUtility.systemCopyBuffer = sb.ToString();
		}
#endif

		#endregion
	}
}