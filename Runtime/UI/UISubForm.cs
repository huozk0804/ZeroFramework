namespace ZeroFramework.UI
{
    public abstract class UISubForm : IReference
    {
        public UIForm parent { get; }
        
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
            throw new System.NotImplementedException();
        }
    }
}