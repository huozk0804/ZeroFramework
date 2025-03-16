using System;
using UnityEditor;
using UnityEngine;
using YooAsset;
using YooAsset.Editor;
using BuildResult = YooAsset.Editor.BuildResult;

namespace ZeroFramework.Editor
{
	public static class YooAssetBuildHelper
	{
		/// <summary>
		/// 构建资源包
		/// </summary>
		public static PackageInvokeBuildResult BuildPackage (PackageInvokeBuildParam buildParam, BuildTarget buildTarget, string version, bool enableLog) {
			string packageName = buildParam.PackageName;
			string buildPipelineName = buildParam.BuildPipelineName;
			BuildResult buildResult;

			if (buildPipelineName == EBuildPipeline.EditorSimulateBuildPipeline.ToString()) {
				var buildParameters = new EditorSimulateBuildParameters {
					BuildOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot(),
					BuildinFileRoot = AssetBundleBuilderHelper.GetStreamingAssetsRoot(),
					BuildPipeline = EBuildPipeline.EditorSimulateBuildPipeline.ToString(),
					BuildBundleType = (int)0,
					BuildTarget = buildTarget,
					PackageName = packageName,
					PackageVersion = version,
					FileNameStyle = EFileNameStyle.HashName,
					BuildinFileCopyOption = EBuildinFileCopyOption.None,
					BuildinFileCopyParams = string.Empty
				};

				var pipeline = new EditorSimulateBuildPipeline();
				buildResult = pipeline.Run(buildParameters, enableLog);
			} else if (buildPipelineName == EBuildPipeline.ScriptableBuildPipeline.ToString()) {
				// 内置着色器资源包名称
				var builtinShaderBundleName = GetBuiltinShaderBundleName(packageName);
				var buildParameters = new ScriptableBuildParameters {
					BuildOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot(),
					BuildinFileRoot = AssetBundleBuilderHelper.GetStreamingAssetsRoot(),
					BuildPipeline = EBuildPipeline.ScriptableBuildPipeline.ToString(),
					BuildBundleType = (int)1,
					BuildTarget = buildTarget,
					PackageName = packageName,
					PackageVersion = version,
					EnableSharePackRule = true,
					VerifyBuildingResult = true,
					FileNameStyle = EFileNameStyle.HashName,
					BuildinFileCopyOption = EBuildinFileCopyOption.None,
					BuildinFileCopyParams = string.Empty,
					CompressOption = ECompressOption.LZ4,
					ClearBuildCacheFiles = false,
					UseAssetDependencyDB = true,
					BuiltinShadersBundleName = builtinShaderBundleName
				};

				var pipeline = new ScriptableBuildPipeline();
				buildResult = pipeline.Run(buildParameters, enableLog);

			} else if (buildPipelineName == EBuildPipeline.BuiltinBuildPipeline.ToString()) {
				var buildParameters = new BuiltinBuildParameters {
					BuildOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot(),
					BuildinFileRoot = AssetBundleBuilderHelper.GetStreamingAssetsRoot(),
					BuildPipeline = EBuildPipeline.ScriptableBuildPipeline.ToString(),
					BuildBundleType = (int)1,
					BuildTarget = buildTarget,
					PackageName = packageName,
					PackageVersion = version,
					EnableSharePackRule = true,
					VerifyBuildingResult = true,
					FileNameStyle = EFileNameStyle.HashName,
					BuildinFileCopyOption = EBuildinFileCopyOption.None,
					BuildinFileCopyParams = string.Empty,
					CompressOption = ECompressOption.LZ4,
					ClearBuildCacheFiles = false,
					UseAssetDependencyDB = true
				};

				var pipeline = new BuiltinBuildPipeline();
				buildResult = pipeline.Run(buildParameters, enableLog);
			} else if (buildPipelineName == EBuildPipeline.RawFileBuildPipeline.ToString()) {
				var buildParameters = new RawFileBuildParameters {
					BuildOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot(),
					BuildinFileRoot = AssetBundleBuilderHelper.GetStreamingAssetsRoot(),
					BuildPipeline = EBuildPipeline.RawFileBuildPipeline.ToString(),
					BuildBundleType = (int)1,
					BuildTarget = buildTarget,
					PackageName = packageName,
					PackageVersion = version,
					VerifyBuildingResult = true,
					FileNameStyle = EFileNameStyle.HashName,
					BuildinFileCopyOption = EBuildinFileCopyOption.None,
					BuildinFileCopyParams = string.Empty,
					ClearBuildCacheFiles = true,
					UseAssetDependencyDB = true
				};

				var pipeline = new RawFileBuildPipeline();
				buildResult = pipeline.Run(buildParameters, enableLog);
			} else {
				throw new NotImplementedException(buildPipelineName);
			}

			if (buildResult.Success) {
				var reulst = new PackageInvokeBuildResult();
				reulst.PackageRootDirectory = buildResult.OutputPackageDirectory;
				return reulst;
			} else {
				Debug.LogError(buildResult.ErrorInfo);
				throw new Exception($"{buildPipelineName} build failed !");
			}
		}

		/// <summary>
		/// 内置着色器资源包名称
		/// 注意：和自动收集的着色器资源包名保持一致！
		/// </summary>
		private static string GetBuiltinShaderBundleName (string packageName) {
			var uniqueBundleName = AssetBundleCollectorSettingData.Setting.UniqueBundleName;
			var packRuleResult = DefaultPackRule.CreateShadersPackRuleResult();
			return packRuleResult.GetBundleName(packageName, uniqueBundleName);
		}


		/// <summary>
		/// shader变体收集器工具
		/// </summary>
		/// <exception cref="Exception"></exception>
		public static void CollectSVC () {
			// string savePath = ShaderVariantCollectorSettingData.Setting.SavePath;
			// System.Action completedCallback = () => {
			// 	ShaderVariantCollection collection =
			// 		AssetDatabase.LoadAssetAtPath<ShaderVariantCollection>(savePath);
			// 	if (collection != null) {
			// 		Debug.Log($"ShaderCount : {collection.shaderCount}");
			// 		Debug.Log($"VariantCount : {collection.variantCount}");
			// 	} else {
			// 		throw new Exception("Failed to Collect shader Variants.");
			// 	}
			//
			// 	EditorTools.CloseUnityGameWindow();
			// 	EditorApplication.Exit(0);
			// };
			//
			// ShaderVariantCollector.Run(savePath, completedCallback);
		}
	}
}