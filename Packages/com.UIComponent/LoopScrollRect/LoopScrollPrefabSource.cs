using UnityEngine;

namespace ZeroFramework.UICom
{
    public interface LoopScrollPrefabSource
    {
        GameObject GetObject(int index);

        void ReturnObject(Transform trans);
    }
}
