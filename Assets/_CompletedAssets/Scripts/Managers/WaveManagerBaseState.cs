using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CompleteProject
{
	public class WaveManagerBaseState : MonoBehaviour {

		protected WaveManager waveManager;

		public virtual void OnStateEnter()
		{
			waveManager = GetComponent<WaveManager>();
		}

		public virtual void Execute() 
		{

		}

		public virtual void OnStateExit()
		{

		}
	}
}
