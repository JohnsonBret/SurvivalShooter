using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompleteProject
{
	public class WaveManager : MonoBehaviour {

		public int waveNumber = 0;
		public int NumberOfWavesToBoss;

		[SerializeField]
		private WaveManagerBaseState currentState;
		[SerializeField]
		private WaveManagerBaseState[] waveManagerStates;
		[SerializeField]
		private WaveManagerBaseState bossState;

		private int currentStateArrayIndex = 0;

		void OnEnable(){
			PlayerHealth.OnGameOver += HandleGameOver;
		}

		void OnDisable(){
			PlayerHealth.OnGameOver -= HandleGameOver;
		}

		private void HandleGameOver(){
		
		}

		void Start () 
		{
			StartWaveManager ();
		}

		void Update () 
		{
			currentState.Execute();
		}

		public void StartWaveManager()
		{
			currentState.OnStateEnter();
		}

		public void ChangeState()
		{
			currentState.OnStateExit();

			currentState = GetNextState();
			currentState.OnStateEnter();
		}

		private WaveManagerBaseState GetNextState()
		{
			if (NextWaveShouldBeBoss()) {
				return BossEnemyWave();
			} 
			return NormalEnemyWave();
		}

		private bool NextWaveShouldBeBoss()
		{
			return waveNumber % NumberOfWavesToBoss == 0 && currentStateArrayIndex == waveManagerStates.Length - 1;
		}

		private WaveManagerBaseState BossEnemyWave()
		{
			return bossState;
		}

		private WaveManagerBaseState NormalEnemyWave()
		{
			currentStateArrayIndex++;

			if (currentStateArrayIndex > waveManagerStates.Length - 1) {
				currentStateArrayIndex = 0;
				return waveManagerStates [currentStateArrayIndex];
			}

			return waveManagerStates [currentStateArrayIndex];
		}
	}
}
