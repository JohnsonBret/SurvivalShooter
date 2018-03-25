using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompleteProject
{
	public class WaveManagerCountdownCleanup : WaveManagerBaseState {

		public float cleanupTime = 0;
		private float cleanupTimeRemaining = 0;

		[SerializeField]
		private Animator waveCompleteTextAnimator;

		public override void OnStateEnter()
		{
			base.OnStateEnter();
			InitializeCleanupTimeRemaining ();
			SubscribeToEvents ();
		}

		private void InitializeCleanupTimeRemaining()
		{
			cleanupTimeRemaining = cleanupTime;
		}

		private void SubscribeToEvents()
		{
			EnemyHealth.OnEnemyDeath += HandleEnemyDeath;
		}

		private void HandleEnemyDeath()
		{
			
		}

		public override void Execute() 
		{
			UpdateCountdownCleanup();
		}

		private void UpdateCountdownCleanup()
		{
			CountdownRestTime ();

			if (CleanupTimeHasElapsed ()) {
				waveManager.ChangeState ();
			}
		}

		private void CountdownRestTime()
		{
			cleanupTimeRemaining -= Time.deltaTime;
		}

		private bool CleanupTimeHasElapsed()
		{
			return cleanupTimeRemaining <= 0;
		}

		public override void OnStateExit()
		{
			KillAllRemainingEnemies();
			UnSubscribeToEvents();
			DisplayWaveCompleteUiText();
		}

		private void KillAllRemainingEnemies()
		{
			GameObject[] remainingEnemies = FindAllRemainingEnemies();
			foreach (GameObject enemy in remainingEnemies) {
				int remainingHealth = enemy.GetComponent<EnemyHealth> ().currentHealth;
				enemy.GetComponent<EnemyHealth> ().TakeDamage (remainingHealth, enemy.transform.position);
			}
		}

		private GameObject[] FindAllRemainingEnemies()
		{
			return GameObject.FindGameObjectsWithTag ("Enemy");
		}

		private void UnSubscribeToEvents()
		{
			EnemyHealth.OnEnemyDeath -= HandleEnemyDeath;
		}

		private void DisplayWaveCompleteUiText()
		{
			waveCompleteTextAnimator.Play ("WaveComplete");
		}
	}
}