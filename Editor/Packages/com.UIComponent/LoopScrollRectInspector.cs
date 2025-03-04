using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.AnimatedValues;
using ZeroFramework.UICom;

namespace ZeroFramework.Editor.Package
{
	[CustomEditor(typeof(LoopScrollRect), true)]
	public class LoopScrollRectInspector : GameFrameworkInspector
	{
		private SerializedProperty _content;
		private SerializedProperty _horizontal;
		private SerializedProperty _vertical;
		private SerializedProperty _movementType;
		private SerializedProperty _elasticity;
		private SerializedProperty _inertia;
		private SerializedProperty _decelerationRate;
		private SerializedProperty _scrollSensitivity;
		private SerializedProperty _viewport;
		private SerializedProperty _horizontalScrollbar;
		private SerializedProperty _verticalScrollbar;
		private SerializedProperty _horizontalScrollbarVisibility;
		private SerializedProperty _verticalScrollbarVisibility;
		private SerializedProperty _horizontalScrollbarSpacing;
		private SerializedProperty _verticalScrollbarSpacing;
		private SerializedProperty _onValueChanged;
		private AnimBool _showElasticity;
		private AnimBool _showDecelerationRate;
		private bool _viewportIsNotChild, m_HScrollbarIsNotChild, m_VScrollbarIsNotChild;
		private static string _HError = "For this visibility mode, the Viewport property and the Horizontal Scrollbar property both needs to be set to a Rect Transform that is a child to the Scroll Rect.";
		private static string _VError = "For this visibility mode, the Viewport property and the Vertical Scrollbar property both needs to be set to a Rect Transform that is a child to the Scroll Rect.";

		//==========LoopScrollRect==========
		private SerializedProperty _totalCount;
		private SerializedProperty _reverseDirection;
		private int _index = 0;
		private float _offset = 0;
		private LoopScrollRectBase.ScrollMode _scrollMode = LoopScrollRectBase.ScrollMode.ToStart;
		private float _speed = 1000, _time = 1;

		protected virtual void OnEnable () 
		{
			_content = serializedObject.FindProperty("m_Content");
			_horizontal = serializedObject.FindProperty("m_Horizontal");
			_vertical = serializedObject.FindProperty("m_Vertical");
			_movementType = serializedObject.FindProperty("m_MovementType");
			_elasticity = serializedObject.FindProperty("m_Elasticity");
			_inertia = serializedObject.FindProperty("m_Inertia");
			_decelerationRate = serializedObject.FindProperty("m_DecelerationRate");
			_scrollSensitivity = serializedObject.FindProperty("m_ScrollSensitivity");
			_viewport = serializedObject.FindProperty("m_Viewport");
			_horizontalScrollbar = serializedObject.FindProperty("m_HorizontalScrollbar");
			_verticalScrollbar = serializedObject.FindProperty("m_VerticalScrollbar");
			_horizontalScrollbarVisibility = serializedObject.FindProperty("m_HorizontalScrollbarVisibility");
			_verticalScrollbarVisibility = serializedObject.FindProperty("m_VerticalScrollbarVisibility");
			_horizontalScrollbarSpacing = serializedObject.FindProperty("m_HorizontalScrollbarSpacing");
			_verticalScrollbarSpacing = serializedObject.FindProperty("m_VerticalScrollbarSpacing");
			_onValueChanged = serializedObject.FindProperty("m_OnValueChanged");

			_showElasticity = new AnimBool(Repaint);
			_showDecelerationRate = new AnimBool(Repaint);
			SetAnimBools(true);

			//==========LoopScrollRect==========
			_totalCount = serializedObject.FindProperty("totalCount");
			_reverseDirection = serializedObject.FindProperty("reverseDirection");
		}

		protected virtual void OnDisable () {
			_showElasticity.valueChanged.RemoveListener(Repaint);
			_showDecelerationRate.valueChanged.RemoveListener(Repaint);
		}

		void SetAnimBools (bool instant) {
			SetAnimBool(_showElasticity, !_movementType.hasMultipleDifferentValues && _movementType.enumValueIndex == (int)ScrollRect.MovementType.Elastic, instant);
			SetAnimBool(_showDecelerationRate, !_inertia.hasMultipleDifferentValues && _inertia.boolValue == true, instant);
		}

		void SetAnimBool (AnimBool a, bool value, bool instant) {
			if (instant)
				a.value = value;
			else
				a.target = value;
		}

		void CalculateCachedValues () {
			_viewportIsNotChild = false;
			m_HScrollbarIsNotChild = false;
			m_VScrollbarIsNotChild = false;
			if (targets.Length == 1) {
				Transform transform = ((LoopScrollRect)target).transform;
				if (_viewport.objectReferenceValue == null || ((RectTransform)_viewport.objectReferenceValue).transform.parent != transform)
					_viewportIsNotChild = true;
				if (_horizontalScrollbar.objectReferenceValue == null || ((Scrollbar)_horizontalScrollbar.objectReferenceValue).transform.parent != transform)
					m_HScrollbarIsNotChild = true;
				if (_verticalScrollbar.objectReferenceValue == null || ((Scrollbar)_verticalScrollbar.objectReferenceValue).transform.parent != transform)
					m_VScrollbarIsNotChild = true;
			}
		}

		public override void OnInspectorGUI () {
			SetAnimBools(false);

			serializedObject.Update();
			// Once we have a reliable way to know if the object changed, only re-cache in that case.
			CalculateCachedValues();

			EditorGUILayout.PropertyField(_content);
			EditorGUILayout.PropertyField(_horizontal);
			EditorGUILayout.PropertyField(_vertical);
			EditorGUILayout.PropertyField(_movementType);

			if (EditorGUILayout.BeginFadeGroup(_showElasticity.faded)) {
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(_elasticity);
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFadeGroup();

			EditorGUILayout.PropertyField(_inertia);
			if (EditorGUILayout.BeginFadeGroup(_showDecelerationRate.faded)) {
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(_decelerationRate);
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFadeGroup();

			EditorGUILayout.PropertyField(_scrollSensitivity);

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(_viewport);

			EditorGUILayout.PropertyField(_horizontalScrollbar);
			if (_horizontalScrollbar.objectReferenceValue && !_horizontalScrollbar.hasMultipleDifferentValues) {
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(_horizontalScrollbarVisibility, EditorGUIUtility.TrTextContent("Visibility"));

				if ((ScrollRect.ScrollbarVisibility)_horizontalScrollbarVisibility.enumValueIndex == ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport
					&& !_horizontalScrollbarVisibility.hasMultipleDifferentValues) {
					if (_viewportIsNotChild || m_HScrollbarIsNotChild)
						EditorGUILayout.HelpBox(_HError, MessageType.Error);
					EditorGUILayout.PropertyField(_horizontalScrollbarSpacing, EditorGUIUtility.TrTextContent("Spacing"));
				}

				EditorGUI.indentLevel--;
			}

			EditorGUILayout.PropertyField(_verticalScrollbar);
			if (_verticalScrollbar.objectReferenceValue && !_verticalScrollbar.hasMultipleDifferentValues) {
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(_verticalScrollbarVisibility, EditorGUIUtility.TrTextContent("Visibility"));

				if ((ScrollRect.ScrollbarVisibility)_verticalScrollbarVisibility.enumValueIndex == ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport
					&& !_verticalScrollbarVisibility.hasMultipleDifferentValues) {
					if (_viewportIsNotChild || m_VScrollbarIsNotChild)
						EditorGUILayout.HelpBox(_VError, MessageType.Error);
					EditorGUILayout.PropertyField(_verticalScrollbarSpacing, EditorGUIUtility.TrTextContent("Spacing"));
				}

				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(_onValueChanged);

			//==========LoopScrollRect==========
			EditorGUILayout.PropertyField(_totalCount);
			EditorGUILayout.PropertyField(_reverseDirection);

			serializedObject.ApplyModifiedProperties();

			LoopScrollRect scroll = (LoopScrollRect)target;
			GUI.enabled = Application.isPlaying;

			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Clear")) {
				scroll.ClearCells();
			}
			if (GUILayout.Button("Refresh")) {
				scroll.RefreshCells();
			}
			if (GUILayout.Button("Refill")) {
				scroll.RefillCells();
			}
			if (GUILayout.Button("RefillFromEnd")) {
				scroll.RefillCellsFromEnd();
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();

			float w = (EditorGUIUtility.currentViewWidth - 50) / 3;
			EditorGUILayout.BeginHorizontal();
			EditorGUIUtility.labelWidth = 45;
			_index = EditorGUILayout.IntField("Index", _index, GUILayout.Width(w));
			_offset = EditorGUILayout.FloatField("Offset", _offset, GUILayout.Width(w));
			_scrollMode = (LoopScrollRectBase.ScrollMode)EditorGUILayout.EnumPopup("Mode", _scrollMode, GUILayout.Width(w));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUIUtility.labelWidth = 60;
			EditorGUI.indentLevel++;
			_speed = EditorGUILayout.FloatField("Speed", _speed, GUILayout.Width(w + 15));
			EditorGUI.indentLevel--;
			if (GUILayout.Button("Scroll With Speed", GUILayout.Width(130))) {
				scroll.ScrollToCell(_index, _speed, _offset, _scrollMode);
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUIUtility.labelWidth = 60;
			EditorGUI.indentLevel++;
			_time = EditorGUILayout.FloatField("Time", _time, GUILayout.Width(w + 15));
			EditorGUI.indentLevel--;
			if (GUILayout.Button("Scroll Within Time", GUILayout.Width(130))) {
				scroll.ScrollToCellWithinTime(_index, _time, _offset, _scrollMode);
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}