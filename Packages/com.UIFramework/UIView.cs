namespace ZeroFramework.UI
{
    /// <summary>
    /// 不单独存在，依附于UIForm实例存在
    /// </summary>
    public abstract class UIView : IReference
    {
        public UIPanel Parent { get; }
        
        public virtual void OnShow(object args)
        {
        }

        public virtual void OnHide()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnDestroy()
        {
        }

        public void Clear()
        {
            
        }
    }
}