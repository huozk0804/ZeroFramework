using System;
using UnityEngine;
using ZeroFramework.UI;

namespace ZeroFramework
{
    public sealed partial class GameFrameworkConfig : ScriptableObjectSingleton<GameFrameworkConfig>
    {
        [SerializeField] public bool m_EnableOpenUIFormSuccessEvent = true;
        [SerializeField] public bool m_EnableOpenUIFormFailureEvent = true;
        [SerializeField] public bool m_EnableOpenUIFormUpdateEvent = false;
        [SerializeField] public bool m_EnableOpenUIFormDependencyAssetEvent = false;
        [SerializeField] public bool m_EnableCloseUIFormCompleteEvent = true;
        [SerializeField] public float m_InstanceAutoReleaseInterval = 60f;
        [SerializeField] public int m_InstanceCapacity = 16;
        [SerializeField] public float m_InstanceExpireTime = 60f;
        [SerializeField] public int m_InstancePriority = 0;
        [SerializeField] public string m_UIFormHelperTypeName = "ZeroFramework.UI.DefaultUIFormHelper";
        [SerializeField] public UIFormHelperBase m_CustomUIFormHelper = null;
        [SerializeField] public string m_UIGroupHelperTypeName = "ZeroFramework.UI.DefaultUIGroupHelper";
        [SerializeField] public UIGroupHelperBase m_CustomUIGroupHelper = null;
        [SerializeField] public UIGroup[] m_UIGroups = null;

        [Serializable]
        public sealed class UIGroup
        {
            [SerializeField] private string m_Name = null;
            [SerializeField] private int m_Depth = 0;
            public string Name => m_Name;
            public int Depth => m_Depth;
        }
    }
}