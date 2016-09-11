	using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Weapon : NetworkBehaviour {

	Collider weaponCollider ;

	//攻擊參數
	public float damage;
	public float fatigue = 100f;
	public bool isHands= false;
	public Vector3 force = new Vector3(1,3,1);

	//public float damageFromMeatBallBase;
	private float damageFromeWeapon;
	private float canAttackKeeptime;
	private float canAttackSetTrueTime;

//	private bool canAttack = false;
//	private bool attackOnceBool = false;

	public int weaponCode;

	public Text HPText;

	List<Combat> attackedList = new List<Combat>();

	#region 技能攻擊相關
	public delegate void OnHit(GameObject enemy,Vector3 pos,Quaternion face);
	public OnHit onHit;

//	public delegate void Project(Vector3 appearPosition,Quaternion face);
//	public Project project;

	public GameObject effect;
	public GameObject projection;
	#endregion

	public MeatBallStatus selfStatus;

	void Awake(){
		weaponCollider = GetComponent<Collider> ();
	}

	// Use this for initialization
	void Start () {
		selfStatus = GetComponentInParent<MeatBallStatus> ();
		weaponCode = selfStatus.currentWeapon;
		WeaponCoilderOff ();
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
				combat.TakeDamage (damage,selfStatus.playerNetId,fatigue,force);
				onHit (other.gameObject,other.gameObject.transform.position,Quaternion.Euler(transform.forward));

				//onHit(other.gameObject,other.transform.position,Quaternion.Euler(transform.forward));

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
	/*
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
//			canAttack = false;
		}

  */

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



//	IEnumerator SetAttackBoolByTime(){
//		yield return new WaitForSeconds (canAttackSetTrueTime);
//		canAttack = true;
//		//Debug.Log ("canAttack = true");
//		yield return new WaitForSeconds (canAttackKeeptime);
//		canAttack = false;
//		//Debug.Log ("canAttack = false");
//	}

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

	[ServerCallback]
	public void UseSkill(){
		
	}
}
