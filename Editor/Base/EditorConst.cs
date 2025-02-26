namespace ZeroFramework.Editor
{
    public static class EditorConst
    {
        //编辑器的基础路径，可以统一修改
        public const string BaseMainPath = "ZERO/";
        public const string BaseGameObjectPath = "GameObject/ZERO/";
        public const string BaseAssetPath = "Assets/ZERO/";

		//编辑器按钮显示的优先级管理
        //数值越小，位置越靠上：默认优先级为 1000
		//相邻项差值 ≥11 时自动插入分隔线
		public const int MenuPriority_Base = 0;
        public const int MenuPriority_Com = 100;
        public const int MenuPriority_Intro = 200;
	}
}