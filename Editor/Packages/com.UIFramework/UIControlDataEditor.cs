using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ZeroFramework.UI;

namespace ZeroFramework.Editor.Package
{
    [CustomEditor(typeof(UIControlData))]
    public class UIControlDataEditor : UnityEditor.Editor
    {
        public string[] allTypeNames;
        public System.Type[] allTypes;

        private List<CtrlItemData> _ctrlItemData;
        private List<ControlItemDrawer> _ctrlItemDrawers;
        private string _searchPattern = string.Empty;
        private string _createPath = string.Empty;

        void Awake()
        {
            allTypeNames = UIControlData.GetAllTypeNames();
            allTypes = UIControlData.GetAllTypes();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space(4f);
            _searchPattern = EditorGUILayout.TextField(_searchPattern);
            UIControlData data = target as UIControlData;
            if (data.ctrlItemData == null)
                data.ctrlItemData = new List<CtrlItemData>();

            _ctrlItemData = data.ctrlItemData;
            CheckDrawers();

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.Space();

                // 绘制控件绑定
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("控件绑定", EditorStyles.boldLabel);
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
                EditorGUILayout.BeginVertical();
                {
                    var com = (UIControlData)target;
                    EditorGUILayout.LabelField("操作按钮", EditorStyles.boldLabel);
                    
                    GUILayout.Space(10f);
                    
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("生成路径(相对Assets)：",GUILayout.Width(130f));
                        _createPath = EditorGUILayout.TextField(_createPath);
                    }
                    EditorGUILayout.EndHorizontal();
                    
                    GUILayout.Space(5f);
                    if (GUILayout.Button("生成/更新代码模板"))
                    {
                        com.GenerateCodeTemplate(_createPath);
                    }
                    
                    GUILayout.Space(10f);
                    
                    if (GUILayout.Button("复制代码变量声明到剪贴板"))
                    {
                        com.CopyCodeDefineToClipBoardPrivate();
                    }
                    
                    GUILayout.Space(10f);
                    
                    if (GUILayout.Button("复制代码变量赋值到剪贴板"))
                    {
                        com.CopyCodeFindToClipBoard();
                    }
                    
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            this.Repaint();
        }

        public void AddControlAfter(ControlItemDrawer drawer)
        {
            int idx = _ctrlItemDrawers.IndexOf(drawer);
            Debug.Assert(idx != -1);

            AddControlAfter(idx);
        }

        public void RemoveControl(ControlItemDrawer drawer)
        {
            int idx = _ctrlItemDrawers.IndexOf(drawer);
            Debug.Assert(idx != -1);

            RemoveControl(idx);
        }

        #region Private

        private void CheckDrawers()
        {
            if (_ctrlItemDrawers == null)
            {
                _ctrlItemDrawers = new List<ControlItemDrawer>(100);
                foreach (var item in _ctrlItemData)
                {
                    ControlItemDrawer drawer = new ControlItemDrawer(this, item);
                    _ctrlItemDrawers.Add(drawer);
                }
            }
        }

        private void AddControlAfter(int idx)
        {
            CtrlItemData itemData = new CtrlItemData();
            _ctrlItemData.Insert(idx + 1, itemData);

            ControlItemDrawer drawer = new ControlItemDrawer(this, itemData);
            _ctrlItemDrawers.Insert(idx + 1, drawer);
        }

        private void RemoveControl(int idx)
        {
            _ctrlItemData.RemoveAt(idx);
            _ctrlItemDrawers.RemoveAt(idx);
        }

        #endregion
    }
}