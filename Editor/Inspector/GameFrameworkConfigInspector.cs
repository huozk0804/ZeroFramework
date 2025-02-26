using System;
using System.Reflection;
using UnityEditor;

namespace ZeroFramework.Editor
{
    [CustomEditor(typeof(GameFrameworkConfig))]
    public sealed partial class GameFrameworkConfigInspector : GameFrameworkInspector
    {
        private readonly GameFrameworkLinkedList<Action> _inspectorFunc =
            new GameFrameworkLinkedList<Action>();

        private readonly GameFrameworkLinkedList<Action> _enableFunc =
            new GameFrameworkLinkedList<Action>();

        private readonly GameFrameworkLinkedList<Action> _completeFunc =
            new GameFrameworkLinkedList<Action>();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            foreach (var item in _inspectorFunc)
            {
                item.Invoke();
                EditorGUILayout.Space(20);
            }

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();
            foreach (var action in _completeFunc)
            {
                action.Invoke();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            MethodInfo[] allMethods =
                typeof(GameFrameworkConfigInspector).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance |
                                                                BindingFlags.DeclaredOnly);
            foreach (MethodInfo method in allMethods)
            {
                if (method.IsDefined(typeof(InspectorConfigInitAttribute), true))
                {
                    method.Invoke(this, null);
                }
            }

            foreach (var item in _enableFunc)
            {
                item.Invoke();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InspectorConfigInitAttribute : Attribute
    {
    }
}