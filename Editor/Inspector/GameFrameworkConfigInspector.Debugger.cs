
namespace ZeroFramework.Editor
{
	public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
	{


		[InspectorConfigInit]
		void DebugConsoleInspectorInit () {
			_enableFunc.AddLast(OnDebuggerEnable);
			_inspectorFunc.AddLast(OnDebuggerInspectorGUI);
			_completeFunc.AddLast(OnDebuggerComplete);
		}

		void OnDebuggerEnable () { }
		void OnDebuggerInspectorGUI () { }
		void OnDebuggerComplete () { }
	}
}