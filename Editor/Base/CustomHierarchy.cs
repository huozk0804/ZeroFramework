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
		// �ܵĿ������ڿ�����ر� ��ʾͼ��
		public static bool EnableCustomHierarchy = true;

		static CustomHierarchy () {
			EditorApplication.hierarchyWindowItemOnGUI += HierarchWindowOnGui;
		}

		// ����Rect
		private static Rect CreateRect (Rect selectionRect, int index) {
			var rect = new Rect(selectionRect);
			rect.x += rect.width - 20 - (20 * index);
			rect.width = 18;
			return rect;
		}

		// ����ͼ��
		private static void DrawIcon<T> (Rect rect) {
			// ���Unity���õ�ͼ��
			var icon = EditorGUIUtility.ObjectContent(null, typeof(T)).image;
			GUI.Label(rect, icon);
		}

		// �ۺ����ϣ��������ͣ�����ͼ�������
		private static void DrawRectIcon<T> (Rect selectionRect, GameObject go, ref int order) where T : Component {
			//if (go.GetComponent<T>())
			if (go.HasComponent<T>(false)) // ʹ����չ����HasComponent
			{
				// ͼ��Ļ�������
				order += 1;
				var rect = CreateRect(selectionRect, order);

				// ����ͼ��
				DrawIcon<T>(rect);
			}
		}

		// ����Hierarchy
		static void HierarchWindowOnGui (int instanceId, Rect selectionRect) {
			if (!EnableCustomHierarchy)
				return;
			try {
				// CheckBox // -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --
				var rectCheck = new Rect(selectionRect);
				rectCheck.x += rectCheck.width - 20;
				rectCheck.width = 18;

				// ͨ��ID���Obj
				var obj = EditorUtility.InstanceIDToObject(instanceId);
				var go = (GameObject)obj;// as GameObject;
										 // ����Checkbox 
				EditorGUI.BeginChangeCheck();
				var result = GUI.Toggle(rectCheck, go.activeSelf, string.Empty);
				if (result != go.activeSelf) {
					go.SetActive(result);
				}
				if (EditorGUI.EndChangeCheck()) {
					EditorUtility.SetDirty(obj);
				}
				// -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- 
				// ͼ������к�
				var index = 0;

				// is Static 
				if (go.isStatic) {
					index += 1;
					var rectIcon = CreateRect(selectionRect, index);
					GUI.Label(rectIcon, "S");
				}

				// Draw //  -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- 
				// �����ڴ��޸ģ�������Ҫɾ����Ҫ���Ƶ�����
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
		/// ����Ƿ������
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="go"></param>
		/// <param name="checkChildren">�Ƿ����Ӳ㼶</param>
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