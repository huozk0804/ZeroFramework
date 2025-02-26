using UnityEditor;
using ZeroFramework.Entity;

namespace ZeroFramework.Editor
{
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private SerializedProperty _enableShowEntityUpdateEvent = null;
        private SerializedProperty _enableShowEntityDependencyAssetEvent = null;
        private SerializedProperty _entityGroups = null;
        private readonly HelperInfo<EntityHelperBase> _entityHelperInfo = new HelperInfo<EntityHelperBase>("entity");

        private readonly HelperInfo<EntityGroupHelperBase> _entityGroupHelperInfo =
            new HelperInfo<EntityGroupHelperBase>("entityGroup");

        [InspectorConfigInit]
        void EntityInspectorInit()
        {
            _enableFunc.AddLast(OnEntityEnable);
            _inspectorFunc.AddLast(OnEntityInspectorGUI);
            _completeFunc.AddLast(OnEntityComplete);
        }

        void OnEntityInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.LabelField("Entity", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_enableShowEntityUpdateEvent);
                EditorGUILayout.PropertyField(_enableShowEntityDependencyAssetEvent);
                _entityHelperInfo.Draw();
                _entityGroupHelperInfo.Draw();
                EditorGUILayout.PropertyField(_entityGroups, true);
            }
            EditorGUI.EndDisabledGroup();
        }

        void OnEntityEnable()
        {
            _enableShowEntityUpdateEvent = serializedObject.FindProperty("enableShowEntityUpdateEvent");
            _enableShowEntityDependencyAssetEvent =
                serializedObject.FindProperty("enableShowEntityDependencyAssetEvent");
            _entityGroups = serializedObject.FindProperty("entityGroups");
            _entityHelperInfo.Init(serializedObject);
            _entityGroupHelperInfo.Init(serializedObject);
            _entityHelperInfo.Refresh();
            _entityGroupHelperInfo.Refresh();
        }

        void OnEntityComplete()
        {
            _entityGroupHelperInfo.Refresh();
            _entityHelperInfo.Refresh();
        }
    }
}