//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace ZeroFramework.Editor
{
    /// <summary>
    /// 构建配置相关的实用函数。
    /// </summary>
    internal static class BuildSettings
    {
        private static readonly string _ConfigurationPath = null;
        private static readonly List<string> _DefaultSceneNames = new List<string>();
        private static readonly List<string> _SearchScenePaths = new List<string>();

        static BuildSettings()
        {
            _ConfigurationPath = Type.GetConfigurationPath<BuildSettingsConfigPathAttribute>() ??
                                  Utility.Path.GetRegularPath(Path.Combine(Application.dataPath,
                                      "ZeroFramework/Configs/BuildSettings.xml"));
            _DefaultSceneNames.Clear();
            _SearchScenePaths.Clear();

            if (!File.Exists(_ConfigurationPath))
            {
                return;
            }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(_ConfigurationPath);
                XmlNode xmlRoot = xmlDocument.SelectSingleNode("ZeroFramework");
                XmlNode xmlBuildSettings = xmlRoot.SelectSingleNode("BuildSettings");
                XmlNode xmlDefaultScenes = xmlBuildSettings.SelectSingleNode("DefaultScenes");
                XmlNode xmlSearchScenePaths = xmlBuildSettings.SelectSingleNode("SearchScenePaths");

                XmlNodeList xmlNodeList = null;
                XmlNode xmlNode = null;

                xmlNodeList = xmlDefaultScenes.ChildNodes;
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    xmlNode = xmlNodeList.Item(i);
                    if (xmlNode.Name != "DefaultScene")
                    {
                        continue;
                    }

                    string defaultSceneName = xmlNode.Attributes.GetNamedItem("Name").Value;
                    _DefaultSceneNames.Add(defaultSceneName);
                }

                xmlNodeList = xmlSearchScenePaths.ChildNodes;
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    xmlNode = xmlNodeList.Item(i);
                    if (xmlNode.Name != "SearchScenePath")
                    {
                        continue;
                    }

                    string searchScenePath = xmlNode.Attributes.GetNamedItem("Path").Value;
                    _SearchScenePaths.Add(searchScenePath);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 将构建场景设置为默认。
        /// </summary>
        [MenuItem(EditorConst.BaseMainPath + "Scenes/Default scenes in build setting", false, 20)]
        public static void DefaultScenes()
        {
            HashSet<string> sceneNames = new HashSet<string>();
            foreach (string sceneName in _DefaultSceneNames)
            {
                sceneNames.Add(sceneName);
            }

            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
            foreach (string sceneName in sceneNames)
            {
                scenes.Add(new EditorBuildSettingsScene(sceneName, true));
            }

            EditorBuildSettings.scenes = scenes.ToArray();

            Debug.Log("Set scenes of build settings to default scenes.");
        }

        /// <summary>
        /// 将构建场景设置为所有。
        /// </summary>
        [MenuItem(EditorConst.BaseMainPath + "Scenes/All scenes in build settings", false, 21)]
        public static void AllScenes()
        {
            HashSet<string> sceneNames = new HashSet<string>();
            foreach (string sceneName in _DefaultSceneNames)
            {
                sceneNames.Add(sceneName);
            }

            string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", _SearchScenePaths.ToArray());
            foreach (string sceneGuid in sceneGuids)
            {
                string sceneName = AssetDatabase.GUIDToAssetPath(sceneGuid);
                sceneNames.Add(sceneName);
            }

            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
            foreach (string sceneName in sceneNames)
            {
                scenes.Add(new EditorBuildSettingsScene(sceneName, true));
            }

            EditorBuildSettings.scenes = scenes.ToArray();

            Debug.Log("Set scenes of build settings to all scenes.");
        }
    }
}