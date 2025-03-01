//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework.Entity
{
    /// <summary>
    /// 显示实体失败事件。
    /// </summary>
    public sealed class ShowEntityFailureEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化显示实体失败事件的新实例。
        /// </summary>
        public ShowEntityFailureEventArgs()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroupName = null;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 获取实体编号。
        /// </summary>
        public int EntityId { get; private set; }

        /// <summary>
        /// 获取实体资源名称。
        /// </summary>
        public string EntityAssetName { get; private set; }

        /// <summary>
        /// 获取实体组名称。
        /// </summary>
        public string EntityGroupName { get; private set; }

        /// <summary>
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建显示实体失败事件。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <param name="entityGroupName">实体组名称。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的显示实体失败事件。</returns>
        public static ShowEntityFailureEventArgs Create(int entityId, string entityAssetName, string entityGroupName,
            string errorMessage, object userData)
        {
            ShowEntityFailureEventArgs showEntityFailureEventArgs0 =
                ReferencePool.Acquire<ShowEntityFailureEventArgs>();
            showEntityFailureEventArgs0.EntityId = entityId;
            showEntityFailureEventArgs0.EntityAssetName = entityAssetName;
            showEntityFailureEventArgs0.EntityGroupName = entityGroupName;
            showEntityFailureEventArgs0.ErrorMessage = errorMessage;
            showEntityFailureEventArgs0.UserData = userData;
            return showEntityFailureEventArgs0;
        }

        /// <summary>
        /// 清理显示实体失败事件。
        /// </summary>
        public override void Clear()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroupName = null;
            ErrorMessage = null;
            UserData = null;
        }
    }
}