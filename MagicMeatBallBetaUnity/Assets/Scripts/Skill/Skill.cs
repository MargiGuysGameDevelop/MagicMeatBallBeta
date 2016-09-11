using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Networking;

[SerializeField]
public class Skill : NetworkBehaviour{


	#region 基礎欄位
	public string name;

	public int skillNumber;

//	[SerializeField]
	protected int suitID;

	[SerializeField]
	protected float damage;

	[SerializeField]
	protected float fatigue;

	[SerializeField]
	protected float skillTime;

	protected Transform meatBallTran;
	#endregion

	//UI
	public ColdDown CD = new ColdDown();
	public Sprite icon;

	#region 攻擊事件
	[SerializeField]
	protected GameObject effect;

	[SerializeField]
	public GameObject projection;
	protected float projectExistTimes ;

	Weapon weaponScript;

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

	void Awake(){
		weaponScript = GetComponentInParent<Weapon> ();
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
	public void GiveProperty(Weapon weapon,MeatBallStatus MBS,int index){
		
		suitID = MBS.currentWeapon;
		skillNumber = index;
		weapon.damage = this.damage;
		weapon.fatigue = this.fatigue;
		weapon.onHit = null;
		weapon.onHit += HitSomeOne;
		weapon.effect = this.effect;
		weapon.projection = this.projection;
		MBS.meatBall.CmdSetAnimInt("SkillInt",index);
		MBS.meatBall.CmdSetSkillLayer ();
		//Debug.Log ("before Command");
		//CmdGiveWeaponProperty ();
		//Debug.Log ("after Command");
		if (meatBallTran == null)
			meatBallTran = MBS.GetComponent<Transform> ();
	}
	#endregion

	[Command]
	void CmdGiveWeaponProperty(){
		Debug.Log ("in Command");
		weaponScript.effect = this.effect;
		weaponScript.projection = this.projection;

	}

	void Start(){
		CD.currentValue = 0f;
	}

	public void Update(){
		if (Time.timeScale == 0)
			return;

		CD.Timer ();

	}

	//剛施展技能
	virtual public void StartSKill(){
		if(name != "attack")
			LogManager.Log (GetComponentInParent<MeatBall>().name + "使出了" + name + "!");
	}

	//施展技能中
	virtual public bool PlayingSkill(){
		if (CD.value < skillTime + CD.currentValue) {
			return false;
		}
		else {
			EndSkill ();
			return true;
		}
	}

	//結束技能
	virtual public void EndSkill(){
		
	}

}
