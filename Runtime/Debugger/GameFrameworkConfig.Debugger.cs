using UnityEngine;
using ZeroFramework.Debugger;

namespace ZeroFramework
{
	public sealed partial class GameFrameworkConfig : ScriptableObjectSingleton<GameFrameworkConfig>
	{
		[SerializeField]
		private GUISkin m_Skin = null;

		[SerializeField]
		private DebuggerActiveWindowType m_ActiveWindow = DebuggerActiveWindowType.AlwaysOpen;

		[SerializeField]
		private bool m_ShowFullWindow = false;

		//[SerializeField]
		//private ConsoleWindow m_ConsoleWindow = new ConsoleWindow();
	}
}