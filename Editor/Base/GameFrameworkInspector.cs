﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

using UnityEditor;

namespace ZeroFramework.Editor
{
    /// <summary>
    /// 游戏框架 Inspector 抽象类。
    /// </summary>
    public abstract class GameFrameworkInspector : UnityEditor.Editor
    {
        private bool _isCompiling = false;

        /// <summary>
        /// 绘制事件。
        /// </summary>
        public override void OnInspectorGUI()
        {
            if (_isCompiling && !EditorApplication.isCompiling)
            {
                _isCompiling = false;
                OnCompileComplete();
            }
            else if (!_isCompiling && EditorApplication.isCompiling)
            {
                _isCompiling = true;
                OnCompileStart();
            }
        }

        /// <summary>
        /// 编译开始事件。
        /// </summary>
        protected virtual void OnCompileStart()
        {
        }

        /// <summary>
        /// 编译完成事件。
        /// </summary>
        protected virtual void OnCompileComplete()
        {
        }

        protected bool IsPrefabInHierarchy(UnityEngine.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

#if UNITY_2018_3_OR_NEWER
            return PrefabUtility.GetPrefabAssetType(obj) != PrefabAssetType.Regular;
#else
            return PrefabUtility.GetPrefabType(obj) != PrefabType.Prefab;
#endif
        }
    }
}