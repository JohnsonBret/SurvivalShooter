using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace CompleteProject
{
	public class PlayerShootingFlame : MonoBehaviour
	{
		public int damagePerShot = 1;                  // The damage inflicted by each bullet.
		public int damageRate;

		int shootableMask;								// A layer mask so the raycast only hits things on the shootable layer.
		[SerializeField]
		private ParticleSystem flameParticles;          // Reference to the particle system.
		[SerializeField]
		private CapsuleCollider flameCollider;
		[SerializeField]
		private ParticleSystem ignitionFlame;
		Light flameLight;                                 // Reference to the light component.
		public Light faceLight;							// Duh
		float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.


		void Awake ()
		{
			// Create a layer mask for the Shootable layer.
			shootableMask = LayerMask.GetMask ("Shootable");
			flameLight = GetComponent<Light> ();
		}

		void OnEnable()
		{
			ignitionFlame.Stop ();
			ignitionFlame.Play ();
		}

		void OnDisable()
		{
			ignitionFlame.Stop ();
		}

		void Update ()
		{

			if(ShootButtonIsDown() && Time.timeScale != 0){
				StartFlame ();
			}

			if (ShootButtonIsUp () && Time.timeScale != 0) {
				StopFlame ();
			}
		}

		private bool ShootButtonIsDown()
		{
			return Input.GetButtonDown ("Fire1");
		}

		private bool ShootButtonIsUp()
		{
			return Input.GetButtonUp ("Fire1");
		}


		private void StartFlame ()
		{

			flameCollider.enabled = true;

			// Enable the lights.
			flameLight.enabled = true;
			faceLight.enabled = true;


			// Stop the particles from playing if they were, then start the particles.
			flameParticles.Stop ();
			flameParticles.Play ();

		}

		private void StopFlame()
		{
			DisableEffects ();
			flameCollider.enabled = false;
		}

		public void DisableEffects ()
		{
			
			faceLight.enabled = false;
			flameLight.enabled = false;
			flameParticles.Stop ();
		}

		void OnTriggerStay(Collider col)
		{
			if (Time.frameCount % damageRate == 0) {
				// Try and find an EnemyHealth script on the gameobject hit.
				EnemyHealth enemyHealth = col.gameObject.GetComponent <EnemyHealth> ();

				// If the EnemyHealth component exist...
				if (enemyHealth != null) {
					// ... the enemy should take damage.
					enemyHealth.TakeDamage (damagePerShot, col.gameObject.transform.position);
					enemyHealth.SetFire ();
				}
			}

		}
	}
}