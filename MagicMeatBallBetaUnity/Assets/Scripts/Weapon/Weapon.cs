using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Weapon : MonoBehaviour {


	public float damage;

	//public float damageFromMeatBallBase;
	private float damageFromeWeapon;
	private float canAttackKeeptime;
	private float canAttackSetTrueTime;

	private bool canAttack = false;

	public int weaponCode;

	public Text HPText;



	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		

	void OnTriggerStay(Collider other){
		//print ("triggeriN");
		Combat combat = other.GetComponent<Combat> ();
		//Debug.Log ("combat is :" + combat);
		//Debug.Log ("other (be hurt man ) : " + other);
		if (combat) {
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
