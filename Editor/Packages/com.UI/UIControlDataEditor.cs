using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ZeroFramework.UI;

namespace ZeroFramework.Editor
{
    [CustomEditor(typeof(UIControlData))]
    public class UIControlDataEditor : UnityEditor.Editor
    {
        public static GUIStyle titleStyle;
        public static GUIStyle labelStyle;
        public static GUIStyle boxStyle;
        public static GUIStyle textFieldStyle;
        public static GUIStyle popupAlignLeft;
        public string[] allTypeNames;
        public System.Type[] allTypes;

        private List<CtrlItemData> _ctrlItemDatas;
        private List<SubUIItemData> _subUIItemDatas;
        private List<ControlItemDrawer> _ctrlItemDrawers;
        private List<SubUIItemDrawer> _subUIItemDrawers;
        private string _searchPattern = string.Empty;
        private string _createPath = string.Empty;

        void Awake()
        {
            if (titleStyle == null)
            {
                titleStyle = new GUIStyle("Title");
                titleStyle.fontStyle = FontStyle.Bold;
                titleStyle.fontSize = 16;
                titleStyle.normal.textColor = Color.grey;
            }

            if (labelStyle == null)
            {
                labelStyle = new GUIStyle("Label");
                labelStyle.fontSize = 13;
                labelStyle.wordWrap = true;
            }

            if (boxStyle == null)
            {
                boxStyle = new GUIStyle("Box");
            }

            if (textFieldStyle == null)
            {
                textFieldStyle = new GUIStyle("TextField");
                textFieldStyle.fixedHeight = 20;
                textFieldStyle.padding = new RectOffset(3, 3, 3, 3);
            }

            if (popupAlignLeft == null)
            {
                popupAlignLeft = new GUIStyle("Popup");
                popupAlignLeft.alignment = TextAnchor.MiddleLeft;
            }

            allTypeNames = UIControlData.GetAllTypeNames();
            allTypes = UIControlData.GetAllTypes();
        }

        public override void OnInspectorGUI()
        {
            _searchPattern = EditorGUILayout.TextField(_searchPattern);
            UIControlData data = target as UIControlData;
            if (data.ctrlItemDatas == null)
                data.ctrlItemDatas = new List<CtrlItemData>();

            if (data.subUIItemDatas == null)
                data.subUIItemDatas = new List<SubUIItemData>();

            _ctrlItemDatas = data.ctrlItemDatas;
            _subUIItemDatas = data.subUIItemDatas;
            CheckDrawers();

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.Space();

                // 绘制控件绑定
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("控件绑定", titleStyle);
                    if (_ctrlItemDrawers.Count == 0)
                    {
                        if (GUILayout.Button("+", EditorStyles.miniButton))
                        {
                            AddControlAfter(-1);
                            Repaint();
                            return;
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
                foreach (var drawer in _ctrlItemDrawers)
                {
                    if (!drawer.Match(_searchPattern))
                        continue;
                    GUILayout.Space(10f);
                    if (!drawer.Draw())
                    {
                        Repaint();
                        return;
                    }

                    GUILayout.Space(10f);
                }

                GUILayout.Space(10f);

                // 绘制子UI
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("子UI绑定", titleStyle);
                    if (_subUIItemDrawers.Count == 0)
                    {
                        if (GUILayout.Button("+", EditorStyles.miniButton))
                        {
                            AddSubUIAfter(-1);
                            Repaint();
                            return;
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
                foreach (var drawer in _subUIItemDrawers)
                {
                    GUILayout.Space(10f);
                    if (!drawer.Draw())
                    {
                        Repaint();
                        return;
                    }

                    GUILayout.Space(10f);
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            this.Repaint();
        }

        public void AddControlAfter(ControlItemDrawer drawer)
        {
            int idx = _ctrlItemDrawers.IndexOf(drawer);
            Debug.Assert(idx != -1);

            AddControlAfter(idx);
        }

        public void AddSubUIAfter(SubUIItemDrawer drawer)
        {
            int idx = _subUIItemDrawers.IndexOf(drawer);
            Debug.Assert(idx != -1);

            AddSubUIAfter(idx);
        }

        public void RemoveControl(ControlItemDrawer drawer)
        {
            int idx = _ctrlItemDrawers.IndexOf(drawer);
            Debug.Assert(idx != -1);

            RemoveControl(idx);
        }

        public void RemoveSubUI(SubUIItemDrawer drawer)
        {
            int idx = _subUIItemDrawers.IndexOf(drawer);
            Debug.Assert(idx != -1);

            RemoveSubUI(idx);
        }

        #region Private

        private void CheckDrawers()
        {
            if (_ctrlItemDrawers == null)
            {
                _ctrlItemDrawers = new List<ControlItemDrawer>(100);
                foreach (var item in _ctrlItemDatas)
                {
                    ControlItemDrawer drawer = new ControlItemDrawer(this, item);
                    _ctrlItemDrawers.Add(drawer);
                }
            }

            if (_subUIItemDrawers == null)
            {
                _subUIItemDrawers = new List<SubUIItemDrawer>(100);
                foreach (var item in _subUIItemDatas)
                {
                    SubUIItemDrawer drawer = new SubUIItemDrawer(this, item);
                    _subUIItemDrawers.Add(drawer);
                }
            }
        }

        private void AddControlAfter(int idx)
        {
            CtrlItemData itemData = new CtrlItemData();
            _ctrlItemDatas.Insert(idx + 1, itemData);

            ControlItemDrawer drawer = new ControlItemDrawer(this, itemData);
            _ctrlItemDrawers.Insert(idx + 1, drawer);
        }

        private void AddSubUIAfter(int idx)
        {
            SubUIItemData itemData = new SubUIItemData();
            _subUIItemDatas.Insert(idx + 1, itemData);

            SubUIItemDrawer drawer = new SubUIItemDrawer(this, itemData);
            _subUIItemDrawers.Insert(idx + 1, drawer);
        }

        private void RemoveControl(int idx)
        {
            _ctrlItemDatas.RemoveAt(idx);
            _ctrlItemDrawers.RemoveAt(idx);
        }

        private void RemoveSubUI(int idx)
        {
            _subUIItemDatas.RemoveAt(idx);
            _subUIItemDrawers.RemoveAt(idx);
        }

        #endregion
    }
}