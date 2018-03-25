using UnityEngine;

namespace CompleteProject
{
    public class EnemyHealth : MonoBehaviour
    {
        public int startingHealth = 100;            // The amount of health the enemy starts the game with.
        public int currentHealth;                   // The current health the enemy has.
        public float sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.
        public int scoreValue = 10;                 // The amount added to the player's score when the enemy dies.
		public float healthSpawnChance = 0.0f;		// Percentage Chance to spawn health;

		[SerializeField]
		private GameObject heartPrefab;				// The heart we spawn when the enemy is slain.

		[SerializeField]
		private ParticleSystem flameParticles;

        Animator anim;                              // Reference to the animator.
        ParticleSystem hitParticles;                // Reference to the particle system that plays when the enemy is damaged.
        CapsuleCollider capsuleCollider;            // Reference to the capsule collider.
        bool isDead;                                // Whether the enemy is dead.
        bool isSinking;                             // Whether the enemy has started sinking through the floor.

		public delegate void OnDeathEvent();
		public static event OnDeathEvent OnEnemyDeath;

        void Awake ()
        {
            // Setting up the references.
            anim = GetComponent <Animator> ();
            hitParticles = GetComponentInChildren <ParticleSystem> ();
            capsuleCollider = GetComponent <CapsuleCollider> ();

            // Setting the current health when the enemy first spawns.
            currentHealth = startingHealth;
        }


        void Update ()
        {
            // If the enemy should be sinking...
            if(isSinking)
            {
                // ... move the enemy down by the sinkSpeed per second.
                transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
            }
        }


        public void TakeDamage (int amount, Vector3 hitPoint)
        {
            // If the enemy is dead...
            if(isDead)
                // ... no need to take damage so exit the function.
                return;
				

            // Reduce the current health by the amount of damage sustained.
            currentHealth -= amount;
            
            // Set the position of the particle system to where the hit was sustained.
            hitParticles.transform.position = hitPoint;

            // And play the particles.
            hitParticles.Play();

            // If the current health is less than or equal to zero...
            if(currentHealth <= 0)
            {
                // ... the enemy is dead.
                Death ();
            }
        }

		public void SetFire()
		{
			if (!flameParticles.isPlaying) {
				flameParticles.Play ();
			}
		}


        void Death ()
        {
            // The enemy is dead.
            isDead = true;

            // Turn the collider into a trigger so shots can pass through it.
            capsuleCollider.isTrigger = true;

			//See if the random number we generate is less than our chance to spawn heart - if so spawn heart
			if(RandomChanceHeartShouldSpawn()){
				//Spawn a heart - Tons of comments
				Instantiate(heartPrefab,transform.position, Quaternion.identity);
			}

            // Tell the animator that the enemy is dead.
            anim.SetTrigger ("Dead");

			OnEnemyDeath();
        }

		private bool RandomChanceHeartShouldSpawn()
		{
			return healthSpawnChance > Random.Range (0.0f, 100.0f);
		}


        public void StartSinking ()
        {
            // Find and disable the Nav Mesh Agent.
            GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;

            // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
            GetComponent <Rigidbody> ().isKinematic = true;

            // The enemy should no sink.
            isSinking = true;

            // Increase the score by the enemy's score value.
            ScoreManager.score += scoreValue;

            // After 2 seconds destory the enemy.
            Destroy (gameObject, 2f);
        }
    }
}