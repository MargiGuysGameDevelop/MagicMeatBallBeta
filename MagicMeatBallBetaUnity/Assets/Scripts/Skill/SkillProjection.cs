using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class SkillProjection : NetworkBehaviour {
	// 無力 攻擊者前方 物件中心發散 物件中心收斂
	public enum ForceType{None,AttackersForward,Diverge,Converge};

	public ForceType forceType;

	[Header("是否每人只能打到一次(與下方平行)")]
	public bool isHitOncePerPerson;

	[Header("打到一次敵人後物件銷毀")]
	public bool isDestroyOnOnceHit;

	[Header("生命週期")]
	public float lifeTime;

	Collider ingnorCollider;

	[Header("XZVelocity")]
	public float forceIndex;
	[Header("YVelocity")]
	public float forceY;

	[HideInInspector]
	public Transform selfTransform;
//	[HideInInspector]
	public float damage;
	[HideInInspector]
	public MeatBallStatus selfStatus;

	[Header("破甲值")]
	public float fatigue;
//	[HideInInspector]
	public Vector3 force;

	public List<Combat> attackedList = new List<Combat>();

	void Awake(){
		selfTransform = GetComponent<Transform> ();
	}

	void SetIgnorCollider(){

		ingnorCollider = selfStatus.GetComponent<Collider> ();
	}

	[ServerCallback]
	void OnTriggerStay(Collider other){
		if (!ingnorCollider)
			SetIgnorCollider ();


		if(ingnorCollider == other)
			return;

		Combat combat = other.GetComponent<Combat> ();
		if (combat) {
			CountForce (combat.transform);
			if (!attackedList.Contains (combat)) {
				attackedList.Add (combat);
				combat.TakeDamage (damage, selfStatus.playerNetId, fatigue, force*forceIndex);
			}
			if (isDestroyOnOnceHit) {
				NetworkServer.Destroy (gameObject);
			} else {
				if(!isHitOncePerPerson)
					attackedList.Remove (combat);
			}


		}
	}

	void Update(){
		if (lifeTime > 0) {
			lifeTime -= Time.deltaTime;
		} else {
			NetworkServer.Destroy (gameObject);
		}

	}


	void CountForce(Transform attackedTransform){
		switch (forceType) {
		case ForceType.None:
			force = Vector3.zero;
			break;
		case ForceType.AttackersForward:
			force = selfStatus.transform.forward;
			force += force + new Vector3(0,forceY,0);
			break;
		case ForceType.Diverge:
			force = attackedTransform.position - selfTransform.position;
			force.Normalize ();
			force += force + new Vector3(0,forceY,0);
			break;
		case ForceType.Converge:
			force = selfTransform.position - attackedTransform.position;
			force.Normalize ();
			force += force + new Vector3(0,forceY,0);
			break;
		};
	}


}
