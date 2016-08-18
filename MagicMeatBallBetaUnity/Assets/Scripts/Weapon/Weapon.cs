using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Weapon : NetworkBehaviour {

	Collider weaponCollider ;

	public float damage;

	//public float damageFromMeatBallBase;
	private float damageFromeWeapon;
	private float canAttackKeeptime;
	private float canAttackSetTrueTime;

	private bool canAttack = false;
//	private bool attackOnceBool = false;

	public int weaponCode;

	public Text HPText;

	List<Combat> attackedList = new List<Combat>();

	void Awake(){
		weaponCollider = GetComponent<Collider> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//SetCanAttackBool ();
	}
		

	void OnTriggerStay(Collider other){

		Combat combat = other.GetComponent<Combat> ();
		if (combat) {
			if (!attackedList.Contains (combat)) {
				attackedList.Add (combat);
				combat.TakeDamage (damage);
				//attackOnceBool = true;
				//canAttack = false;
			}
		}
		/*
		if (canAttack && combat) {

			combat.TakeDamage (damage);
			attackOnceBool = true;
			canAttack = false;
		}
		*/
	}

	void SetAttackedListEmpty(){
		attackedList.Clear ();

	}

	void SetCanAttackBool(){
		
		if (canAttackSetTrueTime > 0) {
			canAttackSetTrueTime -= Time.deltaTime;
		} 
		else if (canAttackKeeptime > 0) {
			canAttackKeeptime -= Time.deltaTime;
//			if (!attackOnceBool) 
//				canAttack = true;
		}
		else {
			SetAttackedListEmpty ();
			canAttack = false;
		}

	}

	public void SetAttackKeepTime(float startTime,float keepTime){
		//canAttack = true;
		/*
		if (canAttack == false) {
			canAttackSetTrueTime = startTime;
			canAttackKeeptime = keepTime;
			StartCoroutine (SetAttackBoolByTime ());
		}
		*/
		if (canAttackKeeptime <= 0) {
			canAttackSetTrueTime = startTime;
			canAttackKeeptime = keepTime;
//			attackOnceBool = false;
		}
	}



	IEnumerator SetAttackBoolByTime(){
		yield return new WaitForSeconds (canAttackSetTrueTime);
		canAttack = true;
		//Debug.Log ("canAttack = true");
		yield return new WaitForSeconds (canAttackKeeptime);
		canAttack = false;
		//Debug.Log ("canAttack = false");
	}

	/*IEnumerator SetAttackBoolFalseByTime(){
	
	}*/

	[ServerCallback]
	public void WeaponCoilderOn(){
		weaponCollider.enabled = true;

	}

	[ServerCallback]
	public void WeaponCoilderOff(){
		weaponCollider.enabled = false;

		SetAttackedListEmpty ();
	}
}
