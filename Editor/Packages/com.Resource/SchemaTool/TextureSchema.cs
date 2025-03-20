#if UNITY_2019_4_OR_NEWER
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using YooAsset.Editor;
using UnityEngine.UIElements;

namespace ZeroFramework.Editor
{

	[CreateAssetMenu(fileName = "TextureSchema", menuName =  EditorConst.BaseAssetPath + "Resource/Create TextureSchema")]
	public class TextureSchema : ScannerSchema
	{
		/// <summary>
		/// ͼƬ�����
		/// </summary>
		public int MaxWidth = 1024;

		/// <summary>
		/// ͼƬ���߶�
		/// </summary>
		public int MaxHeight = 1024;

		/// <summary>
		/// �����б�
		/// </summary>
		public List<string> TestStringValues = new List<string>();


		/// <summary>
		/// ��ȡ�û�ָ����Ϣ
		/// </summary>
		public override string GetUserGuide () {
			return "������ܣ����ͼƬ�ĸ�ʽ���ߴ�";
		}

		/// <summary>
		/// ��������ɨ�豨��
		/// </summary>
		public override ScanReport RunScanner (AssetArtScanner scanner) {
			// ����ɨ�豨��
			string name = "ɨ�����������ʲ�";
			string desc = GetUserGuide();
			var report = new ScanReport(name, desc);
			//report.AddHeader("��Դ·��", 600, 500, 1000).SetStretchable().SetSearchable().SetSortable().SetCounter().SetHeaderType(EHeaderType.AssetPath);
			report.AddHeader("ͼƬ���", 100).SetSortable().SetHeaderType(EHeaderType.LongValue);
			report.AddHeader("ͼƬ�߶�", 100).SetSortable().SetHeaderType(EHeaderType.LongValue);
			//report.AddHeader("�ڴ��С", 120).SetSortable().SetUnits("bytes").SetHeaderType(EHeaderType.LongValue);
			report.AddHeader("ƻ����ʽ", 100);
			report.AddHeader("��׿��ʽ", 100);
			report.AddHeader("������Ϣ", 500).SetStretchable();

			// ��ȡɨ����Դ����
			var searchDirectorys = scanner.Collectors.Select(c => { return c.CollectPath; });
			string[] findAssets = EditorTools.FindAssets(EAssetSearchType.Texture, searchDirectorys.ToArray());

			// ��ʼɨ����Դ����
			var results = SchemaTools.ScanAssets(findAssets, ScanAssetInternal);
			report.ReportElements.AddRange(results);
			return report;
		}
		private ReportElement ScanAssetInternal (string assetPath) {
			var importer = TextureTools.GetAssetImporter(assetPath);
			if (importer == null)
				return null;

			// �����������
			var texture = AssetDatabase.LoadAssetAtPath<Texture>(assetPath);
			var assetGUID = AssetDatabase.AssetPathToGUID(assetPath);
			var iosFormat = TextureTools.GetPlatformIOSFormat(importer);
			var androidFormat = TextureTools.GetPlatformAndroidFormat(importer);
			var memorySize = TextureTools.GetStorageMemorySize(texture);

			// ��ȡ������Ϣ
			string errorInfo = string.Empty;
			{
				// ƻ����ʽ
				if (iosFormat != TextureImporterFormat.ASTC_4x4) {
					errorInfo += " | ";
					errorInfo += "ƻ����ʽ����";
				}

				// ��׿��ʽ
				if (androidFormat != TextureImporterFormat.ASTC_4x4) {
					errorInfo += " | ";
					errorInfo += "��׿��ʽ����";
				}

				// �༶����
				if (importer.isReadable) {
					errorInfo += " | ";
					errorInfo += "�����˿ɶ�д";
				}

				// ��������
				if (texture.width > MaxWidth || texture.height > MaxHeight) {
					errorInfo += " | ";
					errorInfo += "��������";
				}
			}

			// ���ɨ����Ϣ
			ReportElement result = new ReportElement(assetGUID);
			result.AddScanInfo("��Դ·��", assetPath);
			result.AddScanInfo("ͼƬ���", texture.width);
			result.AddScanInfo("ͼƬ�߶�", texture.height);
			result.AddScanInfo("�ڴ��С", memorySize);
			result.AddScanInfo("ƻ����ʽ", iosFormat.ToString());
			result.AddScanInfo("��׿��ʽ", androidFormat.ToString());
			result.AddScanInfo("������Ϣ", errorInfo);

			// �ж��Ƿ�ͨ��
			result.Passes = string.IsNullOrEmpty(errorInfo);
			return result;
		}

		/// <summary>
		/// �޸�ɨ����
		/// </summary>
		public override void FixResult (List<ReportElement> fixList) {
			SchemaTools.FixAssets(fixList, FixAssetInternal);
		}
		private void FixAssetInternal (ReportElement result) {
			var scanInfo = result.GetScanInfo("��Դ·��");
			var assetPath = scanInfo.ScanInfo;
			var importer = TextureTools.GetAssetImporter(assetPath);
			if (importer == null)
				return;

			// ƻ����ʽ
			var iosPlatformSetting = TextureTools.GetPlatformIOSSettings(importer);
			iosPlatformSetting.format = TextureImporterFormat.ASTC_4x4;
			iosPlatformSetting.overridden = true;

			// ��׿��ʽ
			var androidPlatformSetting = TextureTools.GetPlatformAndroidSettings(importer);
			androidPlatformSetting.format = TextureImporterFormat.ASTC_4x4;
			androidPlatformSetting.overridden = true;

			// �ɶ�д
			importer.isReadable = false;

			// ��������
			importer.SetPlatformTextureSettings(iosPlatformSetting);
			importer.SetPlatformTextureSettings(androidPlatformSetting);
			importer.SaveAndReimport();
			Debug.Log($"�޸��� : {assetPath}");
		}

		/// <summary>
		/// ������������
		/// </summary>
		public override SchemaInspector CreateInspector () {
			var container = new VisualElement();

			// ͼƬ�����
			var maxWidthField = new IntegerField();
			maxWidthField.label = "ͼƬ�����";
			maxWidthField.SetValueWithoutNotify(MaxWidth);
			maxWidthField.RegisterValueChangedCallback((evt) => {
				MaxWidth = evt.newValue;
			});
			container.Add(maxWidthField);

			// ͼƬ���߶�
			var maxHeightField = new IntegerField();
			maxHeightField.label = "ͼƬ���߶�";
			maxHeightField.SetValueWithoutNotify(MaxHeight);
			maxHeightField.RegisterValueChangedCallback((evt) => {
				MaxHeight = evt.newValue;
			});
			container.Add(maxHeightField);

			// ���������б�
#if UNITY_2021_3_OR_NEWER
			//ReorderableListView reorderableListView = new ReorderableListView();
			//reorderableListView.SourceData = TestStringValues;
			//reorderableListView.HeaderName = "�����б�";
			//container.Add(reorderableListView);
#endif

			SchemaInspector inspector = new SchemaInspector(container);
			return inspector;
		}
	}
}
#endif