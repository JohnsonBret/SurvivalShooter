using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CompleteProject
{
	public class WaveManagerBossWave : WaveManagerBaseState {

		public float numOfBossesToSpawnThisWave;

		[SerializeField]
		private GameObject bossManager;

		[SerializeField]
		private Animator waveBossTextAnimator;

		[SerializeField]
		private GameObject bossHealthUi;

		[SerializeField]
		private Slider bossHealthSlider;


		private float numOfBossesSpawnedThisWave;
		private float numOfBossesKilledThisWave;

		private float endBossWaveDelay;

		private EnemyHealth bossHealth;

		//STATE ENTER
		public override void OnStateEnter()
		{
			base.OnStateEnter();
			SubscribeToEvents();
			InitializeBossWaveNumbers ();

			StartWave ();
		}

		private void SubscribeToEvents()
		{
			EnemyHealth.OnEnemyDeath += HandleBossDeath;
			EnemyManager.OnEnemySpawn += HandleBossSpawn;
			PlayerHealth.OnGameOver += HandleGameOver;
		}

		private void HandleBossDeath(){
			numOfBossesKilledThisWave++;
			DeActivateBossHealthBar ();
		}

		private void HandleBossSpawn(){
			numOfBossesSpawnedThisWave++;
			InitalizeBossHealthUi ();
		}

		private void HandleGameOver(){
			waveManager.ChangeState ();
		}

		private void InitializeBossWaveNumbers()
		{
			numOfBossesSpawnedThisWave = 0;
			numOfBossesKilledThisWave = 0;
			endBossWaveDelay = 3.0f;
			waveManager.waveNumber++;
		}

		private void InitalizeBossHealthUi (){
			bossHealthUi.SetActive(true);
			bossHealth = GameObject.FindGameObjectWithTag("Boss").GetComponent<EnemyHealth>();
		}

		private void StartWave (){
			StartSpawningWaveBosses ();
			DisplayWaveUiAnimation ();
		}

		private void StartSpawningWaveBosses(){
			bossManager.SetActive (true);
		}

		private void DisplayWaveUiAnimation()
		{
			waveBossTextAnimator.Play ("WaveBossStart");
		}
			
		//STATE EXECUTE
		public override void Execute() 
		{
			UpdateWave ();
		}

		private void UpdateWave(){
			if (CurrentWaveSpawningIsComplete()) {
				StopSpawningWaveBosses();
			}

			UpdateBossHealthUi();

			if (CurrentWaveBossesDefeated ()) {
				CountdownBossWaveEndDelay ();

				if (BossEndWaveDelayElapsed ()) {
					waveManager.ChangeState ();
				}
			}
		}

		private bool CurrentWaveSpawningIsComplete(){
			return numOfBossesSpawnedThisWave >= numOfBossesToSpawnThisWave;
		}

		private void StopSpawningWaveBosses(){
			bossManager.SetActive (false);
		}

		private void UpdateBossHealthUi(){
			bossHealthSlider.value = CalculateBossHealthPercentage ();
		}

		private float CalculateBossHealthPercentage(){
			if (bossHealth != null) {
				return (float)bossHealth.currentHealth / (float)bossHealth.startingHealth;
			}
			return 1.0f;
		}

		private bool CurrentWaveBossesDefeated(){
			return numOfBossesKilledThisWave >= numOfBossesToSpawnThisWave;
		}

		private void CountdownBossWaveEndDelay(){
			 endBossWaveDelay -= Time.deltaTime;
		}

		private bool BossEndWaveDelayElapsed(){
			return endBossWaveDelay <= 0.0f;
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
			EnemyHealth.OnEnemyDeath -= HandleBossDeath;
			EnemyManager.OnEnemySpawn -= HandleBossSpawn;
			PlayerHealth.OnGameOver -= HandleGameOver;
		}

		private void DeActivateBossHealthBar()
		{
			bossHealthUi.SetActive (false);
		}
	}
}
