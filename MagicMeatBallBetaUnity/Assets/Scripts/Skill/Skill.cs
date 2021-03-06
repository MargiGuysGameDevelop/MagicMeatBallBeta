﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

[SerializeField]
public class Skill : MonoBehaviour{


	#region 基礎欄位
	public string name;

	public int skillNumber;

//	[SerializeField]
//	protected int suitID;

	[SerializeField]
	public float damage;

	[SerializeField]
	public float fatigue;

	[SerializeField]
	protected float skillTime;

	protected Transform selfTran;

	public Vector3 force;
	#endregion

	//UI
	public ColdDown CD = new ColdDown();
	public Sprite icon;

	//for moster

	public bool isRunSkillCDUI(){
		if (CD.isDone)
			return true;
		else{
			CD.Count ();
			return false;
		}
	}

//	bool usingSkill = false;
//	public bool IsUsingSkill(){
//		if (!usingSkill)
//			return false;
//		else
//			usingSkill = false;	
//		return true;
//	}

	#region 攻擊事件
//	[SerializeField]
	public GameObject effect;

	[SerializeField]
	public GameObject projection;

	public GameObject skillEffect;

	/// <summary>
	/// 打到別人時觸發
	/// </summary>
	/// <param name="enemy">Enemy.</param>
	/// <param name="enemyPos">Enemy position.</param>
	/// <param name="face">Face.</param>
	public void HitSomeOne(GameObject enemy,Vector3 enemyPos,Quaternion face){
//		if(effect)
//			GameObject.DestroyObject(Instantiate (effect,enemyPos,face) as GameObject,5f);

	}



	/// <summary>
	/// 發設物體時觸發
	/// </summary>
	/// <param name="enemy">Enemy.</param>
	/// <param name="appearPosition">Appear position.</param>
	/// <param name="face">Face.</param>
//	public void Project(Vector3 appearPosition,Quaternion face){
//		if(projection)
//			GameObject.DestroyObject(Instantiate (projection,appearPosition,face) as GameObject,projectExistTimes);
//	}
	#endregion

	//參照
//	protected Weapon rightWeapon;
//	protected ExtraStates extraStates;

	#region 給予武器/發射物數值
//	[Command]
//	public void CmdUseSkill(int index){
//		RpcUseSkill (index);
//	}

//	public void UseSkill(int index){
//		//		suitID = MBS.currentWeapon;
//		Debug.Log ("UsingSkill");
//		skillNumber = index;
//		weapon.damage = this.damage;
//		weapon.fatigue = this.fatigue;
//		weapon.onHit = null;
//		weapon.onHit += HitSomeOne;
//		weapon.effect = this.effect;
//		weapon.projection = this.projection;
//		meatBallStatus.meatBall.CmdSetAnimInt("SkillInt",index);
//		meatBallStatus.meatBall.CmdSetSkillLayer ();
//		StartSKill ();
//		usingSkill = true;
//	}

	#endregion

	void Awake(){
		NeetToAwake ();
	
	}

	virtual public void NeetToAwake(){
		var mb = GetComponentInParent<MeatBall> ();
		if(mb)
		selfTran = mb.GetComponent<Transform>();
	}

	void Start(){
		CD.currentValue = 0f;
		if (skillTime > CD.value)
			skillTime = CD.value;
	}

	public void Update(){
		if (Time.timeScale == 0)
			return;

		CD.Timer ();

	}

	//剛施展技能
	virtual public void StartSKill(){
		if (name != "") {
				LogManager.Log (GetComponentInParent<MeatBall> ().name + "使出了" + name + "!");
		}
	}

	//施展技能中
	virtual public bool PlayingSkill(){
		if ( skillTime > CD.value- CD.currentValue) {
			return false;
		}
		else {
			EndSkill ();
//			Debug.Log (CD.value- CD.currentValue);
			return true;
		}
	}

	//結束技能
	virtual public void EndSkill(){
		
	}
		
	public float GetDamage(){
		return this.damage;
	}
}
