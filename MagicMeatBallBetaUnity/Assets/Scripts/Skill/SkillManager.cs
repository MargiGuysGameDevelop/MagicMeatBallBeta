﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SkillManager : MonoBehaviour {

	#region 基本欄位
	[SerializeField]
	string weaponSkillName;

	ExtraStates extraStates;
	Weapon weapon;
	MeatBallStatus selfStatus;
	#endregion

	#region 初始化

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

	#region 施展技能中
//	bool isSkillPlaying;
	public delegate bool SkillPlaying();
	public SkillPlaying playing;

	public bool NoAnySkill(){
		return true;
	}
	#endregion

	void Start(){
		selfStatus = GetComponentInParent<MeatBallStatus> ();

		extraStates = GetComponentInParent<ExtraStates> ();
		weapon = GetComponent<Weapon> ();

		UI = GameObject.Find ("SkillUI").GetComponentInChildren<SkillUIManager> ();

		UI.InitialSkills (skillList);
		for(int i=0;i<4;i++){ 
			UI.iconList [i].sprite = skillList [i+1].icon;
		}

		playing = NoAnySkill;

		skillList [0].GiveProperty (weapon,selfStatus,0);
//		Debug.Log (meatBall);

//		foreach(Skill item in skillList){
//			Debug.Log (item.name);
//		}
	}

	void Update(){
		if (Time.timeScale == 0)
			return;

		if (!selfStatus.isLocalPlayer)
			return;

		if(playing()){
			
			if(playing != NoAnySkill)
				playing = NoAnySkill;

			for(int i=0;i<buttonName.Length;i++){// 0 1 2 3 4 
				if (Input.GetButtonDown (buttonName [i]) && skillList [i].CD.isDone)
					skillIndex = i;
			}

			if (skillIndex != lastSkillIndex) {
				skillList[skillIndex].GiveProperty (weapon,selfStatus,skillIndex);
				skillList[skillIndex].StartSKill ();
				playing = skillList[skillIndex].PlayingSkill;
				skillList[skillIndex].CD.Count ();
				lastSkillIndex = skillIndex;
			}
			if (skillIndex != 0)
				skillIndex = 0;
		}
	}
}

