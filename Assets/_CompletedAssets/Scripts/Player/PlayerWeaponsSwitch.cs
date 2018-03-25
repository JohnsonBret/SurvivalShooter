using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CompleteProject
{
	public class PlayerWeaponsSwitch : MonoBehaviour {

		[SerializeField]
		private GameObject[] Kalash;
		[SerializeField]
		private GameObject[] FlameThrower;

		public enum WEAPON
		{
			KALASHNIKOV,
			FLAMETHOWER
		};

		private WEAPON currentWeapon;

		void Start () 
		{
			currentWeapon = WEAPON.KALASHNIKOV;
		}

		void Update () {
			if (SwitchWeaponsButtonDown ()) {
				SwitchWeapon ();
			}
				
		}

		private bool SwitchWeaponsButtonDown(){
			return Input.GetKeyDown (KeyCode.Q);
		}

		//This logic can seem a bit backwards at first
		//If our current weapon is a Kalashnikov and we
		//want to switch weapons we will switch to the
		//flamethrower
		private void SwitchWeapon(){

			switch (currentWeapon) {

				case WEAPON.KALASHNIKOV:
					currentWeapon = WEAPON.FLAMETHOWER;
					SetFlameThrowerActive ();
					DeActivateKalashnikov ();
					break;

				case WEAPON.FLAMETHOWER:
					currentWeapon = WEAPON.KALASHNIKOV;
					SetKalashnikovActive ();
					DeActivateFlamethrower ();
					break;
				}
		}

		private void SetKalashnikovActive()
		{
			foreach (GameObject weaponComponent in Kalash) {
				weaponComponent.SetActive (true);
			}
		}

		private void DeActivateKalashnikov()
		{
			foreach (GameObject weaponComponent in Kalash) {
				weaponComponent.SetActive (false);
			}
		}

		private void SetFlameThrowerActive()
		{
			foreach (GameObject weaponComponent in FlameThrower) {
				weaponComponent.SetActive (true);
			}
		}

		private void DeActivateFlamethrower()
		{
			foreach (GameObject weaponComponent in FlameThrower) {
				weaponComponent.SetActive (false);
			}
		}
	}
}
