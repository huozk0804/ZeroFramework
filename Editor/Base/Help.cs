//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace ZeroFramework.Editor
{
    /// <summary>
    /// 帮助相关的实用函数。
    /// </summary>
    public static class Help
    {
        [MenuItem(EditorConst.BaseMainPath + "Documentation", false, priority = EditorConst.MenuPriority_Intro)]
        public static void ShowDocumentation()
        {
            ShowHelp("https://gameframework.cn/document/");
        }

        [MenuItem(EditorConst.BaseMainPath + "API Reference", false, priority = EditorConst.MenuPriority_Intro)]
        public static void ShowApiReference()
        {
            ShowHelp("https://gameframework.cn/api/");
        }

        [MenuItem(EditorConst.BaseMainPath + "Git Repository", false, priority = EditorConst.MenuPriority_Intro)]
        public static void ShowGitRepository()
        {
            ShowHelp("https://github.com/huozk0804/ZeroFramework");
        }

        private static void ShowHelp(string uri)
        {
            Application.OpenURL(uri);
        }
    }
}