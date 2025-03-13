using UnityEngine;

namespace ZeroFramework
{
	[DisallowMultipleComponent]
	public sealed class ZeroFrameworkComponent : MonoBehaviour
	{
		private void Awake()
		{
			DontDestroyOnLoad(this);
		}

		private void OnDestroy () {
			StopAllCoroutines();
		}
	}
}