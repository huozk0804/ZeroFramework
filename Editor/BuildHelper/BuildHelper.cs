using UnityEditor;

namespace ZeroFramework.Editor
{
	public static class BuildHelper
	{
		[MenuItem(EditorConst.BaseMainPath + "自定义打包/Windows64", priority = EditorConst.MenuPriorityBase)]
		public static void BuildWindow64 () {
		}

		[MenuItem(EditorConst.BaseMainPath + "自定义打包/Android", priority = EditorConst.MenuPriorityBase)]
		public static void BuildAndroid () {
		}

		[MenuItem(EditorConst.BaseMainPath + "自定义打包/IOS", priority = EditorConst.MenuPriorityBase)]
		public static void BuildIOS () {
		}
	}
}

