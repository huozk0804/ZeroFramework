using System.Collections.Generic;
using System;
using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using HybridCLR.Editor.Settings;
using UnityEditor;
using System.IO;
using UnityEngine;

namespace ZeroFramework.Editor
{
	public static class HybridCLRBuildHelper
	{
		private const string EnableHybridClrScriptingDefineSymbol = "ENABLE_HYBRIDCLR";

		static HybridCLRBuildHelper () {
#if ENABLE_HYBRIDCLR
        HybridCLR.Editor.SettingsUtil.Enable = true;
#else
			HybridCLR.Editor.SettingsUtil.Enable = false;
#endif
		}

		/// <summary>
		/// 禁用HybridCLR宏定义。
		/// </summary>
		[MenuItem("HybridCLR/Define Symbols/Disable HybridCLR", false, 30)]
		public static void Disable () {
			ScriptingDefineSymbols.RemoveScriptingDefineSymbol(EnableHybridClrScriptingDefineSymbol);
			HybridCLR.Editor.SettingsUtil.Enable = false;
		}

		/// <summary>
		/// 开启HybridCLR宏定义。
		/// </summary>
		[MenuItem("HybridCLR/Define Symbols/Enable HybridCLR", false, 31)]
		public static void Enable () {
			ScriptingDefineSymbols.RemoveScriptingDefineSymbol(EnableHybridClrScriptingDefineSymbol);
			ScriptingDefineSymbols.AddScriptingDefineSymbol(EnableHybridClrScriptingDefineSymbol);
			HybridCLR.Editor.SettingsUtil.Enable = true;
		}

		[MenuItem("HybridCLR/Build/BuildAssets And CopyTo AssemblyTextAssetPath")]
		public static void BuildAndCopyDlls () {
#if ENABLE_HYBRIDCLR
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        CompileDllCommand.CompileDll(target);
        CopyAOTHotUpdateDlls(target);
#endif
		}

		public static void BuildAndCopyDlls (BuildTarget target) {
#if ENABLE_HYBRIDCLR
        CompileDllCommand.CompileDll(target);
        CopyAOTHotUpdateDlls(target);
#endif
		}

		public static void CopyAOTHotUpdateDlls (BuildTarget target) {
			CopyAOTAssembliesToAssetPath();
			CopyHotUpdateAssembliesToAssetPath();
			AssetDatabase.Refresh();
		}

		public static void CopyAOTAssembliesToAssetPath () {
#if ENABLE_HYBRIDCLR
        var target = EditorUserBuildSettings.activeBuildTarget;
        string aotAssembliesSrcDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
        string aotAssembliesDstDir = Application.dataPath +"/"+ SettingsUtils.HybridCLRCustomGlobalSettings.AssemblyTextAssetPath;

        foreach (var dll in SettingsUtils.HybridCLRCustomGlobalSettings.AOTMetaAssemblies)
        {
            string srcDllPath = $"{aotAssembliesSrcDir}/{dll}";
            if (!System.IO.File.Exists(srcDllPath))
            {
                Debug.LogError($"ab中添加AOT补充元数据dll:{srcDllPath} 时发生错误,文件不存在。裁剪后的AOT dll在BuildPlayer时才能生成，因此需要你先构建一次游戏App后再打包。");
                continue;
            }
            string dllBytesPath = $"{aotAssembliesDstDir}/{dll}.bytes";
            System.IO.File.Copy(srcDllPath, dllBytesPath, true);
            Debug.Log($"[CopyAOTAssembliesToStreamingAssets] copy AOT dll {srcDllPath} -> {dllBytesPath}");
        }
#endif
		}

		public static void CopyHotUpdateAssembliesToAssetPath () {
#if ENABLE_HYBRIDCLR
        var target = EditorUserBuildSettings.activeBuildTarget;

        string hotfixDllSrcDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
        string hotfixAssembliesDstDir = Application.dataPath +"/"+ SettingsUtils.HybridCLRCustomGlobalSettings.AssemblyTextAssetPath;
        foreach (var dll in SettingsUtil.HotUpdateAssemblyFilesExcludePreserved)
        {
            string dllPath = $"{hotfixDllSrcDir}/{dll}";
            string dllBytesPath = $"{hotfixAssembliesDstDir}/{dll}.bytes";
            System.IO.File.Copy(dllPath, dllBytesPath, true);
            Debug.Log($"[CopyHotUpdateAssembliesToStreamingAssets] copy hotfix dll {dllPath} -> {dllBytesPath}");
        }
#endif
		}

		private static void BuildHotUpdate (BuildTargetGroup group, BuildTarget target) {
			Console.WriteLine($"[JenkinsBuild] Start building hot update for {Enum.GetName(typeof(BuildTarget), target)}");

			EditorUserBuildSettings.SwitchActiveBuildTarget(group, target);

			// 打开热更新
			HybridCLRSettings.Instance.enable = true;
			HybridCLRSettings.Save();

			try {
				CompileDllCommand.CompileDll(target);
				Il2CppDefGeneratorCommand.GenerateIl2CppDef();

				// 这几个生成依赖HotUpdateDlls
				LinkGeneratorCommand.GenerateLinkXml(target);

				// 生成裁剪后的aot dll
				StripAOTDllCommand.GenerateStripedAOTDlls(target);

				// 桥接函数生成依赖于AOT dll，必须保证已经build过，生成AOT dll
				MethodBridgeGeneratorCommand.GenerateMethodBridgeAndReversePInvokeWrapper(target);
				AOTReferenceGeneratorCommand.GenerateAOTGenericReference(target);
			} catch (Exception e) {
				Console.WriteLine($"[JenkinsBuild] ERROR while building hot update! Message:\n{e.ToString()}");
				return;
			}

			// 复制打出来的DLL并进行替换
			string sourcePath = Path.Combine(
				Application.dataPath,
				$"../{SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target)}"
			);
			string destinationPath = Path.Combine(Application.dataPath, "HotUpdateDLLs");

			if (!Directory.Exists(sourcePath)) {
				Console.WriteLine(
					"[JenkinsBuild] Source directory does not exist! Possibly HybridCLR build failed!"
				);
				return;
			}

			if (!Directory.Exists(destinationPath)) {
				Console.WriteLine(
					"[JenkinsBuild] Destination directory does not exist!"
				);
				Directory.CreateDirectory(destinationPath);
			}

			// string[] dllFiles = Directory.GetFiles(sourcePath, "*.dll");

			// foreach (string dllFile in dllFiles)
			// {
			//     string fileName = Path.GetFileName(dllFile);
			//     string destinationFile = Path.Combine(destinationPath, fileName + ".bytes");
			//     Console.WriteLine($"[JenkinsBuild] Copy: {dllFile}");
			//     File.Copy(dllFile, destinationFile, true);
			// }

			List<string> hotUpdateAssemblyNames = SettingsUtil.HotUpdateAssemblyNamesExcludePreserved;
			for (int i = 0; i < hotUpdateAssemblyNames.Count; i++) {
				Console.WriteLine($"[JenkinsBuild] Copy: {hotUpdateAssemblyNames[i] + ".dll"}");
				File.Copy(sourcePath + "/" + hotUpdateAssemblyNames[i] + ".dll", Path.Combine(destinationPath, hotUpdateAssemblyNames[i] + ".dll.bytes"), true);
			}

			Console.WriteLine("[JenkinsBuild] Hot Update DLLs copied successfully!");

			// 复制打出来的AOT元数据DLL并进行替换
			Console.WriteLine("[JenkinsBuild] Start copying AOT Metadata DLLs!");
			sourcePath = Path.Combine(
				Application.dataPath,
				$"../{SettingsUtil.GetAssembliesPostIl2CppStripDir(target)}"
			);
			destinationPath = Path.Combine(Application.dataPath, "HotUpdateDLLs/AOTMetadata");

			if (!Directory.Exists(sourcePath)) {
				Console.WriteLine(
					"[JenkinsBuild] Source directory does not exist! Possibly HybridCLR build failed!"
				);
				return;
			}

			if (!Directory.Exists(destinationPath)) {
				Console.WriteLine(
					"[JenkinsBuild] Destination directory does not exist!"
				);
				Directory.CreateDirectory(destinationPath);
			}

			// 获取AOTGenericReferences.cs文件的路径
			string aotReferencesFilePath = Path.Combine(
				Application.dataPath,
				SettingsUtil.HybridCLRSettings.outputAOTGenericReferenceFile
			);

			if (!File.Exists(aotReferencesFilePath)) {
				Console.WriteLine(
					"[JenkinsBuild] AOTGenericReferences.cs file does not exist! Abort the build!"
				);
				return;
			}

			// 读取AOTGenericReferences.cs文件内容
			string[] aotReferencesFileContent = File.ReadAllLines(aotReferencesFilePath);

			// 查找PatchedAOTAssemblyList列表
			List<string> patchedAOTAssemblyList = new List<string>();

			for (int i = 0; i < aotReferencesFileContent.Length; i++) {
				if (aotReferencesFileContent[i].Contains("PatchedAOTAssemblyList")) {
					while (!aotReferencesFileContent[i].Contains("};")) {
						if (aotReferencesFileContent[i].Contains("\"")) {
							int startIndex = aotReferencesFileContent[i].IndexOf("\"") + 1;
							int endIndex = aotReferencesFileContent[i].LastIndexOf("\"");
							string dllName = aotReferencesFileContent[i].Substring(
								startIndex,
								endIndex - startIndex
							);
							patchedAOTAssemblyList.Add(dllName);
						}
						i++;
					}
					break;
				}
			}

			// 复制DLL文件到目标文件夹，并添加后缀名".bytes"
			foreach (string dllName in patchedAOTAssemblyList) {
				string sourceFile = Path.Combine(sourcePath, dllName);
				string destinationFile = Path.Combine(
					destinationPath,
					Path.GetFileName(dllName) + ".bytes"
				);

				if (File.Exists(sourceFile)) {
					Console.WriteLine($"[JenkinsBuild] Copy: {sourceFile}");
					File.Copy(sourceFile, destinationFile, true);
					//SetAOTMetadataDllLabel("Assets/HotUpdateDLLs/" + Path.GetFileName(dllName) + ".bytes");
				} else {
					Console.WriteLine("[JenkinsBuild] AOTMetadata DLL file not found: " + dllName);
				}
			}

			AssetDatabase.SaveAssets();

			Console.WriteLine("[JenkinsBuild] BuildHotUpdate complete!");

			AssetDatabase.Refresh();

			// 刷新后开始给DLL加标签
			//SetHotUpdateDllLabel("Assets/HotUpdateDLLs/Assembly-CSharp.dll.bytes");
			for (int i = 0; i < hotUpdateAssemblyNames.Count; i++) {
				//SetHotUpdateDllLabel("Assets/HotUpdateDLLs/" + hotUpdateAssemblyNames[i] + ".dll.bytes");
			}

			foreach (string dllName in patchedAOTAssemblyList) {
				//SetAOTMetadataDllLabel("Assets/HotUpdateDLLs/AOTMetadata/" + Path.GetFileName(dllName) + ".bytes");
			}

			Console.WriteLine("[JenkinsBuild] Start building Addressables!");
			//BuildAddressableContent();
		}

		private static void CopyHotDLLFiles () {

		}

		private static void CopyAotDllFiles () {

		}
	}
}
