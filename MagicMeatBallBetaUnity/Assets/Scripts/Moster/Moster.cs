using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public enum MosterType{
	Moster,
	Boss
};

public enum MosterKind{
	normal,
	Random
};


[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(NetworkAnimator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(FunPhsics))]
[RequireComponent(typeof(Combat))]
[RequireComponent(typeof(MosterNevgation))]
[RequireComponent(typeof(MosterStatus))]
public class Moster :  NetworkBehaviour{

	//public adjust
	public MosterType type = MosterType.Moster;
	public MosterKind kind = MosterKind.normal;
	[Header("if kind = random")]
	int maxKindNumber  = 2;
	[SerializeField]
	float[] ranges;
	float stopDistance ;
	[SerializeField]
	float attackRange = 1.5f;
	[SerializeField]
	float attackFrequence = 2f;
	private float attackTimer;

	public Combat player;

	MosterNevgation nav;
	Animator anim;
	Transform trans;
	Weapon weapon;
	public MosterStatus selfStatus;

	//delegate
	public delegate void NormalDelegate();
	public NormalDelegate deadFunc;
	public NormalDelegate callMoster;
	public 	NormalDelegate addScore;
	public NormalDelegate addKill;

	Combat selfComat;

	//stage
	public int myIndexOnGM = 0;

	MosterSkillManager skillManager; 

	void Awake(){
		//self property
		anim = GetComponent<Animator> ();
		trans = transform;
		selfStatus = GetComponent<MosterStatus> ();
		selfComat = GetComponent<Combat> ();
		nav = GetComponent<MosterNevgation> ();
		weapon = GetComponentInChildren<Weapon> ();
		attackTimer = 0f;
		skillManager = GetComponent<MosterSkillManager>();
	}

	void OnEnable(){
		if (stopDistance < attackRange)
			stopDistance = attackRange;
		if (kind == MosterKind.Random) {
			var weapons = GetComponentsInChildren<Weapon> ();
			int number = Random.Range (0, weapons.Length);
			SetKindOfMoster (number);
			for(int i=0;i<weapons.Length;i++){
				if (i == number) {
					attackRange = ranges [i];
					weapon = weapons [number];
					weapon.enabled = true;
				} else {
					weapons [i].gameObject.SetActive (false);
				}
			}

		}
	}

	public bool NeedToUpdate(){
		
		if (!selfComat)
			return false;
		else if (selfComat.enabled == false)
			return false;

		if (Time.timeScale == 0f)
			return true;

		if (player == null) {
			Debug.Log (1);
			return true;
		}
		
		if (player.selfStatus.isInvincible) {
			return true;
		}

		if (!selfComat || selfComat.enabled == false)
			return true;

		attackTimer += Time.deltaTime;
			
		if (!IsSkillable ())
			return true;

		if (skillManager) {
			skillManager.angleWithTarget = nav.LookTarget ();
		}
		else
			nav.LookTarget ();
		
		if (nav.distanceWithPlayer <= stopDistance) {
			if (attackTimer >= attackFrequence && 
				nav.distanceWithPlayer <= attackRange){
				anim.SetBool ("Attack", true);
				attackTimer = 0f;
			}
			CmdSetAnimFloat ("movement",0f);
			return true;
		}

		if (skillManager)
			skillManager.distanceWithTarget = nav.distanceWithPlayer;

		nav.Follow ();


		return true;
	}

	/// <summary>
	/// extra = times of attackRange
	/// </summary>
	/// <param name="extra">Extra.</param>

		
	#region publicFunction
	public float DistanceWithTarget(){
//		Debug.Log (1);
		return nav.distanceWithPlayer;
	}

	public bool IsSkillable(){
		return anim.GetBool ("Skillable");
	}

	public bool IsHurt(){
		return anim.GetBool ("Hurt");
	}

	public void SetStopDistance(float extra){
		stopDistance = attackRange * extra;
	}

	public void SetKindOfMoster(int kind){
		anim.SetLayerWeight (kind,1f);
	}

	[ServerCallback]
	public void Dead(){
		selfStatus.isInvincible = true;
		if (deadFunc != null)
			deadFunc ();
		CmdSetAnimBool ("Dead", true);
		selfComat.enabled = false;
		if (type == MosterType.Boss && addScore != null)
			addScore ();
		else if(addKill != null)
			addKill ();
	}

	[ServerCallback]
	public void Hurt(){
		if (type == MosterType.Boss)
			return;
		CmdSetAnimBool ("Hurt",true);
	}
	[ServerCallback]
	public bool IsInvincble(){
		return selfStatus.isInvincible;
	}
	#endregion

	#region AnimationEvent
	[ServerCallback]
	public void Destroy(){
		NetworkServer.Destroy (gameObject);
	}

	[ServerCallback]
	//只要Server的Coilder開並讓其他玩家扣HP就夠了
	public void AttackColliderOn(){
		weapon.WeaponCoilderOn ();
	}

	[ServerCallback]
	public void AttackColliderOff(){
		weapon.WeaponCoilderOff();
	}

	[ServerCallback]
	void CallMoster(){
		if (callMoster != null)
			callMoster ();
	}

	[ServerCallback]
	public void Project(){
		if (weapon.projection == null)
			return;
		var projection = Instantiate(weapon.projection,
			transform.position,transform.rotation) as GameObject;
		SkillProjection skillProjection = projection.GetComponent<SkillProjection> ();
		skillProjection.mosterStatus = this.selfStatus;
		skillProjection.myMainTransform = this.trans;
		skillProjection.attackedList.Add (this.GetComponent<Combat> ());
		skillProjection.ignoreCollider = GetComponent<CapsuleCollider> ();
		skillProjection.damage = weapon.damage;
		skillProjection.forceIndex = weapon.force.x;
		skillProjection.forceY = weapon.force.y;
		skillProjection.fatigue = weapon.fatigue;
		skillProjection.hitEffect = weapon.effect;
		skillProjection.gameObject.SetActive (true);
		NetworkServer.Spawn (projection);
	}

	[ServerCallback]
	public void SkillEffect(){
		if(weapon.skillEffect){
		var effect = Instantiate(weapon.skillEffect,
			transform.position,transform.rotation) as GameObject;
			var projection = effect.GetComponent<SkillProjection> ();
			projection.myMainTransform = transform;
			projection.mosterStatus = this.selfStatus;
			projection.myMainTransform = this.trans;
			projection.attackedList.Add (this.GetComponent<Combat> ());
			projection.ignoreCollider = GetComponent<CapsuleCollider> ();
			projection.damage = weapon.damage;
			projection.forceIndex = weapon.force.x;
			projection.forceY = weapon.force.y;
			projection.fatigue = weapon.fatigue;
			projection.hitEffect = weapon.effect;
			projection.gameObject.SetActive (true);
			effect.SetActive (true);
			NetworkServer.Spawn (effect);
		}
	}
		
		
	#endregion

	#region BaseAnim
	[ServerCallback]
	public void CmdInitAnim(){
		RpcInitAnim ();
	}

	[ServerCallback]
	public void RpcInitAnim(){
		//		Debug.Log ("重生");
		anim.Play ("Movement");
	}

	[ServerCallback]
	public void CmdSetAnimFloat(string boolName,float setFloat)
	{
		RpcSetAnimFloat(boolName,setFloat);
	}

	[ServerCallback]
	public void RpcSetAnimFloat(string boolName,float setFloat)
	{
		anim.SetFloat(boolName,setFloat);
	}

	[ServerCallback]
	public void CmdSetAnimInt(string boolName,int setInt)
	{
		RpcSetAnimInt(boolName,setInt);
	}

	[ServerCallback]
	public void RpcSetAnimInt(string boolName,int setInt)
	{
		anim.SetInteger(boolName,setInt);
	}



	[ServerCallback]
	public void CmdSetAnimBool(string boolName,bool boolState)
	{
		RpcSetAnimBool(boolName,boolState);
	}

	[ServerCallback]
	public void RpcSetAnimBool(string boolName,bool boolState)
	{
		anim.SetBool(boolName,boolState);
	}

	[ServerCallback]
	public void CmdSetAnimTrigger(string triggerName)
	{
		RpcSetAnimTrigger(triggerName);
	}

	[ServerCallback]
	public void RpcSetAnimTrigger(string triggerName)
	{
		anim.SetTrigger(triggerName);
	}
	#endregion


}
