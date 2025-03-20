using System.Collections.Generic;

namespace ZeroFramework
{
	public sealed partial class TimerManager : GameFrameworkModule, ITimerManager
	{
		// buffer adding timers so we don't edit a collection during iteration
		private List<Timer> _timersToAdd = new List<Timer>();
		private List<Timer> _timers = new List<Timer>();

		protected internal override int Priority => base.Priority;

		protected internal override void Update (float elapseSeconds, float realElapseSeconds) {
			this.UpdateAllTimers();
		}

		protected internal override void Shutdown () {
			CancelAllTimer();
		}

		public void ReigisterTimer (Timer timer) {
			this._timersToAdd.Add(timer);
		}

		public void CancelAllTimer () {
			foreach (Timer timer in this._timers) {
				timer.Cancel();
			}

			this._timers = new List<Timer>();
			this._timersToAdd = new List<Timer>();
		}

		public void PauseAllTimer () {
			foreach (Timer timer in this._timers) {
				timer.Pause();
			}
		}

		public void ResumeAllTimer () {
			foreach (Timer timer in this._timers) {
				timer.Resume();
			}
		}

		private void UpdateAllTimers () {
			if (this._timersToAdd.Count > 0) {
				this._timers.AddRange(this._timersToAdd);
				this._timersToAdd.Clear();
			}

			foreach (Timer timer in this._timers) {
				timer.Update();
			}

			this._timers.RemoveAll(t => t.isDone);
		}
	}
}