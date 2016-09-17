using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SkillManager : NetworkBehaviour {

	#region 基本欄位
	[SerializeField]
	string weaponSkillName;

	ExtraStates extraStates;
	Weapon weapon;
	MeatBallStatus selfStatus;
	MeatBall self;

	bool usingSkill = false;
	#endregion

	#region 初始化

//	[ContextMenu("初始化技能及UI")]
	public void Initial(){
		weapon = GetComponentInChildren<Weapon> ();
		skillList = weapon.GetComponentsInChildren<Skill> ();
		skillIndex = 0;
		CmdUsingSkill ();
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

	#region 施展技能Delegate
	public delegate bool SkillPlaying();
	public SkillPlaying playing;

	public delegate void SkillStart();
	SkillStart start;

	public delegate void SkillEnd();
	SkillEnd end;

	public bool NoAnySkill(){
		return true;
	}
	#endregion

	void Start(){
		selfStatus = GetComponent<MeatBallStatus> ();

		extraStates = GetComponent<ExtraStates> ();

		self = GetComponent<MeatBall> ();

		weapon = GetComponent<Weapon> ();

		UI = GameObject.Find ("SkillUI").GetComponentInChildren<SkillUIManager> ();

//		UI.InitialSkills (skillList);
		for(int i=0;i<4;i++){ 
			UI.iconList [i].sprite = skillList [i+1].icon;
		}

		playing = NoAnySkill;
	}

	void Update(){
		if (Time.timeScale == 0)
			return;

		if (!isLocalPlayer)
			return;

		//skill have Isskillable 
		//meatball ishurt
		//isSkilling 
		//isCD

		if (!self.IsSkillable () || self.IsHurt () || selfStatus.isDead)
			return;

		if (playing ()) {
			if (playing != NoAnySkill)
				playing = NoAnySkill;

			for (int i = 0; i < buttonName.Length; i++) {
				if (Input.GetButtonDown (buttonName [i]) && skillList [i].CD.isDone)
					skillIndex = i;
			}

			if (skillIndex != lastSkillIndex) {
				CmdUsingSkill ();
				self.CmdSetAnimInt("SkillInt",skillIndex);
				self.CmdSetSkillLayer ();
				return;
			}
			if (skillIndex != 0)
				skillIndex = 0;
		} 
//		else
//			Debug.Log ("GG");

	}

	[Command]
	void CmdUsingSkill(){
		RpcUsingSkill ();
	}

	[ClientRpc]
	void RpcUsingSkill(){
		Debug.Log ("RpcUsingSkill");
		if(skillIndex != 0)
			UI.CountCD (skillIndex-1,skillList[skillIndex].CD.value);
		skillList [skillIndex].CD.Count ();
		skillList [skillIndex].skillNumber = skillIndex;
		Debug.Log(skillIndex);
		playing = skillList[skillIndex].PlayingSkill;
//		Debug.Log (skillList[skillIndex].damage);
		weapon.damage = skillList[skillIndex].damage;
//		Debug.Log (weapon.damage);
		weapon.fatigue = skillList[skillIndex].fatigue;
		weapon.onHit = skillList[skillIndex].HitSomeOne;
		weapon.effect = skillList[skillIndex].effect;
		weapon.projection = skillList[skillIndex].projection;
		start = skillList[skillIndex].StartSKill ;
		start ();
		usingSkill = true;
		lastSkillIndex = skillIndex;
	}
}

