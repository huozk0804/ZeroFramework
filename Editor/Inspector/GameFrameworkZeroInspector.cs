using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using ZeroFramework.Config;
using ZeroFramework.Download;
using ZeroFramework.Entity;
using ZeroFramework.Localization;
using ZeroFramework.Scenes;
using ZeroFramework.Setting;
using ZeroFramework.Sound;
using ZeroFramework.UI;
using ZeroFramework;

namespace ZeroFramework.Editor
{
    [CustomEditor(typeof(Zero))]
    public sealed class GameFrameworkZeroInspector : GameFrameworkInspector
    {
        private SerializedProperty m_IsInitialize = null;

        private readonly Dictionary<string, List<ReferencePoolInfo>> m_ReferencePoolInfos =
            new Dictionary<string, List<ReferencePoolInfo>>(StringComparer.Ordinal);

        private readonly HashSet<string> m_OpenedItems = new HashSet<string>();

        private void OnEnable()
        {
            m_IsInitialize = serializedObject.FindProperty("_isInitialize");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            Zero t = (Zero)target;
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.PropertyField(m_IsInitialize);

                //Config
                if (Zero.Instance.HasModule<IConfigManager>())
                {
                    EditorGUILayout.LabelField("Config", EditorStyles.boldLabel);
                    var manager = Zero.Instance.GetModule<IConfigManager>();
                    EditorGUILayout.LabelField("Config Count", manager.Count.ToString());
                    EditorGUILayout.LabelField("Cached Bytes Size", manager.CachedBytesSize.ToString());
                }

                //DataNode
                if (Zero.Instance.HasModule<IDataNodeManager>())
                {
                    EditorGUILayout.Space(10);
                    EditorGUILayout.LabelField("DataNode", EditorStyles.boldLabel);
                    var manager = Zero.Instance.GetModule<IDataNodeManager>();
                    DrawDataNode(manager.Root);
                }

                //Download
                if (Zero.Instance.HasModule<IDownloadManager>())
                {
                    EditorGUILayout.Space(10);
                    EditorGUILayout.LabelField("Download", EditorStyles.boldLabel);
                    var manager = Zero.Instance.Download;
                    EditorGUILayout.LabelField("Paused", manager.Paused.ToString());
                    EditorGUILayout.LabelField("Total Agent Count", manager.TotalAgentCount.ToString());
                    EditorGUILayout.LabelField("Free Agent Count", manager.FreeAgentCount.ToString());
                    EditorGUILayout.LabelField("Working Agent Count", manager.WorkingAgentCount.ToString());
                    EditorGUILayout.LabelField("Waiting Agent Count", manager.WaitingTaskCount.ToString());
                    EditorGUILayout.LabelField("Current Speed", manager.CurrentSpeed.ToString());
                    EditorGUILayout.BeginVertical("box");
                    {
                        TaskInfo[] downloadInfos = manager.GetAllDownloadInfos();
                        if (downloadInfos.Length > 0)
                        {
                            foreach (TaskInfo downloadInfo in downloadInfos)
                            {
                                DrawDownloadInfo(downloadInfo);
                            }

                            if (GUILayout.Button("Export CSV Data"))
                            {
                                string exportFileName = EditorUtility.SaveFilePanel("Export CSV Data", string.Empty,
                                    "Download Task Data.csv", string.Empty);
                                if (!string.IsNullOrEmpty(exportFileName))
                                {
                                    try
                                    {
                                        int index = 0;
                                        string[] data = new string[downloadInfos.Length + 1];
                                        data[index++] = "Download Path,Serial Id,Tag,Priority,Status";
                                        foreach (TaskInfo downloadInfo in downloadInfos)
                                        {
                                            data[index++] = Utility.Text.Format("{0},{1},{2},{3},{4}",
                                                downloadInfo.Description, downloadInfo.SerialId,
                                                downloadInfo.Tag ?? string.Empty, downloadInfo.Priority,
                                                downloadInfo.Status);
                                        }

                                        File.WriteAllLines(exportFileName, data, Encoding.UTF8);
                                        Debug.Log(Utility.Text.Format("Export download task CSV data to '{0}' success.",
                                            exportFileName));
                                    }
                                    catch (Exception exception)
                                    {
                                        Debug.LogError(Utility.Text.Format(
                                            "Export download task CSV data to '{0}' failure, exception is '{1}'.",
                                            exportFileName, exception));
                                    }
                                }
                            }
                        }
                        else
                        {
                            GUILayout.Label("Download Task is Empty ...");
                        }
                    }
                    EditorGUILayout.EndVertical();
                }

                //Entity
                if (Zero.Instance.HasModule<IEntityManager>())
                {
                    EditorGUILayout.Space(10);
                    EditorGUILayout.LabelField("Entity", EditorStyles.boldLabel);
                    var manager = Zero.Instance.GetModule<IEntityManager>();
                    EditorGUILayout.LabelField("Entity Group Count", manager.EntityGroupCount.ToString());
                    EditorGUILayout.LabelField("Entity Count (Total)", manager.EntityCount.ToString());
                    IEntityGroup[] entityGroups = manager.GetAllEntityGroups();
                    foreach (IEntityGroup entityGroup in entityGroups)
                    {
                        EditorGUILayout.LabelField(Utility.Text.Format("Entity Count ({0})", entityGroup.Name),
                            entityGroup.EntityCount.ToString());
                    }
                }

                //Localization
                if (Zero.Instance.HasModule<ILocalizationManager>())
                {
                    EditorGUILayout.Space(10);
                    EditorGUILayout.LabelField("Localization", EditorStyles.boldLabel);
                    var manager = Zero.Instance.GetModule<ILocalizationManager>();
                    EditorGUILayout.LabelField("Language", manager.Language.ToString());
                    EditorGUILayout.LabelField("System Language", manager.SystemLanguage.ToString());
                    EditorGUILayout.LabelField("Dictionary Count", manager.DictionaryCount.ToString());
                    EditorGUILayout.LabelField("Cached Bytes Size", manager.CachedBytesSize.ToString());
                }

                //ReferencePool
                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField("Reference Pool", EditorStyles.boldLabel);
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("Reference Pool Count", ReferencePool.Count.ToString());
                    m_ReferencePoolInfos.Clear();
                    ReferencePoolInfo[] referencePoolInfos = ReferencePool.GetAllReferencePoolInfos();
                    foreach (ReferencePoolInfo referencePoolInfo in referencePoolInfos)
                    {
                        string assemblyName = referencePoolInfo.Type.Assembly.GetName().Name;
                        if (!m_ReferencePoolInfos.TryGetValue(assemblyName, out var results))
                        {
                            results = new List<ReferencePoolInfo>();
                            m_ReferencePoolInfos.Add(assemblyName, results);
                        }

                        results.Add(referencePoolInfo);
                    }

                    foreach (var assemblyReferencePoolInfo in m_ReferencePoolInfos)
                    {
                        bool lastState = m_OpenedItems.Contains(assemblyReferencePoolInfo.Key);
                        bool currentState = EditorGUILayout.Foldout(lastState, assemblyReferencePoolInfo.Key);
                        if (currentState != lastState)
                        {
                            if (currentState)
                            {
                                m_OpenedItems.Add(assemblyReferencePoolInfo.Key);
                            }
                            else
                            {
                                m_OpenedItems.Remove(assemblyReferencePoolInfo.Key);
                            }
                        }

                        if (currentState)
                        {
                            EditorGUILayout.BeginVertical("box");
                            {
                                EditorGUILayout.LabelField("Class Name",
                                    "Unused\tUsing\tAcquire\tRelease\tAdd\tRemove");
                                assemblyReferencePoolInfo.Value.Sort(ReferenceComparison);
                                foreach (ReferencePoolInfo referencePoolInfo in assemblyReferencePoolInfo.Value)
                                {
                                    DrawReferencePoolInfo(referencePoolInfo);
                                }

                                if (GUILayout.Button("Export CSV Data"))
                                {
                                    string exportFileName = EditorUtility.SaveFilePanel("Export CSV Data", string.Empty,
                                        Utility.Text.Format("Reference Pool Data - {0}.csv",
                                            assemblyReferencePoolInfo.Key), string.Empty);
                                    if (!string.IsNullOrEmpty(exportFileName))
                                    {
                                        try
                                        {
                                            int index = 0;
                                            string[] data = new string[assemblyReferencePoolInfo.Value.Count + 1];
                                            data[index++] =
                                                "Class Name,Full Class Name,Unused,Using,Acquire,Release,Add,Remove";
                                            foreach (ReferencePoolInfo referencePoolInfo in assemblyReferencePoolInfo
                                                         .Value)
                                            {
                                                data[index++] = Utility.Text.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                                                    referencePoolInfo.Type.Name, referencePoolInfo.Type.FullName,
                                                    referencePoolInfo.UnusedReferenceCount,
                                                    referencePoolInfo.UsingReferenceCount,
                                                    referencePoolInfo.AcquireReferenceCount,
                                                    referencePoolInfo.ReleaseReferenceCount,
                                                    referencePoolInfo.AddReferenceCount,
                                                    referencePoolInfo.RemoveReferenceCount);
                                            }

                                            File.WriteAllLines(exportFileName, data, Encoding.UTF8);
                                            Debug.Log(Utility.Text.Format(
                                                "Export reference pool CSV data to '{0}' success.", exportFileName));
                                        }
                                        catch (Exception exception)
                                        {
                                            Debug.LogError(Utility.Text.Format(
                                                "Export reference pool CSV data to '{0}' failure, exception is '{1}'.",
                                                exportFileName, exception));
                                        }
                                    }
                                }
                            }
                            EditorGUILayout.EndVertical();
                        }
                    }
                }
                EditorGUILayout.EndVertical();

                //Scene
                if (Zero.Instance.HasModule<IScenesManager>())
                {
                    EditorGUILayout.Space(10);
                    EditorGUILayout.LabelField("Scene", EditorStyles.boldLabel);
                    var manager = Zero.Instance.GetModule<IScenesManager>();
                    EditorGUILayout.LabelField("Loaded Scene Asset Names",
                        GetSceneNameString(manager.GetLoadedSceneAssetNames()));
                    EditorGUILayout.LabelField("Loading Scene Asset Names",
                        GetSceneNameString(manager.GetLoadingSceneAssetNames()));
                    EditorGUILayout.LabelField("Unloading Scene Asset Names",
                        GetSceneNameString(manager.GetUnloadingSceneAssetNames()));
                    // EditorGUILayout.ObjectField("Main Camera", manager.MainCamera, typeof(Camera), true);
                }

                //Setting
                if (Zero.Instance.HasModule<ISettingManager>())
                {
                    EditorGUILayout.Space(10);
                    EditorGUILayout.LabelField("Setting", EditorStyles.boldLabel);
                    var manager = Zero.Instance.GetModule<ISettingManager>();
                    EditorGUILayout.BeginVertical("box");
                    {
                        EditorGUILayout.LabelField("Setting Count",
                            manager.Count >= 0 ? manager.Count.ToString() : "<Unknown>");
                        if (manager.Count > 0)
                        {
                            string[] settingNames = manager.GetAllSettingNames();
                            foreach (string settingName in settingNames)
                            {
                                EditorGUILayout.LabelField(settingName, manager.GetString(settingName));
                            }
                        }

                        EditorGUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button("Save Settings"))
                            {
                                manager.Save();
                            }

                            if (GUILayout.Button("Remove All Settings"))
                            {
                                manager.RemoveAllSettings();
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndVertical();
                }

                //Sound
                if (Zero.Instance.HasModule<ISoundManager>())
                {
                    EditorGUILayout.Space(10);
                    EditorGUILayout.LabelField("Sound", EditorStyles.boldLabel);
                    var manager = Zero.Instance.GetModule<ISoundManager>();
                    EditorGUILayout.LabelField("Sound Group Count", manager.SoundGroupCount.ToString());
                }

                //UI
                if (Zero.Instance.HasModule<IUIManager>())
                {
                    EditorGUILayout.Space(10);
                    EditorGUILayout.LabelField("UI", EditorStyles.boldLabel);
                    var manager = Zero.Instance.GetModule<IUIManager>();
                    EditorGUILayout.LabelField("UI Group Count", manager.UIGroupCount.ToString());
                }

                //WebRequest
                if (Zero.Instance.HasModule<IWebRequestManager>())
                {
                    EditorGUILayout.Space(10);
                    EditorGUILayout.LabelField("WebRequest", EditorStyles.boldLabel);
                    var manager = Zero.Instance.GetModule<IWebRequestManager>();
                    EditorGUILayout.LabelField("Total Agent Count", manager.TotalAgentCount.ToString());
                    EditorGUILayout.LabelField("Free Agent Count", manager.FreeAgentCount.ToString());
                    EditorGUILayout.LabelField("Working Agent Count", manager.WorkingAgentCount.ToString());
                    EditorGUILayout.LabelField("Waiting Agent Count", manager.WaitingTaskCount.ToString());
                    EditorGUILayout.BeginVertical("box");
                    {
                        TaskInfo[] webRequestInfos = manager.GetAllWebRequestInfos();
                        if (webRequestInfos.Length > 0)
                        {
                            foreach (TaskInfo webRequestInfo in webRequestInfos)
                            {
                                DrawWebRequestInfo(webRequestInfo);
                            }

                            if (GUILayout.Button("Export CSV Data"))
                            {
                                string exportFileName = EditorUtility.SaveFilePanel("Export CSV Data", string.Empty,
                                    "WebRequest Task Data.csv", string.Empty);
                                if (!string.IsNullOrEmpty(exportFileName))
                                {
                                    try
                                    {
                                        int index = 0;
                                        string[] data = new string[webRequestInfos.Length + 1];
                                        data[index++] = "WebRequest Uri,Serial Id,Tag,Priority,Status";
                                        foreach (TaskInfo webRequestInfo in webRequestInfos)
                                        {
                                            data[index++] = Utility.Text.Format("{0},{1},{2},{3},{4}",
                                                webRequestInfo.Description, webRequestInfo.SerialId,
                                                webRequestInfo.Tag ?? string.Empty, webRequestInfo.Priority,
                                                webRequestInfo.Status);
                                        }

                                        File.WriteAllLines(exportFileName, data, Encoding.UTF8);
                                        Debug.Log(Utility.Text.Format(
                                            "Export web request task CSV data to '{0}' success.", exportFileName));
                                    }
                                    catch (Exception exception)
                                    {
                                        Debug.LogError(Utility.Text.Format(
                                            "Export web request task CSV data to '{0}' failure, exception is '{1}'.",
                                            exportFileName, exception));
                                    }
                                }
                            }
                        }
                        else
                        {
                            GUILayout.Label("WebRequest Task is Empty ...");
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
            }

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        private void DrawDataNode(IDataNode dataNode)
        {
            EditorGUILayout.LabelField(dataNode.FullName, dataNode.ToDataString());
            IDataNode[] child = dataNode.GetAllChild();
            foreach (IDataNode c in child)
            {
                DrawDataNode(c);
            }
        }

        private void DrawDownloadInfo(TaskInfo downloadInfo)
        {
            EditorGUILayout.LabelField(downloadInfo.Description,
                Utility.Text.Format("[SerialId]{0} [Tag]{1} [Priority]{2} [Status]{3}", downloadInfo.SerialId,
                    downloadInfo.Tag ?? "<None>", downloadInfo.Priority, downloadInfo.Status));
        }

        private void DrawReferencePoolInfo(ReferencePoolInfo referencePoolInfo)
        {
            EditorGUILayout.LabelField(referencePoolInfo.Type.FullName,
                Utility.Text.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", referencePoolInfo.UnusedReferenceCount,
                    referencePoolInfo.UsingReferenceCount, referencePoolInfo.AcquireReferenceCount,
                    referencePoolInfo.ReleaseReferenceCount, referencePoolInfo.AddReferenceCount,
                    referencePoolInfo.RemoveReferenceCount));
        }

        private int ReferenceComparison(ReferencePoolInfo a, ReferencePoolInfo b)
        {
            return a.Type.FullName.CompareTo(b.Type.FullName);
        }

        private string GetSceneNameString(string[] sceneAssetNames)
        {
            if (sceneAssetNames == null || sceneAssetNames.Length <= 0)
            {
                return "<Empty>";
            }

            string GetSceneName(string sceneAssetName)
            {
                if (string.IsNullOrEmpty(sceneAssetName))
                {
                    Log.Error("Scene asset name is invalid.");
                    return null;
                }

                int sceneNamePosition = sceneAssetName.LastIndexOf('/');
                if (sceneNamePosition + 1 >= sceneAssetName.Length)
                {
                    Log.Error("Scene asset name '{0}' is invalid.", sceneAssetName);
                    return null;
                }

                string sceneName = sceneAssetName.Substring(sceneNamePosition + 1);
                sceneNamePosition = sceneName.LastIndexOf(".unity");
                if (sceneNamePosition > 0)
                {
                    sceneName = sceneName.Substring(0, sceneNamePosition);
                }

                return sceneName;
            }


            string sceneNameString = string.Empty;
            foreach (string sceneAssetName in sceneAssetNames)
            {
                if (!string.IsNullOrEmpty(sceneNameString))
                {
                    sceneNameString += ", ";
                }

                sceneNameString += GetSceneName(sceneAssetName);
            }

            return sceneNameString;
        }

        private void DrawWebRequestInfo(TaskInfo webRequestInfo)
        {
            EditorGUILayout.LabelField(webRequestInfo.Description,
                Utility.Text.Format("[SerialId]{0} [Tag]{1} [Priority]{2} [Status]{3}", webRequestInfo.SerialId,
                    webRequestInfo.Tag ?? "<None>", webRequestInfo.Priority, webRequestInfo.Status));
        }
    }
}