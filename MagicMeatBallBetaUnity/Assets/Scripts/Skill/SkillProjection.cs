using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class SkillProjection : MonoBehaviour {

	//unity component
	Transform trans;
	CapsuleCollider cCollider;

	//攻擊參數
	public float damage;
	public float fatigue = 100f;
	public Vector3 force = new Vector3(1,3,1);
	bool isMove;
	public Vector3 velocity;
	public float destorySecond = 3f;

	public int netID;

	List<Combat> attackedList = new List<Combat>();

	#region 攻擊事件
	public delegate void OnHit(GameObject enemy,Vector3 pos,Quaternion face);
	public OnHit onHit;

//	public delegate void Project(GameObject enemy,Vector3 appearPosition,Quaternion face);
//	public Project project;
	#endregion

	void Initial(float damage,float fatigue,Vector3 force,Vector3 velocity,float dS){
		this.damage = damage;
		this.fatigue = fatigue;
		if(force != null)
			this.force = force;
		this.transform.LookAt (transform.position + velocity);
		destorySecond = dS;
	}

	void OnEnable(){
		
	}

	void Start(){
		if (velocity == Vector3.zero)
			isMove = false;
		else
			isMove = true;

		trans = GetComponent<Transform> ();
		cCollider = GetComponent<CapsuleCollider> ();
	}

	void Update(){
		if (isMove)
			transform.Translate (transform.forward*Time.deltaTime);
	}

	void OnTriggerEnter(Collider other){
//		Combat combat = other.GetComponent<Combat> ();
		Combat combat = other.gameObject.GetComponent<Combat>();
		if (combat && !attackedList.Contains(combat)) {
			if (!attackedList.Contains (combat)) {
				attackedList.Add (combat);
				combat.TakeDamage (damage, netID, fatigue, force);
				onHit (other.gameObject,FindHitPoint(other.transform),
					Quaternion.Euler(transform.forward));
			}
		}
	}
		
	Vector3 FindHitPoint(Transform tran){
		RaycastHit hit;
		cCollider.Raycast
		(
			new Ray (trans.position + new Vector3 (0f, 1f, 0f), tran.position - cCollider.transform.position),
			out hit,
			10f
		);
		return hit.point;
	}

	/*
	void OnTriggerStay(Collider other){
		Combat combat = other.GetComponent<Combat> ();
		if (combat && !attackedList.Contains(combat)) {
			force = 
			if (!attackedList.Contains (combat)) {
				attackedList.Add (combat);
				combat.TakeDamage (damage, netID, fatigue, force);
			}
		}
	}*/
}
