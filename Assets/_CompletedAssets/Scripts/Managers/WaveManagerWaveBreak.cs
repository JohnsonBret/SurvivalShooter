using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CompleteProject
{
	public class WaveManagerWaveBreak : WaveManagerBaseState {

		public float restTime = 0;
		private float restTimeRemaining = 0;

		public override void OnStateEnter()
		{
			base.OnStateEnter();
			InitializeRestTimeRemaining ();
		}

		private void InitializeRestTimeRemaining()
		{
			restTimeRemaining = restTime;
		}

		public override void Execute() 
		{
			UpdateWaveBreak();
		}

		private void UpdateWaveBreak()
		{
			CountdownRestTime ();

			if (RestTimeHasElapsed ()) {
				waveManager.ChangeState ();
			}
		}

		private void CountdownRestTime()
		{
			restTimeRemaining -= Time.deltaTime;
		}

		private bool RestTimeHasElapsed()
		{
			return restTimeRemaining <= 0;
		}

		public override void OnStateExit()
		{
			
		}
	}
}
