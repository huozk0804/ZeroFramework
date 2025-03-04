//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ZeroFramework.UI
{
    /// <summary>
    /// 界面管理器。
    /// </summary>
    public sealed partial class UIManager : GameFrameworkModule, IUIManager
    {
        private readonly Dictionary<string, UIGroup> _uiGroups;
        private readonly Dictionary<int, string> _uiFormsBeingLoaded;
        private readonly HashSet<int> _uiFormsToReleaseOnLoad;
        private readonly Queue<IUIPanel> _recycleQueue;

        private IObjectPool<UIPanelInstanceObject> _instancePool;
        private IUIPanelHelper _uiFormHelper;
        private IUIGroupHelper _uiGroupHelper;
		private int _serial;
        private bool _isShutdown;
        private EventHandler<OpenUIPanelSuccessEventArgs> _openUIFormSuccessEventHandler;
        private EventHandler<OpenUIPanelFailureEventArgs> _openUIFormFailureEventHandler;
        private EventHandler<OpenUIPanelUpdateEventArgs> _openUIFormUpdateEventHandler;
        private EventHandler<OpenUIPanelDependencyAssetEventArgs> _openUIFormDependencyAssetEventHandler;
        private EventHandler<CloseUIPanelCompleteEventArgs> _closeUIFormCompleteEventHandler;

        /// <summary>
        /// 初始化界面管理器的新实例。
        /// </summary>
        public UIManager()
        {
            _uiGroups = new Dictionary<string, UIGroup>(StringComparer.Ordinal);
            _uiFormsBeingLoaded = new Dictionary<int, string>();
            _uiFormsToReleaseOnLoad = new HashSet<int>();
            _recycleQueue = new Queue<IUIPanel>();

            _serial = 0;
            _isShutdown = false;
            _openUIFormSuccessEventHandler = null;
            _openUIFormFailureEventHandler = null;
            _openUIFormUpdateEventHandler = null;
            _openUIFormDependencyAssetEventHandler = null;
            _closeUIFormCompleteEventHandler = null;

            _instancePool =
                Zero.Instance.ObjectPool.CreateSingleSpawnObjectPool<UIPanelInstanceObject>("UI Instance Pool");

            UIPanelHelperBase uiFormHelper = Helper.CreateHelper(GameFrameworkConfig.Instance.uiFormHelperTypeName,
                GameFrameworkConfig.Instance.uiFormCustomHelper);
            if (uiFormHelper == null)
            {
                Log.Error("Can not create ui form config helper.");
                return;
            }
            SetUIFormHelper(uiFormHelper);

            UIGroupHelperBase uIGroupHelper = Helper.CreateHelper(GameFrameworkConfig.Instance.uiGroupHelperTypeName, GameFrameworkConfig.Instance.uiGroupCustomHelper);
            if (uIGroupHelper == null) {
                Log.Error("Can not create ui group helper.");
                return;
            }
        }

        /// <summary>
        /// 获取界面组数量。
        /// </summary>
        public int UIGroupCount => _uiGroups.Count;

        /// <summary>
        /// 获取或设置界面实例对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        public float InstanceAutoReleaseInterval
        {
            get => _instancePool.AutoReleaseInterval;
            set => _instancePool.AutoReleaseInterval = value;
        }

        /// <summary>
        /// 获取或设置界面实例对象池的容量。
        /// </summary>
        public int InstanceCapacity
        {
            get => _instancePool.Capacity;
            set => _instancePool.Capacity = value;
        }

        /// <summary>
        /// 获取或设置界面实例对象池对象过期秒数。
        /// </summary>
        public float InstanceExpireTime
        {
            get => _instancePool.ExpireTime;
            set => _instancePool.ExpireTime = value;
        }

        /// <summary>
        /// 获取或设置界面实例对象池的优先级。
        /// </summary>
        public int InstancePriority
        {
            get => _instancePool.Priority;
            set => _instancePool.Priority = value;
        }

        /// <summary>
        /// 打开界面成功事件。
        /// </summary>
        public event EventHandler<OpenUIPanelSuccessEventArgs> OpenUIFormSuccess
        {
            add => _openUIFormSuccessEventHandler += value;
            remove => _openUIFormSuccessEventHandler -= value;
        }

        /// <summary>
        /// 打开界面失败事件。
        /// </summary>
        public event EventHandler<OpenUIPanelFailureEventArgs> OpenUIFormFailure
        {
            add => _openUIFormFailureEventHandler += value;
            remove => _openUIFormFailureEventHandler -= value;
        }

        /// <summary>
        /// 打开界面更新事件。
        /// </summary>
        public event EventHandler<OpenUIPanelUpdateEventArgs> OpenUIFormUpdate
        {
            add => _openUIFormUpdateEventHandler += value;
            remove => _openUIFormUpdateEventHandler -= value;
        }

        /// <summary>
        /// 打开界面时加载依赖资源事件。
        /// </summary>
        public event EventHandler<OpenUIPanelDependencyAssetEventArgs> OpenUIFormDependencyAsset
        {
            add => _openUIFormDependencyAssetEventHandler += value;
            remove => _openUIFormDependencyAssetEventHandler -= value;
        }

        /// <summary>
        /// 关闭界面完成事件。
        /// </summary>
        public event EventHandler<CloseUIPanelCompleteEventArgs> CloseUIFormComplete
        {
            add => _closeUIFormCompleteEventHandler += value;
            remove => _closeUIFormCompleteEventHandler -= value;
        }

        /// <summary>
        /// 界面管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            while (_recycleQueue.Count > 0)
            {
                IUIPanel uiForm = _recycleQueue.Dequeue();
                uiForm.OnRecycle();
                _instancePool.Unspawn(uiForm.Handle);
            }

            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                uiGroup.Value.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 关闭并清理界面管理器。
        /// </summary>
        public override void Shutdown()
        {
            _isShutdown = true;
            CloseAllLoadedUIForms();
            _uiGroups.Clear();
            _uiFormsBeingLoaded.Clear();
            _uiFormsToReleaseOnLoad.Clear();
            _recycleQueue.Clear();
        }

        /// <summary>
        /// 设置界面辅助器。
        /// </summary>
        /// <param name="uiFormHelper">界面辅助器。</param>
        public void SetUIFormHelper(IUIPanelHelper uiFormHelper)
        {
            if(uiFormHelper == null) {
				throw new GameFrameworkException("UI form helper is invalid.");
			}

            _uiFormHelper = uiFormHelper;
        }

		/// <summary>
		/// 设置界面辅助器。
		/// </summary>
		/// <param name="uiFormHelper">界面辅助器。</param>
		public void SetUIGroupHelper (IUIGroupHelper uiGroupHelper) {
			if (uiGroupHelper == null) {
				throw new GameFrameworkException("UI group helper is invalid.");
			}

			_uiGroupHelper = uiGroupHelper;
		}

		/// <summary>
		/// 是否存在界面组。
		/// </summary>
		/// <param name="uiGroupName">界面组名称。</param>
		/// <returns>是否存在界面组。</returns>
		public bool HasUIGroup(string uiGroupName)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new GameFrameworkException("UI group name is invalid.");
            }

            return _uiGroups.ContainsKey(uiGroupName);
        }

        /// <summary>
        /// 获取界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>要获取的界面组。</returns>
        public IUIGroup GetUIGroup(string uiGroupName)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new GameFrameworkException("UI group name is invalid.");
            }

            if (_uiGroups.TryGetValue(uiGroupName, out var uiGroup))
            {
                return uiGroup;
            }

            return null;
        }

        /// <summary>
        /// 获取所有界面组。
        /// </summary>
        /// <returns>所有界面组。</returns>
        public IUIGroup[] GetAllUIGroups()
        {
            int index = 0;
            IUIGroup[] results = new IUIGroup[_uiGroups.Count];
            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                results[index++] = uiGroup.Value;
            }

            return results;
        }

        /// <summary>
        /// 获取所有界面组。
        /// </summary>
        /// <param name="results">所有界面组。</param>
        public void GetAllUIGroups(List<IUIGroup> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                results.Add(uiGroup.Value);
            }
        }

		/// <summary>
		/// 增加界面组。
		/// </summary>
		/// <param name="uiGroupName">界面组名称。</param>
		/// <returns>是否增加界面组成功。</returns>
		public bool AddUIGroup (string uiGroupName) {
			return AddUIGroup(uiGroupName, 0, _uiGroupHelper);
		}

		/// <summary>
		/// 增加界面组。
		/// </summary>
		/// <param name="uiGroupName">界面组名称。</param>
		/// <param name="uiGroupHelper">界面组辅助器。</param>
		/// <returns>是否增加界面组成功。</returns>
		public bool AddUIGroup(string uiGroupName, IUIGroupHelper uiGroupHelper)
        {
            return AddUIGroup(uiGroupName, 0, uiGroupHelper);
        }

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupDepth">界面组深度。</param>
        /// <param name="uiGroupHelper">界面组辅助器。</param>
        /// <returns>是否增加界面组成功。</returns>
        public bool AddUIGroup(string uiGroupName, int uiGroupDepth, IUIGroupHelper uiGroupHelper)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new GameFrameworkException("UI group name is invalid.");
            }

            if (uiGroupHelper == null)
            {
                throw new GameFrameworkException("UI group helper is invalid.");
            }

            if (HasUIGroup(uiGroupName))
            {
                return false;
            }

            _uiGroups.Add(uiGroupName, new UIGroup(uiGroupName, uiGroupDepth, uiGroupHelper));

            return true;
        }

        /// <summary>
        /// 是否存在界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>是否存在界面。</returns>
        public bool HasUIForm(int serialId)
        {
            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                if (uiGroup.Value.HasUIForm(serialId))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 是否存在界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>是否存在界面。</returns>
        public bool HasUIForm(string uiFormAssetName)
        {
            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                throw new GameFrameworkException("UI form asset name is invalid.");
            }

            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                if (uiGroup.Value.HasUIForm(uiFormAssetName))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>要获取的界面。</returns>
        public IUIPanel GetUIForm(int serialId)
        {
            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                IUIPanel uiForm = uiGroup.Value.GetUIForm(serialId);
                if (uiForm != null)
                {
                    return uiForm;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>要获取的界面。</returns>
        public IUIPanel GetUIForm(string uiFormAssetName)
        {
            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                throw new GameFrameworkException("UI form asset name is invalid.");
            }

            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                IUIPanel uiForm = uiGroup.Value.GetUIForm(uiFormAssetName);
                if (uiForm != null)
                {
                    return uiForm;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>要获取的界面。</returns>
        public IUIPanel[] GetUIForms(string uiFormAssetName)
        {
            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                throw new GameFrameworkException("UI form asset name is invalid.");
            }

            List<IUIPanel> results = new List<IUIPanel>();
            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                results.AddRange(uiGroup.Value.GetUIForms(uiFormAssetName));
            }

            return results.ToArray();
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="results">要获取的界面。</param>
        public void GetUIForms(string uiFormAssetName, List<IUIPanel> results)
        {
            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                throw new GameFrameworkException("UI form asset name is invalid.");
            }

            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                uiGroup.Value.InternalGetUIForms(uiFormAssetName, results);
            }
        }

        /// <summary>
        /// 获取所有已加载的界面。
        /// </summary>
        /// <returns>所有已加载的界面。</returns>
        public IUIPanel[] GetAllLoadedUIForms()
        {
            List<IUIPanel> results = new List<IUIPanel>();
            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                results.AddRange(uiGroup.Value.GetAllUIForms());
            }

            return results.ToArray();
        }

        /// <summary>
        /// 获取所有已加载的界面。
        /// </summary>
        /// <param name="results">所有已加载的界面。</param>
        public void GetAllLoadedUIForms(List<IUIPanel> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                uiGroup.Value.InternalGetAllUIForms(results);
            }
        }

        /// <summary>
        /// 获取所有正在加载界面的序列编号。
        /// </summary>
        /// <returns>所有正在加载界面的序列编号。</returns>
        public int[] GetAllLoadingUIFormSerialIds()
        {
            int index = 0;
            int[] results = new int[_uiFormsBeingLoaded.Count];
            foreach (KeyValuePair<int, string> uiFormBeingLoaded in _uiFormsBeingLoaded)
            {
                results[index++] = uiFormBeingLoaded.Key;
            }

            return results;
        }

        /// <summary>
        /// 获取所有正在加载界面的序列编号。
        /// </summary>
        /// <param name="results">所有正在加载界面的序列编号。</param>
        public void GetAllLoadingUIFormSerialIds(List<int> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<int, string> uiFormBeingLoaded in _uiFormsBeingLoaded)
            {
                results.Add(uiFormBeingLoaded.Key);
            }
        }

        /// <summary>
        /// 是否正在加载界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>是否正在加载界面。</returns>
        public bool IsLoadingUIForm(int serialId)
        {
            return _uiFormsBeingLoaded.ContainsKey(serialId);
        }

        /// <summary>
        /// 是否正在加载界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>是否正在加载界面。</returns>
        public bool IsLoadingUIForm(string uiFormAssetName)
        {
            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                throw new GameFrameworkException("UI form asset name is invalid.");
            }

            return _uiFormsBeingLoaded.ContainsValue(uiFormAssetName);
        }

        /// <summary>
        /// 是否是合法的界面。
        /// </summary>
        /// <param name="uiForm">界面。</param>
        /// <returns>界面是否合法。</returns>
        public bool IsValidUIForm(IUIPanel uiForm)
        {
            if (uiForm == null)
            {
                return false;
            }

            return HasUIForm(uiForm.SerialId);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIForm(string uiFormAssetName, string uiGroupName)
        {
            return OpenUIForm(uiFormAssetName, uiGroupName, ResourceConst.DefaultPriority, false, null);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="priority">加载界面资源的优先级。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIForm(string uiFormAssetName, string uiGroupName, int priority)
        {
            return OpenUIForm(uiFormAssetName, uiGroupName, priority, false, null);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIForm(string uiFormAssetName, string uiGroupName, bool pauseCoveredUIForm)
        {
            return OpenUIForm(uiFormAssetName, uiGroupName, ResourceConst.DefaultPriority, pauseCoveredUIForm, null);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIForm(string uiFormAssetName, string uiGroupName, object userData)
        {
            return OpenUIForm(uiFormAssetName, uiGroupName, ResourceConst.DefaultPriority, false, userData);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="priority">加载界面资源的优先级。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIForm(string uiFormAssetName, string uiGroupName, int priority, bool pauseCoveredUIForm)
        {
            return OpenUIForm(uiFormAssetName, uiGroupName, priority, pauseCoveredUIForm, null);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="priority">加载界面资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIForm(string uiFormAssetName, string uiGroupName, int priority, object userData)
        {
            return OpenUIForm(uiFormAssetName, uiGroupName, priority, false, userData);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIForm(string uiFormAssetName, string uiGroupName, bool pauseCoveredUIForm, object userData)
        {
            return OpenUIForm(uiFormAssetName, uiGroupName, ResourceConst.DefaultPriority, pauseCoveredUIForm, userData);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="priority">加载界面资源的优先级。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIForm(string uiFormAssetName, string uiGroupName, int priority, bool pauseCoveredUIForm,
            object userData)
        {
            if (_uiFormHelper == null)
            {
                throw new GameFrameworkException("You must set UI form helper first.");
            }

            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                throw new GameFrameworkException("UI form asset name is invalid.");
            }

            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new GameFrameworkException("UI group name is invalid.");
            }

            UIGroup uiGroup = (UIGroup)GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("UI group '{0}' is not exist.", uiGroupName));
            }

            int serialId = ++_serial;
            UIPanelInstanceObject uiFormInstanceObject = _instancePool.Spawn(uiFormAssetName);
            if (uiFormInstanceObject == null)
            {
                _uiFormsBeingLoaded.Add(serialId, uiFormAssetName);
                //TODO:资源框架引用待修改
                // Zero.Instance.Resource.LoadAsset(uiFormAssetName, priority, m_LoadAssetCallbacks,
                //     OpenUIFormInfo.Create(serialId, uiGroup, pauseCoveredUIForm, userData));
            }
            else
            {
                InternalOpenUIForm(serialId, uiFormAssetName, uiGroup, uiFormInstanceObject.Target, pauseCoveredUIForm,
                    false, 0f, userData);
            }

            return serialId;
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="serialId">要关闭界面的序列编号。</param>
        public void CloseUIForm(int serialId)
        {
            CloseUIForm(serialId, null);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="serialId">要关闭界面的序列编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void CloseUIForm(int serialId, object userData)
        {
            if (IsLoadingUIForm(serialId))
            {
                _uiFormsToReleaseOnLoad.Add(serialId);
                _uiFormsBeingLoaded.Remove(serialId);
                return;
            }

            IUIPanel uiForm = GetUIForm(serialId);
            if (uiForm == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Can not find UI form '{0}'.", serialId));
            }

            CloseUIForm(uiForm, userData);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiForm">要关闭的界面。</param>
        public void CloseUIForm(IUIPanel uiForm)
        {
            CloseUIForm(uiForm, null);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiForm">要关闭的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void CloseUIForm(IUIPanel uiForm, object userData)
        {
            if (uiForm == null)
            {
                throw new GameFrameworkException("UI form is invalid.");
            }

            UIGroup uiGroup = (UIGroup)uiForm.UIGroup;
            if (uiGroup == null)
            {
                throw new GameFrameworkException("UI group is invalid.");
            }

            uiGroup.RemoveUIForm(uiForm);
            uiForm.OnClose(_isShutdown, userData);
            uiGroup.Refresh();

            if (_closeUIFormCompleteEventHandler != null)
            {
                CloseUIPanelCompleteEventArgs closeUIFormCompleteEventArgs =
                    CloseUIPanelCompleteEventArgs.Create(uiForm.SerialId, uiForm.UIFormAssetName, uiGroup, userData);
                _closeUIFormCompleteEventHandler(this, closeUIFormCompleteEventArgs);
                ReferencePool.Release(closeUIFormCompleteEventArgs);
            }

            _recycleQueue.Enqueue(uiForm);
        }

        /// <summary>
        /// 关闭所有已加载的界面。
        /// </summary>
        public void CloseAllLoadedUIForms()
        {
            CloseAllLoadedUIForms(null);
        }

        /// <summary>
        /// 关闭所有已加载的界面。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        public void CloseAllLoadedUIForms(object userData)
        {
            IUIPanel[] uiForms = GetAllLoadedUIForms();
            foreach (IUIPanel uiForm in uiForms)
            {
                if (!HasUIForm(uiForm.SerialId))
                {
                    continue;
                }

                CloseUIForm(uiForm, userData);
            }
        }

        /// <summary>
        /// 关闭所有正在加载的界面。
        /// </summary>
        public void CloseAllLoadingUIForms()
        {
            foreach (KeyValuePair<int, string> uiFormBeingLoaded in _uiFormsBeingLoaded)
            {
                _uiFormsToReleaseOnLoad.Add(uiFormBeingLoaded.Key);
            }

            _uiFormsBeingLoaded.Clear();
        }

        /// <summary>
        /// 激活界面。
        /// </summary>
        /// <param name="uiForm">要激活的界面。</param>
        public void RefocusUIForm(IUIPanel uiForm)
        {
            RefocusUIForm(uiForm, null);
        }

        /// <summary>
        /// 激活界面。
        /// </summary>
        /// <param name="uiForm">要激活的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void RefocusUIForm(IUIPanel uiForm, object userData)
        {
            if (uiForm == null)
            {
                throw new GameFrameworkException("UI form is invalid.");
            }

            UIGroup uiGroup = (UIGroup)uiForm.UIGroup;
            if (uiGroup == null)
            {
                throw new GameFrameworkException("UI group is invalid.");
            }

            uiGroup.RefocusUIForm(uiForm, userData);
            uiGroup.Refresh();
            uiForm.OnRefocus(userData);
        }

        /// <summary>
        /// 设置界面实例是否被加锁。
        /// </summary>
        /// <param name="uiFormInstance">要设置是否被加锁的界面实例。</param>
        /// <param name="locked">界面实例是否被加锁。</param>
        public void SetUIFormInstanceLocked(object uiFormInstance, bool locked)
        {
            if (uiFormInstance == null)
            {
                throw new GameFrameworkException("UI form instance is invalid.");
            }

            _instancePool.SetLocked(uiFormInstance, locked);
        }

        /// <summary>
        /// 设置界面实例的优先级。
        /// </summary>
        /// <param name="uiFormInstance">要设置优先级的界面实例。</param>
        /// <param name="priority">界面实例优先级。</param>
        public void SetUIFormInstancePriority(object uiFormInstance, int priority)
        {
            if (uiFormInstance == null)
            {
                throw new GameFrameworkException("UI form instance is invalid.");
            }

            _instancePool.SetPriority(uiFormInstance, priority);
        }

        private void InternalOpenUIForm(int serialId, string uiFormAssetName, UIGroup uiGroup, object uiFormInstance,
            bool pauseCoveredUIForm, bool isNewInstance, float duration, object userData)
        {
            try
            {
                IUIPanel uiForm = _uiFormHelper.CreateUIForm(uiFormInstance, uiGroup, userData);
                if (uiForm == null)
                {
                    throw new GameFrameworkException("Can not create UI form in UI form helper.");
                }

                uiForm.OnInit(serialId, uiFormAssetName, uiGroup, pauseCoveredUIForm, isNewInstance, userData);
                uiGroup.AddUIForm(uiForm);
                uiForm.OnOpen(userData);
                uiGroup.Refresh();

                if (_openUIFormSuccessEventHandler != null)
                {
                    OpenUIPanelSuccessEventArgs openUIFormSuccessEventArgs =
                        OpenUIPanelSuccessEventArgs.Create(uiForm, duration, userData);
                    _openUIFormSuccessEventHandler(this, openUIFormSuccessEventArgs);
                    ReferencePool.Release(openUIFormSuccessEventArgs);
                }
            }
            catch (Exception exception)
            {
                if (_openUIFormFailureEventHandler != null)
                {
                    OpenUIPanelFailureEventArgs openUIFormFailureEventArgs = OpenUIPanelFailureEventArgs.Create(serialId,
                        uiFormAssetName, uiGroup.Name, pauseCoveredUIForm, exception.ToString(), userData);
                    _openUIFormFailureEventHandler(this, openUIFormFailureEventArgs);
                    ReferencePool.Release(openUIFormFailureEventArgs);
                    return;
                }

                throw;
            }
        }

        private void LoadAssetSuccessCallback(string uiFormAssetName, object uiFormAsset, float duration,
            object userData)
        {
            OpenUIPanelInfo openUIFormInfo = (OpenUIPanelInfo)userData;
            if (openUIFormInfo == null)
            {
                throw new GameFrameworkException("Open UI form info is invalid.");
            }

            if (_uiFormsToReleaseOnLoad.Contains(openUIFormInfo.SerialId))
            {
                _uiFormsToReleaseOnLoad.Remove(openUIFormInfo.SerialId);
                ReferencePool.Release(openUIFormInfo);
                _uiFormHelper.ReleaseUIForm(uiFormAsset, null);
                return;
            }

            _uiFormsBeingLoaded.Remove(openUIFormInfo.SerialId);
            UIPanelInstanceObject uiFormInstanceObject = UIPanelInstanceObject.Create(uiFormAssetName, uiFormAsset,
                _uiFormHelper.InstantiateUIForm(uiFormAsset), _uiFormHelper);
            _instancePool.Register(uiFormInstanceObject, true);

            InternalOpenUIForm(openUIFormInfo.SerialId, uiFormAssetName, openUIFormInfo.UIGroup,
                uiFormInstanceObject.Target, openUIFormInfo.PauseCoveredUIForm, true, duration,
                openUIFormInfo.UserData);
            ReferencePool.Release(openUIFormInfo);
        }

        private void LoadAssetFailureCallback(string uiFormAssetName, LoadResourceStatus status, string errorMessage,
            object userData)
        {
            OpenUIPanelInfo openUIFormInfo = (OpenUIPanelInfo)userData;
            if (openUIFormInfo == null)
            {
                throw new GameFrameworkException("Open UI form info is invalid.");
            }

            if (_uiFormsToReleaseOnLoad.Contains(openUIFormInfo.SerialId))
            {
                _uiFormsToReleaseOnLoad.Remove(openUIFormInfo.SerialId);
                return;
            }

            _uiFormsBeingLoaded.Remove(openUIFormInfo.SerialId);
            string appendErrorMessage =
                Utility.Text.Format("Load UI form failure, asset name '{0}', status '{1}', error message '{2}'.",
                    uiFormAssetName, status, errorMessage);
            if (_openUIFormFailureEventHandler != null)
            {
                OpenUIPanelFailureEventArgs openUIFormFailureEventArgs =
                    OpenUIPanelFailureEventArgs.Create(openUIFormInfo.SerialId, uiFormAssetName,
                        openUIFormInfo.UIGroup.Name, openUIFormInfo.PauseCoveredUIForm, appendErrorMessage,
                        openUIFormInfo.UserData);
                _openUIFormFailureEventHandler(this, openUIFormFailureEventArgs);
                ReferencePool.Release(openUIFormFailureEventArgs);
                return;
            }

            throw new GameFrameworkException(appendErrorMessage);
        }

        private void LoadAssetUpdateCallback(string uiFormAssetName, float progress, object userData)
        {
            OpenUIPanelInfo openUIFormInfo = (OpenUIPanelInfo)userData;
            if (openUIFormInfo == null)
            {
                throw new GameFrameworkException("Open UI form info is invalid.");
            }

            if (_openUIFormUpdateEventHandler != null)
            {
                OpenUIPanelUpdateEventArgs openUIFormUpdateEventArgs =
                    OpenUIPanelUpdateEventArgs.Create(openUIFormInfo.SerialId, uiFormAssetName,
                        openUIFormInfo.UIGroup.Name, openUIFormInfo.PauseCoveredUIForm, progress,
                        openUIFormInfo.UserData);
                _openUIFormUpdateEventHandler(this, openUIFormUpdateEventArgs);
                ReferencePool.Release(openUIFormUpdateEventArgs);
            }
        }

        private void LoadAssetDependencyAssetCallback(string uiFormAssetName, string dependencyAssetName,
            int loadedCount, int totalCount, object userData)
        {
            OpenUIPanelInfo openUIFormInfo = (OpenUIPanelInfo)userData;
            if (openUIFormInfo == null)
            {
                throw new GameFrameworkException("Open UI form info is invalid.");
            }

            if (_openUIFormDependencyAssetEventHandler != null)
            {
                OpenUIPanelDependencyAssetEventArgs openUIFormDependencyAssetEventArgs =
                    OpenUIPanelDependencyAssetEventArgs.Create(openUIFormInfo.SerialId, uiFormAssetName,
                        openUIFormInfo.UIGroup.Name, openUIFormInfo.PauseCoveredUIForm, dependencyAssetName,
                        loadedCount, totalCount, openUIFormInfo.UserData);
                _openUIFormDependencyAssetEventHandler(this, openUIFormDependencyAssetEventArgs);
                ReferencePool.Release(openUIFormDependencyAssetEventArgs);
            }
        }
    }
}