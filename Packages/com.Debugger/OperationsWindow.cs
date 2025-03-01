//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
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
                
                if (GUILayout.Button("Quit Game", GUILayout.Height(30f)))
                {
					Zero.Instance.Shutdown();
                }
            }
            GUILayout.EndVertical();
        }
    }
}