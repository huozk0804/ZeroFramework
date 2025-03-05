namespace ZeroFramework
{
	/// <summary>
	/// ������Դ״̬��
	/// </summary>
	internal enum LoadResourceStatus : byte
	{
		/// <summary>
		/// ������Դ��ɡ�
		/// </summary>
		Success = 0,

		/// <summary>
		/// ��Դ�����ڡ�
		/// </summary>
		NotExist,

		/// <summary>
		/// ��Դ��δ׼����ϡ�
		/// </summary>
		NotReady,

		/// <summary>
		/// ������Դ����
		/// </summary>
		DependencyError,

		/// <summary>
		/// ��Դ���ʹ���
		/// </summary>
		TypeError,

		/// <summary>
		/// ������Դ����
		/// </summary>
		AssetError
	}
}