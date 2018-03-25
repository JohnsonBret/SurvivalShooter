using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompleteProject
{

	public class HealthPickup : MonoBehaviour {

		public float RotationSpeed;
		public PlayerHealth healthScript;

		private Vector3 hoverVertical;
		private Vector3 destination;
		private bool hoverUp = true;
		private PICKUPSTATE currentState;
		[SerializeField]
		private SphereCollider heartCollider;
		[SerializeField]
		private Rigidbody heartBody;

		enum PICKUPSTATE{
			LAUNCH,
			SPAWN,
			HOVER,
		};

		// Use this for initialization
		void Start () 
		{
			gameObject.SetActive(true);
			hoverVertical = new Vector3(0f, 0.5f, 0f);
			healthScript = GameObject.Find ("Player").GetComponent<PlayerHealth> ();
			hoverDestination();

			//If we forget to set a speed lets give it a default value
			if(RotationSpeed == 0)
			{
				RotationSpeed = 20.0f;
			}
		}
		
		// Update is called once per frame
		void Update () 
		{
			switch (currentState) {
				case PICKUPSTATE.LAUNCH:
					Launch();
					break;
				case PICKUPSTATE.SPAWN:
					Spawn ();
					break;
				
				case PICKUPSTATE.HOVER:
					Hover ();
					break;
			}
		}

		private void Launch(){

			Vector3 spawnVector = new Vector3 (Random.Range (-100.0f, 100.0f), 400.0f, Random.Range (-100.0f, 100.0f));
			GetComponent<Rigidbody> ().AddForce (spawnVector);
			currentState = PICKUPSTATE.SPAWN;

		}

		private void Spawn(){

		}

		private void Hover()
		{
			float rotateAmount = RotationSpeed * Time.deltaTime;
			transform.Rotate(0, rotateAmount, 0);


			transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * 0.5f);

			if(Vector3.Distance(transform.position, destination) < 0.01f)
			{
				hoverDestination();
			}
		}

		void hoverDestination()
		{
			if(hoverUp)
			{
				hoverUp = false;
				destination = transform.position + hoverVertical;
			}
			else
			{
				hoverUp = true;
				destination = transform.position - hoverVertical;
			}
		}

		void OnTriggerEnter(Collider col)
		{
			if (col.CompareTag ("Player")) {
				GivePlayerHealth ();
			} 
		}

		private void OnCollisionEnter(Collision col)
		{
			SetHoverSpotToLandingPosition();
			DisableHeartPhysics();
			EnterHoverState();
		}

		private void SetHoverSpotToLandingPosition()
		{
			destination = new Vector3 (transform.position.x, 0.0f, transform.position.z);
		}

		private void DisableHeartPhysics()
		{
			heartCollider.isTrigger = true;
			heartBody.isKinematic = true;
			heartBody.useGravity = false;
		}

		private void EnterHoverState()
		{
			currentState = PICKUPSTATE.HOVER;
		}

		private void GivePlayerHealth()
		{
			healthScript.currentHealth += 30;
			healthScript.healthSlider.value = healthScript.currentHealth;
			gameObject.SetActive(false);
		}
	}
}