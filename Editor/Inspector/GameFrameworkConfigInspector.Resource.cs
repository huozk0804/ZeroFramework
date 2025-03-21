using UnityEditor;

namespace ZeroFramework.Editor
{
	public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
	{
		private SerializedProperty _defaultPackageName;
		private SerializedProperty _resourcePlayMode;
		private SerializedProperty _fileVerifyLevel;
		private SerializedProperty _milliseconds;
		private SerializedProperty _downloadingMaxNum;
		private SerializedProperty _failedDownloadTryAgainNum;
		private SerializedProperty _assetAutoReleaseInterval;
		private SerializedProperty _assetPoolCapacity;
		private SerializedProperty _assetPoolExpireTime;
		private SerializedProperty _assetPoolPriority;

		[InspectorConfigInit]
		void ResourceInspectorInit () {
			_enableFunc.AddLast(OnResourceEnable);
			_inspectorFunc.AddLast(OnResourceInspectorGUI);
			_completeFunc.AddLast(OnResourceComplete);
		}

		void OnResourceInspectorGUI () {
			EditorGUILayout.LabelField("Resource(YooAsset) Module", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(_defaultPackageName);
			EditorGUILayout.PropertyField(_resourcePlayMode);
			EditorGUILayout.PropertyField(_fileVerifyLevel);
			EditorGUILayout.PropertyField(_milliseconds);
			EditorGUILayout.PropertyField(_downloadingMaxNum);
			EditorGUILayout.PropertyField(_failedDownloadTryAgainNum);
			EditorGUILayout.PropertyField(_assetAutoReleaseInterval);
			EditorGUILayout.PropertyField(_assetPoolCapacity);
			EditorGUILayout.PropertyField(_assetPoolExpireTime);
			EditorGUILayout.PropertyField(_assetPoolPriority);
			serializedObject.ApplyModifiedProperties();
			Repaint();
		}

		void OnResourceEnable () {
			_defaultPackageName = serializedObject.FindProperty("defaultPackageName");
			_resourcePlayMode = serializedObject.FindProperty("resourcePlayMode");
			_fileVerifyLevel = serializedObject.FindProperty("fileVerifyLevel");
			_milliseconds = serializedObject.FindProperty("milliseconds");
			_downloadingMaxNum = serializedObject.FindProperty("downloadingMaxNum");
			_failedDownloadTryAgainNum = serializedObject.FindProperty("failedDownloadTryAgainNum");
			_assetAutoReleaseInterval = serializedObject.FindProperty("assetAutoReleaseInterval");
			_assetPoolCapacity = serializedObject.FindProperty("assetPoolCapacity");
			_assetPoolExpireTime = serializedObject.FindProperty("assetPoolExpireTime");
			_assetPoolPriority = serializedObject.FindProperty("assetPoolPriority");
		}

		void OnResourceComplete () {
		}
	}
}