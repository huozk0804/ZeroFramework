using UnityEngine;

namespace ZeroFramework
{
	[DisallowMultipleComponent]
	public sealed class ZeroFrameworkComponent : MonoBehaviour
	{
		private void OnDestroy () {
			StopAllCoroutines();
		}
	}
}