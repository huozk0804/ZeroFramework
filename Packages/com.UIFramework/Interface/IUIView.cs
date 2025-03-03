namespace ZeroFramework.UI
{
    public interface IUIView
    {
		/// <summary>
		/// 获取界面序列编号。
		/// </summary>
		int SerialId { get; }

		/// <summary>
		/// 获取界面资源名称。
		/// </summary>
		string UIViewAssetName { get; }

		/// <summary>
		/// 获取界面实例。
		/// </summary>
		object Handle { get; }

		/// <summary>
		/// 父界面
		/// </summary>
		UIPanel Parent { get; }
	}
}