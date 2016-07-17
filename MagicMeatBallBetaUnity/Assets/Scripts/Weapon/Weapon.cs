using UnityEngine;
using System.Collections;


public class Weapon : MonoBehaviour {


	public float damage;

	//public float damageFromMeatBallBase;
	private float damageFromeWeapon;

	private float canAttackKeeptime;
	private float canAttackSetTrueTime;

	private bool canAttack = false;

	[SerializeField]public float weaponCode;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		

	void OnTriggerEnter(Collider other){
		//print ("triggeriN");
		Combat combat = other.GetComponent<Combat> ();
		//Debug.Log ("combat is :" + combat);
		//Debug.Log ("other (be hurt man ) : " + other);
		if (combat && canAttack) {
			//print ("find combat");
			combat.CmdTakeDamage (damage);
		}

	}

	public void SetAttackKeepTime(float startTime,float keepTime){
		//canAttack = true;
		if (canAttack == false) {
			canAttackSetTrueTime = startTime;
			canAttackKeeptime = keepTime;

			StartCoroutine (SetAttackBoolByTime ());
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



}
