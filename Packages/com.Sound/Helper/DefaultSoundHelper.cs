//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework.Sound
{
    /// <summary>
    /// 默认声音辅助器。
    /// </summary>
    public class DefaultSoundHelper : SoundHelperBase
    {
        //private ResourceManager _resourceComponent = null;

        /// <summary>
        /// 释放声音资源。
        /// </summary>
        /// <param name="soundAsset">要释放的声音资源。</param>
        public override void ReleaseSoundAsset(object soundAsset)
        {
            //TODO:资源框架引用待修改
            // m_ResourceComponent.UnloadAsset(soundAsset);
        }

        private void Start()
        {
            //m_ResourceComponent = GameEntry.GetComponent<ResourceComponent>();
            //if (_resourceComponent == null)
            //{
            //    Log.Fatal("Resource component is invalid.");
            //    return;
            //}
        }
    }
}
