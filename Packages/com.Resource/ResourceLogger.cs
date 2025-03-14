//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework.Resource
{
    internal class ResourceLogger : YooAsset.ILogger
    {
        public void Log(string message)
        {
            ZeroFramework.Log.Info(message);
        }

        public void Warning(string message)
        {
			ZeroFramework.Log.Warning(message);
        }

        public void Error(string message)
        {
			ZeroFramework.Log.Error(message);
        }

        public void Exception(System.Exception exception)
        {
            ZeroFramework.Log.Fatal(exception.Message);
        }
    }
}