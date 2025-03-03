using System.Text;
using UnityEditor;
using UnityEngine;

namespace ZeroFramework.Editor
{
    public abstract class EditorScriptableObjectSingleton<T> : ScriptableObject
        where T : EditorScriptableObjectSingleton<T>
    {
        protected static T _Instance;

        public static T Instance
        {
            get
            {
                if (!_Instance)
                {
                    LoadInstance();
                }

                return _Instance;
            }
        }

        private static void LoadInstance()
        {
            string name = typeof(T).Name;
            string[] array = AssetDatabase.FindAssets("t:" + name);
            if (array.Length == 0)
            {
                _Instance = CreateInstance<T>();
                AssetDatabase.CreateAsset(_Instance, "Assets/" + name + ".asset");
            }
            else
            {
                if (array.Length > 1)
                {
                    StringBuilder tips = new StringBuilder();
                    tips.AppendLine($"There are {array.Length} {name} in the project:");
                    foreach (string text in array)
                    {
                        tips.AppendLine(AssetDatabase.GUIDToAssetPath(text));
                    }

                    tips.Append("Use " + AssetDatabase.GUIDToAssetPath(array[0]));
                    Log.Warning(tips);
                }

                string path = AssetDatabase.GUIDToAssetPath(array[0]);
                _Instance = AssetDatabase.LoadAssetAtPath<T>(path);
            }
        }
    }
}