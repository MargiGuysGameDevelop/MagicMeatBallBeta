using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MosterSkillManager : SkillManager {

	Moster moster;

	MosterSkill[] mosterSkillList;

	public float distanceWithTarget;
	public float targetSpeed;
	public float angleWithTarget;

	public override void NeedToStart ()
	{
		moster = GetComponent<Moster> ();
		mosterSkillList = GetComponentsInChildren<MosterSkill> ();
		weapon = GetComponentInChildren<Weapon>();
		playing = NoAnySkill;
	}

//	[Command]
	public override void UsingSkill (int inputIndex)
	{
		moster.CmdSetAnimInt ("SkillInt",inputIndex);
		mosterSkillList [inputIndex].CD.Count ();
		mosterSkillList [inputIndex].skillNumber = inputIndex;
		playing = mosterSkillList[inputIndex].PlayingSkill;
		weapon.damage = mosterSkillList[inputIndex].damage;
		weapon.fatigue = mosterSkillList[inputIndex].fatigue;
		weapon.onHit = mosterSkillList[inputIndex].HitSomeOne;
		weapon.effect = mosterSkillList[inputIndex].effect;
		weapon.projection = mosterSkillList[inputIndex].projection;
		weapon.force = mosterSkillList [inputIndex].force;
		if(mosterSkillList [inputIndex].skillEffect)
			weapon.skillEffect = mosterSkillList [inputIndex].skillEffect;
		start = mosterSkillList[inputIndex].StartSKill ;
		start ();
		usingSkill = true;
	}

	public override void NeedToUpdate ()
	{

		if (!moster.IsSkillable () || moster.IsHurt ()) {
			return;
		}

		for (skillIndex = mosterSkillList.Length - 1; skillIndex >= 0; skillIndex--) {
			if (mosterSkillList [skillIndex].IsUseSkill (distanceWithTarget,angleWithTarget)) {
				UsingSkill (skillIndex);
				return;
			}
		}
	}
}
