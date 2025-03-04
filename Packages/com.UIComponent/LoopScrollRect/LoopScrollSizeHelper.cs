using UnityEngine;

namespace ZeroFramework.UICom
{
    // optional class for better scroll support
    public interface LoopScrollSizeHelper
    {
        Vector2 GetItemsSize(int itemsCount);
    }
}
