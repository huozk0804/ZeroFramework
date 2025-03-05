namespace ZeroFramework
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
		/// ��Դ��δ׼����ϡ�
		/// </summary>
		NotReady,

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
		BinaryOnFileSystem
	}
}