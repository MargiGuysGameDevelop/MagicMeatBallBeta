﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SkillManager : NetworkBehaviour {

	#region 基本欄位
	[SerializeField]
	protected string weaponSkillName;

	protected ExtraStates extraStates;
	protected Weapon weapon;
	protected MeatBallStatus selfStatus;
	protected MeatBall self;

	protected bool usingSkill = false;
	#endregion

	#region 初始化

//	[ContextMenu("初始化技能及UI")]
	virtual public void Initial(){
		weapon = GetComponentInChildren<Weapon> ();
		skillList = weapon.GetComponentsInChildren<Skill> ();
//
//		skillIndex = 0;
//		CmdUsingSkill (0);
	}
	#endregion

	#region 技能列
	protected int skillIndex = 0;
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
	protected SkillStart start;

	public delegate void SkillEnd();
	SkillEnd end;

	public bool NoAnySkill(){
		return true;
	}
	#endregion

	void Start(){
		NeedToStart ();
	}

	virtual public void NeedToStart(){
		selfStatus = GetComponent<MeatBallStatus> ();

		extraStates = GetComponent<ExtraStates> ();

		self = GetComponent<MeatBall> ();

		Initial ();

		UI = GameObject.Find ("SkillUI").GetComponentInChildren<SkillUIManager> ();

		for(int i=0;i<4;i++){ 
			UI.iconList [i].sprite = skillList [i+1].icon;
		}

		playing = NoAnySkill;
	}

	void Update(){
		if (Time.timeScale == 0)
			return;

		NeedToUpdate ();
	}

	virtual public void NeedToUpdate(){
		if (!isLocalPlayer)
			return;
		
		if (!self.IsSkillable () || self.IsHurt () || selfStatus.isDead)
			return;

		if (playing ()) {
			if (playing != NoAnySkill)
				playing = NoAnySkill;

			for (int i = 0; i < buttonName.Length; i++) {
				if (Input.GetButtonDown (buttonName [i]) && skillList [i].CD.isDone) {
					skillIndex = i;
					UsingSkill (skillIndex);
				}
			}
		} 
	}

	virtual public void UsingSkill(int inputIndex){
		self.CmdSetAnimInt("SkillInt",skillIndex);
//		self.CmdSetSkillLayer ();
		CmdUsingSkill (inputIndex);
	}

	[Command]
	virtual public void CmdUsingSkill(int inputIndex){
//		Debug.Log ("CmdUseskill" + gameObject.name + ":" +  skillList[skillIndex].name);
		RpcUsingSkill (inputIndex);
	}

	[ClientRpc]
	virtual public void RpcUsingSkill(int inputIndex){
		if(skillIndex != 0)
			UI.CountCD (inputIndex-1,skillList[skillIndex].CD.value);
		skillList [inputIndex].CD.Count ();
		skillList [inputIndex].skillNumber = skillIndex;
		playing = skillList[inputIndex].PlayingSkill;
		weapon.damage = skillList[inputIndex].damage;
		weapon.fatigue = skillList[inputIndex].fatigue;
		weapon.onHit = skillList[inputIndex].HitSomeOne;
		weapon.effect = skillList[inputIndex].effect;
		weapon.projection = skillList[inputIndex].projection;
		weapon.force = skillList [inputIndex].force;
		if(skillList [inputIndex].skillEffect)
			weapon.skillEffect = skillList [inputIndex].skillEffect;
		start = skillList[inputIndex].StartSKill ;
		start ();
		usingSkill = true;
	}
}

