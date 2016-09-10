using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

[SerializeField]
public class Skill : MonoBehaviour{


	#region 基礎欄位
	[SerializeField]
	new protected string name;

//	[SerializeField]
	protected int suitID;

	[SerializeField]
	protected float damage;

	[SerializeField]
	protected float fatigue;
	#endregion

	//UI
	public ColdDown CD = new ColdDown();
	public Sprite icon;

	#region 攻擊事件
	[SerializeField]
	protected GameObject effect;

	[SerializeField]
	protected GameObject projection;
	float projectExistTimes ;

	/// <summary>
	/// 打到別人時觸發
	/// </summary>
	/// <param name="enemy">Enemy.</param>
	/// <param name="enemyPos">Enemy position.</param>
	/// <param name="face">Face.</param>
	public void HitSomeOne(GameObject enemy,Vector3 enemyPos,Quaternion face){
		if(effect)
			GameObject.DestroyObject(Instantiate (effect,enemyPos,face) as GameObject,5f);

	}

	/// <summary>
	/// 發設物體時觸發
	/// </summary>
	/// <param name="enemy">Enemy.</param>
	/// <param name="appearPosition">Appear position.</param>
	/// <param name="face">Face.</param>
	public void Project(GameObject enemy,Vector3 appearPosition,Quaternion face){
		if(projection)
			GameObject.DestroyObject(Instantiate (projection,appearPosition,face) as GameObject,projectExistTimes);
	}
	#endregion

	//參照
//	protected Weapon rightWeapon;
//	protected ExtraStates extraStates;

	#region 給予武器數值
	public void GiveWeaponProperty(Weapon weapon){
		weapon.damage = this.damage;
		weapon.fatigue = this.fatigue;
		weapon.onHit = null;
		weapon.project = null;
		weapon.onHit += HitSomeOne;
		weapon.project += Project;
	}

	#endregion
		
	public void Update(){
		if (Time.timeScale == 0)
			return;

		CD.Timer ();
	}
}
