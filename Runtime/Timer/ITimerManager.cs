//------------------------------------------------------------
// Zero Framework
// Copyright © 2025-2026 All rights reserved.
// Feedback: https://github.com/huozk0804/ZeroFramework
//------------------------------------------------------------

namespace ZeroFramework
{
	public interface ITimerManager
	{
		void ReigisterTimer (Timer timer);
		void CancelAllTimer ();
		void PauseAllTimer ();
		void ResumeAllTimer ();
	}
}