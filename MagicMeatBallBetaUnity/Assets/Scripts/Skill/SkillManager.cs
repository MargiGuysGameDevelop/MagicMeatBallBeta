using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour {

	#region 基本欄位
	[SerializeField]
	string weaponSkillName;
	#endregion

	#region 初始化
	ExtraStates extraStates;
	Weapon weapon;

	[ContextMenu("初始化技能及UI")]
	void Initial(){

		weapon = GetComponent<Weapon> ();

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
		skillList = GetComponentsInChildren<Skill> ();
	}
	#endregion

	#region 技能列
	int skillIndex = 0;
	public Skill[] skillList;
	#endregion

	#region UI
	public SkillUIManager UI;
	#endregion

	#region 優化
	int lastSkillIndex = 0;
	string[] buttonName = {
		"Attack",
		"Skill01",
		"Skill02",
		"Skill03",
		"Skill04",
	};
	#endregion

	void Start(){
		extraStates = GetComponentInParent<ExtraStates> ();
		weapon = GetComponent<Weapon> ();

		UI = GameObject.Find ("SkillUI").GetComponentInChildren<SkillUIManager> ();

		UI.InitialSkills (skillList);
		for(int i=0;i<4;i++){
			UI.iconList [i].sprite = skillList [i+1].icon;
		}
	}

	void Update(){

		for(int i=0;i<buttonName.Length;i++){
			if (Input.GetButtonDown (buttonName [i]) && skillList [i].CD.isDone)
				skillIndex = i;
		}

		if (skillIndex != lastSkillIndex) {
			skillList [skillIndex].GiveWeaponProperty (weapon);
			skillList [skillIndex].CD.Count ();
			lastSkillIndex = skillIndex;
		}
	}

//	[ContextMenu("抓取技能及UI")]
//	void GetSkills(){
//	}
}
