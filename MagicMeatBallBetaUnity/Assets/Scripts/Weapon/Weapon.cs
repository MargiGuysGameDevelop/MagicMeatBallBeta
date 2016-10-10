using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
//using UnityEngine.Networking;
using Xft;

public class Weapon : MonoBehaviour {

	Collider weaponCollider ;

	//攻擊參數
	public float damage;
	public float fatigue = 100f;
	public bool isHands= false;
	public Vector3 force = Vector3.zero;

	//public float damageFromMeatBallBase;
	private float damageFromeWeapon;
	private float canAttackKeeptime;
	private float canAttackSetTrueTime;

//	private bool canAttack = false;
//	private bool attackOnceBool = false;

	public int weaponCode;

//	public Text HPText;

	List<Combat> attackedList = new List<Combat>();

	public XWeaponTrail[] trails;

	#region 技能攻擊相關
	public delegate void OnHit(GameObject enemy,Vector3 pos,Quaternion face);
	public OnHit onHit;

//	public delegate void Project(Vector3 appearPosition,Quaternion face);
//	public Project project;

	public GameObject effect;


	public GameObject projection;

	public GameObject skillEffect;


	#endregion

	public MeatBallStatus selfStatus;
	//Moster
	public MosterStatus mosterStatus;

	void Awake(){
		weaponCollider = GetComponent<Collider> ();
//		trails = GetComponentsInChildren<XWeaponTrail> ();
	}

	// Use this for initialization
	void Start () {
		selfStatus = GetComponentInParent<MeatBallStatus> ();
		if (selfStatus)
			weaponCode = selfStatus.currentWeapon;
		else
			mosterStatus = GetComponentInParent<MosterStatus> ();
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
				Vector3 newForce;
				if (selfStatus == null) {
					newForce = mosterStatus.transform.forward;
				} else {
					newForce = selfStatus.transform.forward;
				}
				newForce.y += 2;
				newForce.x *= force.x;
				newForce.z *= force.z;
				attackedList.Add (combat);
				if (selfStatus != null)
					combat.TakeDamage (damage, selfStatus.playerNetId, fatigue, newForce);
				else {
					if (other.GetComponent<Moster> ())
						return;
					combat.TakeDamage (damage, -1, fatigue, newForce);
				}
//				if(onHit != null)onHit (other.gameObject,other.gameObject.transform.position,Quaternion.Euler(transform.forward));
				//onHit(other.gameObject,other.transform.position,Quaternion.Euler(transform.forward));
//				Debug.Log ("中獎");

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

//	[ServerCallback]
	public void WeaponCoilderOn(){
		weaponCollider.enabled = true;
//		foreach(XWeaponTrail trail in trails){
//			trail.Activate ();
//		}
	}

//	[ServerCallback]
	public void WeaponCoilderOff(){
		weaponCollider.enabled = false;
//		foreach (XWeaponTrail trai in trails)
//			trai.Deactivate ();
		SetAttackedListEmpty ();
	}

//	[ServerCallback]
	public void UseSkill(){
		
	}

	[ContextMenu("增加技能for貢丸")]
	public void Initial(){
		var attack = new GameObject ();
		attack.AddComponent<Skill> ();
		Instantiate (attack,transform.position,Quaternion.Euler(transform.forward));
		attack.name = "attack";
		attack.transform.parent = this.transform;

		for(int i=0;i<4;i++){
			var gameObject = new GameObject ();
			gameObject.AddComponent<Skill> ();
			Instantiate (gameObject,transform.position,Quaternion.Euler(transform.forward));
			gameObject.name = "skill" + (i + 1).ToString ();
			gameObject.transform.parent = this.transform;
		}
	}

	[ContextMenu("新增技能for怪物")]
	public void NewSkillForMoster(){
		var attack = new GameObject ();
		attack.AddComponent<MosterSkill> ();
		Instantiate (attack,transform.position,Quaternion.Euler(transform.forward));
		attack.name = "attack";
		attack.transform.parent = this.transform;

		for(int i=0;i<4;i++){
			var gameObject = new GameObject ();
			gameObject.AddComponent<MosterSkill> ();
			Instantiate (gameObject,transform.position,Quaternion.Euler(transform.forward));
			gameObject.name = "skill" + (i + 1).ToString ();
			gameObject.transform.parent = this.transform;
		}
	}
}
