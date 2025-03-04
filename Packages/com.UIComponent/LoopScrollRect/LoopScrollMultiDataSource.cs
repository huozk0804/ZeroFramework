using UnityEngine;

namespace ZeroFramework.UICom
{
    public interface LoopScrollMultiDataSource
    {
        void ProvideData(Transform transform, int index);
    }
}