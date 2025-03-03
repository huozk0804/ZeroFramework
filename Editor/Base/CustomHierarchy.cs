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
		public static bool EnableCustomHierarchy = true;

		static CustomHierarchy () {
			EditorApplication.hierarchyWindowItemOnGUI += HierarchWindowOnGui;
		}

		private static Rect CreateRect (Rect selectionRect, int index) {
			var rect = new Rect(selectionRect);
			rect.x += rect.width - 20 - (20 * index);
			rect.width = 18;
			return rect;
		}

		private static void DrawIcon<T> (Rect rect) {
			var icon = EditorGUIUtility.ObjectContent(null, typeof(T)).image;
			GUI.Label(rect, icon);
		}

		private static void DrawRectIcon<T> (Rect selectionRect, GameObject go, ref int order) where T : Component {
			if (go.HasComponent<T>(false))
			{
				order += 1;
				var rect = CreateRect(selectionRect, order);
				DrawIcon<T>(rect);
			}
		}

		static void HierarchWindowOnGui (int instanceId, Rect selectionRect) {
			if (!EnableCustomHierarchy)
				return;
			try {
				var rectCheck = new Rect(selectionRect);
				rectCheck.x += rectCheck.width - 20;
				rectCheck.width = 18;

				var obj = EditorUtility.InstanceIDToObject(instanceId);
				var go = (GameObject)obj;
										 
				EditorGUI.BeginChangeCheck();
				var result = GUI.Toggle(rectCheck, go.activeSelf, string.Empty);
				if (result != go.activeSelf) {
					go.SetActive(result);
				}
				if (EditorGUI.EndChangeCheck()) {
					EditorUtility.SetDirty(obj);
				}
				
				var index = 0;

				if (go.isStatic) {
					index += 1;
					var rectIcon = CreateRect(selectionRect, index);
					GUI.Label(rectIcon, "S");
				}

				DrawRectIcon<MeshRenderer>(selectionRect, go, ref index);
				DrawRectIcon<SkinnedMeshRenderer>(selectionRect, go, ref index);
				DrawRectIcon<BoxCollider>(selectionRect, go, ref index);
				DrawRectIcon<SphereCollider>(selectionRect, go, ref index);
				DrawRectIcon<CapsuleCollider>(selectionRect, go, ref index);
				DrawRectIcon<MeshCollider>(selectionRect, go, ref index);
				DrawRectIcon<CharacterController>(selectionRect, go, ref index);
				DrawRectIcon<Rigidbody>(selectionRect, go, ref index);
				DrawRectIcon<Light>(selectionRect, go, ref index);
				DrawRectIcon<Animator>(selectionRect, go, ref index);
				DrawRectIcon<Animation>(selectionRect, go, ref index);
				DrawRectIcon<Camera>(selectionRect, go, ref index);
				DrawRectIcon<Projector>(selectionRect, go, ref index);
				DrawRectIcon<NavMeshAgent>(selectionRect, go, ref index);
				DrawRectIcon<NavMeshObstacle>(selectionRect, go, ref index);
				DrawRectIcon<ParticleSystem>(selectionRect, go, ref index);
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