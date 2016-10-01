using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkIdentity))]
public class SkillProjection : NetworkBehaviour {
	//普通 沒有
	public enum ProjectionType{Generic,EffectEmitter,HitEffect}
	// 無力 攻擊者前方 物件中心發散 物件中心收斂 觸發其他物件
	public enum ForceType{None,AttackersForward,Diverge,Converge};
	//無(預設) 前進 
	public enum MoveType{None,Forward};

	public MoveType moveType = MoveType.None;
	[Header("速率(moveType = None請無視)")]
	public float projectionVelocity;

	public ProjectionType projectionType = ProjectionType.Generic;
	public ForceType forceType;

	[Header("是否每人只能打到一次(與下方平行)")]
	public bool isHitOncePerPerson;

	[Header("打到一次敵人後物件銷毀")]
	public bool isDestroyOnOnceHit;

	[Header("EffectEmitter情況下打到人所出現的物件(係數皆與此設定相同)")]
	public GameObject[] openObjectAfterHit = new GameObject[1];
	[Header("EffectEmitter情況下打到人後消失的物件")]
	public GameObject[] closeObjectAfterHit = new GameObject[1];

	[Header("物件生命週期")]
	public float lifeTime;
	[Header("EffectEmitter情況下打到人後的生命週期")]
	public float lifeTimeAfterHit;

//	[SyncVar]
//	int SelfID;
	[HideInInspector]
	public Collider ignoreCollider;
	BoxCollider myCollider;

	[Header("XZVelocity的力量")]
	public float forceIndex;
	[Header("YVelocity的力量")]
	public float forceY;

	[HideInInspector]
	public Transform selfTransform;
	[HideInInspector]
	public float damage;
//	[HideInInspector]
	public MeatBallStatus selfStatus;

	[Header("破甲值")]
	[HideInInspector]
	public float fatigue;

	[HideInInspector]
	public Vector3 force;

	public GameObject hitEffect;

	public List<Combat> attackedList = new List<Combat>();

	void Awake(){
		selfTransform = GetComponent<Transform> ();
		myCollider = GetComponent<BoxCollider> ();

		if(projectionType == ProjectionType.EffectEmitter)
			foreach(GameObject go in openObjectAfterHit){
				go.SetActive (false);
			}
	}

//	void Start(){
//		ignoreCollider = 
//	}

	void SetIgnorCollider(){

		ignoreCollider = selfStatus.GetComponent<Collider> ();
	}

	[ServerCallback]
	void OnTriggerStay(Collider other){

		if (projectionType == ProjectionType.HitEffect)
			return;
		
		if (!ignoreCollider)
			SetIgnorCollider ();
		
		if(ignoreCollider == other)
			return;

		if (projectionType == ProjectionType.EffectEmitter) {
			RpcEffectEmitter ();
			return;
		}

		Combat combat = other.GetComponent<Combat> ();
		if (combat) {
			CountForce (combat.transform);
			if (!attackedList.Contains (combat)) {
				attackedList.Add (combat);
				combat.TakeDamage (damage, selfStatus.playerNetId, fatigue, force*forceIndex);
			}
			if (isDestroyOnOnceHit) {
				if(gameObject)
					NetworkServer.Destroy (gameObject);
			} else {
				if(!isHitOncePerPerson)
					attackedList.Remove (combat);
			}
		}
	}

	[ServerCallback]
	void OnTriggerEnter(Collider other){
		if (other.GetComponent<MeatBallStatus> () == selfStatus)
			return;

		if (hitEffect) {
			var effect = Instantiate (hitEffect,
				other.transform.position, other.transform.rotation) as GameObject;
			NetworkServer.Spawn (effect);
		}
	}
		

	void Update(){
		if (Time.timeScale == 0f)
			return;

		Move ();
		
		if (lifeTime > 0) {
			lifeTime -= Time.deltaTime;
		} else {
			if(projectionType == ProjectionType.EffectEmitter){
				lifeTime += lifeTimeAfterHit;
				RpcEffectEmitter ();
				return;
			}
			NetworkServer.Destroy (gameObject);
		}

	}

	void Move(){
		switch(moveType){
		case MoveType.None:
			return;
		case MoveType.Forward:
			selfTransform.Translate (selfTransform.forward * projectionVelocity * Time.deltaTime,
				relativeTo:Space.World);
			break;
		}
	}


	void CountForce(Transform attackedTransform){
		switch (forceType) {
		case ForceType.None:
			force = Vector3.zero;
			break;
		case ForceType.AttackersForward:
			force = selfStatus.transform.forward.normalized * forceIndex;
			force += force + new Vector3(0,forceY,0);
			break;
		case ForceType.Diverge:
			force = attackedTransform.position - selfTransform.position;
			force = force.normalized * forceIndex;
			force += force + new Vector3(0,forceY,0);
			break;
		case ForceType.Converge:
			force = selfTransform.position - attackedTransform.position;
			force = force.normalized * forceIndex;
			force += force + new Vector3(0,forceY,0);
			break;
			default:
			break;
		};
	}

	[ClientRpc]
	void RpcEffectEmitter(){
		foreach (GameObject go in openObjectAfterHit) {
			var skillProjection = go.GetComponent<SkillProjection> ();
			if (isServer) {
				skillProjection.selfStatus = this.selfStatus;
				skillProjection.attackedList.Add (selfStatus.GetComponent<Combat> ());
				skillProjection.damage = this.damage;
				skillProjection.lifeTime = this.lifeTime + 1f;
				skillProjection.fatigue = this.fatigue;
			}
			go.SetActive (true);
		}
		foreach (GameObject go in closeObjectAfterHit) {
			go.SetActive (false);
		}
		myCollider.enabled = false;
		forceType = ForceType.None;
		moveType = MoveType.None;
		projectionType = ProjectionType.Generic;
	}

}
