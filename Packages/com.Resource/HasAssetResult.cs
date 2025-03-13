namespace ZeroFramework.Resource
{
	/// <summary>
	/// �����Դ�Ƿ���ڵĽ����
	/// </summary>
	public enum HasAssetResult : byte
	{
		/// <summary>
		/// ��Դ�����ڡ�
		/// </summary>
		NotExist = 0,

		/// <summary>
		/// ��Դ��Ҫ��Զ�˸������ء�
		/// </summary>
		AssetOnline,

		/// <summary>
		/// ������Դ�Ҵ洢�ڴ����ϡ�
		/// </summary>
		AssetOnDisk,

		/// <summary>
		/// ������Դ�Ҵ洢���ļ�ϵͳ�
		/// </summary>
		AssetOnFileSystem,

		/// <summary>
		/// ���ڶ�������Դ�Ҵ洢�ڴ����ϡ�
		/// </summary>
		BinaryOnDisk,

		/// <summary>
		/// ���ڶ�������Դ�Ҵ洢���ļ�ϵͳ�
		/// </summary>
		BinaryOnFileSystem,

		/// <summary>
		/// ��Դ��λ��ַ��Ч��
		/// </summary>
		Valid,
	}
}