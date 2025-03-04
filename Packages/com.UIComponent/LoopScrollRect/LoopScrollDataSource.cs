using UnityEngine;

namespace ZeroFramework.UICom
{
    public interface LoopScrollDataSource
    {
        void ProvideData(Transform transform, int idx);
    }
}