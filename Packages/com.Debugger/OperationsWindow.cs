//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2024 All rights reserved.
// Homepage:
// Feedback: mailto:
//------------------------------------------------------------

using UnityEngine;
using ZeroFramework.Resource;

namespace ZeroFramework.Debugger
{
    internal sealed class OperationsWindow : ScrollableDebuggerWindowBase
    {
        protected override void OnDrawScrollableWindow()
        {
            GUILayout.Label("<b>Operations</b>");
            GUILayout.BeginVertical("box");
            {
                var objectPoolComponent = Zero.Instance.ObjectPool;
				if (objectPoolComponent != null)
                {
                    if (GUILayout.Button("Object Pool Release", GUILayout.Height(30f)))
                    {
                        objectPoolComponent.Release();
                    }

                    if (GUILayout.Button("Object Pool Release All Unused", GUILayout.Height(30f)))
                    {
                        objectPoolComponent.ReleaseAllUnused();
                    }
                }

                //TODO: 待处理
                //var resourceCompoent = Zero.Instance.Resource;
                //if (resourceCompoent != null)
                //{
                //    if (GUILayout.Button("Unload Unused Assets", GUILayout.Height(30f)))
                //    {
                //        resourceCompoent.ForceUnloadUnusedAssets(false);
                //    }

                //    if (GUILayout.Button("Unload Unused Assets and Garbage Collect", GUILayout.Height(30f)))
                //    {
                //        resourceCompoent.ForceUnloadUnusedAssets(true);
                //    }
                //}

                if (GUILayout.Button("Shutdown Game Framework (None)", GUILayout.Height(30f)))
                {
                    Zero.Instance.Shutdown(ShutdownType.None);
                }

                if (GUILayout.Button("Shutdown Game Framework (Restart)", GUILayout.Height(30f)))
                {
					Zero.Instance.Shutdown(ShutdownType.Restart);
                }

                if (GUILayout.Button("Shutdown Game Framework (Quit)", GUILayout.Height(30f)))
                {
					Zero.Instance.Shutdown(ShutdownType.Quit);
                }
            }
            GUILayout.EndVertical();
        }
    }
}