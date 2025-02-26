using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ZeroFramework.Resource
{
    public interface IResourceHelper
    {
        T LoadAsset<T>(string location, string packageName = "") where T : UnityEngine.Object;
		T LoadSubAsset<T> (string location, string subName, string packageName = "") where T : UnityEngine.Object;
		Object[] LoadSubAssets(string location, string packageName = "");
        byte[] LoadRawFile(string location, string packageName = "");
        UniTask LoadAssetAsync<T>(string location, string packageName = "") where T : UnityEngine.Object;
        UniTask LoadSubAssetAsync<T>(string location,string subName,string packageName ="") where T : UnityEngine.Object;
        UniTask LoadSubAssetsAsync(string location, string packageName = "");
        UniTask LoadSceneAsync(string location, string packageName = "");
        UniTask LoadRawFileAsync(string location, string packageName = "");
    }
}