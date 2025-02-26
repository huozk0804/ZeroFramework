using System.Text;
using UnityEditor;
using UnityEngine;

namespace ZeroFramework.Editor
{
    public abstract class EditorScriptableObjectSingleton<T> : ScriptableObject
        where T : EditorScriptableObjectSingleton<T>
    {
        protected static T instance;

        public static T Instance
        {
            get
            {
                if (!instance)
                {
                    LoadInstance();
                }

                return instance;
            }
        }

        private static void LoadInstance()
        {
            string name = typeof(T).Name;
            string[] array = AssetDatabase.FindAssets("t:" + name);
            if (array.Length == 0)
            {
                instance = CreateInstance<T>();
                AssetDatabase.CreateAsset(instance, "Assets/" + name + ".asset");
            }
            else
            {
                if (array.Length > 1)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine($"There are {array.Length} {name} in the project:");
                    foreach (string text in array)
                    {
                        stringBuilder.AppendLine(AssetDatabase.GUIDToAssetPath(text));
                    }

                    stringBuilder.Append("Use " + AssetDatabase.GUIDToAssetPath(array[0]));
                    Log.Warning(stringBuilder);
                }

                string text2 = AssetDatabase.GUIDToAssetPath(array[0]);
                instance = AssetDatabase.LoadAssetAtPath<T>(text2);
            }
        }
    }
}