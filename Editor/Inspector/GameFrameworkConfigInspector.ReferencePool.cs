using System;
using System.Collections.Generic;
using UnityEditor;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private readonly Dictionary<string, List<ReferencePoolInfo>> _referencePoolInfos =
            new Dictionary<string, List<ReferencePoolInfo>>(StringComparer.Ordinal);

        private readonly HashSet<string> _openedItems = new HashSet<string>();
        private SerializedProperty _enableStrictCheck = null;

        [InspectorConfigInit]
        void ReferencePoolInspectorInit()
        {
            _enableFunc.AddLast(OnReferencePoolEnable);
            _inspectorFunc.AddLast(OnReferencePoolInspectorGUI);
            _completeFunc.AddLast(OnReferencePoolComplete);
        }

        void OnReferencePoolInspectorGUI()
        {
            EditorGUILayout.LabelField("ReferencePool", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_enableStrictCheck);
        }

        void OnReferencePoolEnable()
        {
            _enableStrictCheck = serializedObject.FindProperty("enableStrictCheck");
        }

        void OnReferencePoolComplete()
        {
        }
    }
}