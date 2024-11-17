using UnityEngine;

namespace ZeroFramework
{
    public sealed partial class GameFrameworkConfig : ScriptableObjectSingleton<GameFrameworkConfig>
    {
        [SerializeField] private bool m_EnableLoadSceneUpdateEvent = true;
        [SerializeField] private bool m_EnableLoadSceneDependencyAssetEvent = true;
    }
}