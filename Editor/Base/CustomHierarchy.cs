#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using UnityEngine.AI;
using UnityEngine.UI;

namespace ZeroFramework.Editor
{

	[InitializeOnLoad]
	public class CustomHierarchy
	{
		// 总的开关用于开启或关闭 显示图标
		public static bool EnableCustomHierarchy = true;

		static CustomHierarchy () {
			EditorApplication.hierarchyWindowItemOnGUI += HierarchWindowOnGui;
		}

		// 绘制Rect
		private static Rect CreateRect (Rect selectionRect, int index) {
			var rect = new Rect(selectionRect);
			rect.x += rect.width - 20 - (20 * index);
			rect.width = 18;
			return rect;
		}

		// 绘制图标
		private static void DrawIcon<T> (Rect rect) {
			// 获得Unity内置的图标
			var icon = EditorGUIUtility.ObjectContent(null, typeof(T)).image;
			GUI.Label(rect, icon);
		}

		// 综合以上，根据类型，绘制图标和文字
		private static void DrawRectIcon<T> (Rect selectionRect, GameObject go, ref int order) where T : Component {
			//if (go.GetComponent<T>())
			if (go.HasComponent<T>(false)) // 使用扩展方法HasComponent
			{
				// 图标的绘制排序
				order += 1;
				var rect = CreateRect(selectionRect, order);

				// 绘制图标
				DrawIcon<T>(rect);
			}
		}

		// 绘制Hierarchy
		static void HierarchWindowOnGui (int instanceId, Rect selectionRect) {
			if (!EnableCustomHierarchy)
				return;
			try {
				// CheckBox // -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --
				var rectCheck = new Rect(selectionRect);
				rectCheck.x += rectCheck.width - 20;
				rectCheck.width = 18;

				// 通过ID获得Obj
				var obj = EditorUtility.InstanceIDToObject(instanceId);
				var go = (GameObject)obj;// as GameObject;
										 // 绘制Checkbox 
				EditorGUI.BeginChangeCheck();
				var result = GUI.Toggle(rectCheck, go.activeSelf, string.Empty);
				if (result != go.activeSelf) {
					go.SetActive(result);
				}
				if (EditorGUI.EndChangeCheck()) {
					EditorUtility.SetDirty(obj);
				}
				// -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- 
				// 图标的序列号
				var index = 0;

				// is Static 
				if (go.isStatic) {
					index += 1;
					var rectIcon = CreateRect(selectionRect, index);
					GUI.Label(rectIcon, "S");
				}

				// Draw //  -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- 
				// 可以在此修改，根据需要删减需要绘制的内容
				// Renderer
				DrawRectIcon<MeshRenderer>(selectionRect, go, ref index);
				DrawRectIcon<SkinnedMeshRenderer>(selectionRect, go, ref index);
				// Colliders
				DrawRectIcon<BoxCollider>(selectionRect, go, ref index);
				DrawRectIcon<SphereCollider>(selectionRect, go, ref index);
				DrawRectIcon<CapsuleCollider>(selectionRect, go, ref index);
				DrawRectIcon<MeshCollider>(selectionRect, go, ref index);
				DrawRectIcon<CharacterController>(selectionRect, go, ref index);
				// RigidBody
				DrawRectIcon<Rigidbody>(selectionRect, go, ref index);
				// Lights
				DrawRectIcon<Light>(selectionRect, go, ref index);
				// Joints

				// Animation / Animator
				DrawRectIcon<Animator>(selectionRect, go, ref index);
				DrawRectIcon<Animation>(selectionRect, go, ref index);
				// Camera / Projector
				DrawRectIcon<Camera>(selectionRect, go, ref index);
				DrawRectIcon<Projector>(selectionRect, go, ref index);
				// NavAgent
				DrawRectIcon<NavMeshAgent>(selectionRect, go, ref index);
				DrawRectIcon<NavMeshObstacle>(selectionRect, go, ref index);
				// Network
				// Particle
				DrawRectIcon<ParticleSystem>(selectionRect, go, ref index);
				// Audio
				DrawRectIcon<AudioSource>(selectionRect, go, ref index);

				DrawRectIcon<Image>(selectionRect, go, ref index);
				DrawRectIcon<RawImage>(selectionRect, go, ref index);
				DrawRectIcon<Text>(selectionRect, go, ref index);
			} catch (Exception) {
			}
		}
	}

	public static class ExtensionMethods
	{
		/// <summary>
		/// 检测是否含有组件
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="go"></param>
		/// <param name="checkChildren">是否检测子层级</param>
		/// <returns></returns>
		public static bool HasComponent<T> (this GameObject go, bool checkChildren) where T : Component {
			if (!checkChildren) {
				return go.GetComponent<T>();
			} else {
				return go.GetComponentsInChildren<T>().FirstOrDefault() != null;
			}
		}
	}
}
#endif