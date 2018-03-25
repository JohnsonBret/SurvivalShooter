using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CompleteProject
{
	public class WaveManagerWaveActiveState : WaveManagerBaseState {

		public float numOfEnemiesToSpawnThisWave;
		public float difficultyFactor;
		[SerializeField]
		private GameObject enemyManager;
		[SerializeField]
		private Text waveNumberText;
		[SerializeField]
		private Animator waveTextAnimator;

		private float numOfEnemiesSpawnedThisWave;
		private float numOfEnemiesKilledThisWave;
		private float thresholdOfSlainEnemiesToClearWave = 0.89f;


		//STATE ENTER
		public override void OnStateEnter()
		{
			base.OnStateEnter();
			SubscribeToEvents();
			InitializeEnemyWaveNumbers ();
			StartWave ();
		}

		private void SubscribeToEvents()
		{
			EnemyHealth.OnEnemyDeath += HandleEnemyDeath;
			EnemyManager.OnEnemySpawn += HandleEnemySpawn;
		}

		private void HandleEnemyDeath(){
			numOfEnemiesKilledThisWave++;
		}

		private void HandleEnemySpawn(){
			numOfEnemiesSpawnedThisWave++;
		}

		private void InitializeEnemyWaveNumbers()
		{
			numOfEnemiesToSpawnThisWave = RampNumberOfEnemiesToSpawnThisWave(); 
			numOfEnemiesSpawnedThisWave = 0;
			numOfEnemiesKilledThisWave = 0;
			waveManager.waveNumber++;
		}

		private int RampNumberOfEnemiesToSpawnThisWave()
		{
			int waveNum = waveManager.waveNumber;
			int rampedNumberOfEnemiesToSpawn = Mathf.RoundToInt((float)numOfEnemiesToSpawnThisWave * (1.0f + (float)waveNum / difficultyFactor));
			return rampedNumberOfEnemiesToSpawn;
		}

		private void StartWave (){
			StartSpawningWaveEnemies ();
			DisplayWaveUiAnimation ();
		}

		private void StartSpawningWaveEnemies(){
			enemyManager.SetActive (true);
		}

		private void DisplayWaveUiAnimation()
		{
			SetUiWaveNumber ();
			waveTextAnimator.Play ("WaveStart");
		}

		private void SetUiWaveNumber()
		{
			waveNumberText.text = waveManager.waveNumber.ToString ();
		}


		//STATE EXECUTE
		public override void Execute() 
		{
			UpdateWave ();
		}

		private void UpdateWave(){
			if (CurrentWaveSpawningIsComplete()) {
				StopSpawningWaveEnemies();
			}

			if (CurrentWaveEnemiesNearlyWipedOut ()) {
				waveManager.ChangeState();
			}
		}

		private bool CurrentWaveSpawningIsComplete(){
			return numOfEnemiesSpawnedThisWave >= numOfEnemiesToSpawnThisWave;
		}

		private void StopSpawningWaveEnemies(){
			enemyManager.SetActive (false);
		}
			
		private bool CurrentWaveEnemiesNearlyWipedOut(){
			float percentageOfEnemiesSlain = numOfEnemiesKilledThisWave / numOfEnemiesToSpawnThisWave;
			return percentageOfEnemiesSlain > thresholdOfSlainEnemiesToClearWave;
		}

		//STATE EXIT
		public override void OnStateExit()
		{
			StopWave ();
			UnSubscribeToEvents();
		}

		private void StopWave(){
			
		}


		private void UnSubscribeToEvents()
		{
			EnemyHealth.OnEnemyDeath -= HandleEnemyDeath;
			EnemyManager.OnEnemySpawn -= HandleEnemySpawn;
		}
	}
}
