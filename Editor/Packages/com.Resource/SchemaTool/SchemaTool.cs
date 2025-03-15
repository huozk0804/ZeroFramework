using System.Collections.Generic;
using UnityEditor;
using YooAsset.Editor;

namespace ZeroFramework.Editor
{

	public class SchemaTools
	{
		/// <summary>
		/// ͨ��ɨ���ݷ���
		/// </summary>
		public static List<ReportElement> ScanAssets (string[] scanAssetList, System.Func<string, ReportElement> scanFun, int unloadAssetLimit = int.MaxValue) {
			int scanNumber = 0;
			int progressCount = 0;
			int totalCount = scanAssetList.Length;
			List<ReportElement> results = new List<ReportElement>(totalCount);

			EditorTools.ClearProgressBar();
			foreach (string assetPath in scanAssetList) {
				scanNumber++;
				progressCount++;
				EditorTools.DisplayProgressBar("ɨ����...", progressCount, totalCount);
				var scanResult = scanFun.Invoke(assetPath);
				if (scanResult != null)
					results.Add(scanResult);

				// �ͷű༭��δʹ�õ���Դ
				if (scanNumber >= unloadAssetLimit) {
					scanNumber = 0;
					EditorUtility.UnloadUnusedAssetsImmediate(true);
				}
			}
			EditorTools.ClearProgressBar();

			return results;
		}

		/// <summary>
		/// ͨ���޸���ݷ���
		/// </summary>
		public static void FixAssets (List<ReportElement> fixAssetList, System.Action<ReportElement> fixFun, int unloadAssetLimit = int.MaxValue) {
			int scanNumber = 0;
			int totalCount = fixAssetList.Count;
			int progressCount = 0;
			EditorTools.ClearProgressBar();
			foreach (var scanResult in fixAssetList) {
				scanNumber++;
				progressCount++;
				EditorTools.DisplayProgressBar("�޸���...", progressCount, totalCount);
				fixFun.Invoke(scanResult);

				// �ͷű༭��δʹ�õ���Դ
				if (scanNumber >= unloadAssetLimit) {
					scanNumber = 0;
					EditorUtility.UnloadUnusedAssetsImmediate(true);
				}
			}
			EditorTools.ClearProgressBar();
		}
	}
}