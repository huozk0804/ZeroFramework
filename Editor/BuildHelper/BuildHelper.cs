using UnityEditor;

namespace ZeroFramework.Editor
{
	public static class BuildHelper
	{
		[MenuItem(EditorConst.BaseMainPath + "自定义打包/Windows64", priority = EditorConst.MenuPriority_Base)]
		public static void BuildWindow64 () {
		}

		[MenuItem(EditorConst.BaseMainPath + "自定义打包/Android", priority = EditorConst.MenuPriority_Base)]
		public static void BuildAndroid () {
		}

		[MenuItem(EditorConst.BaseMainPath + "自定义打包/IOS", priority = EditorConst.MenuPriority_Base)]
		public static void BuildIOS () {
		}
	}
}

