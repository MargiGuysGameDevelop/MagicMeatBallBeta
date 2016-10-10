using UnityEngine;
using System.Collections;


public enum MosterSkillDecideElement{
	None,
	Distance,
	Movement
}

public enum SkillTpye{
	Normal,
	Calling
}

public class MosterSkill : Skill {

	[SerializeField]
	MosterSkillDecideElement decideToUse = MosterSkillDecideElement.None;

	[SerializeField]
	SkillTpye skillType = SkillTpye.Normal;

	//Distance
	[SerializeField]
	float distance = 2f;
	[SerializeField]
	float minAngle = 5f;

	//Movement
	float targetVelocity;

	Moster self;

//	Stage0_GM GM;


	public override void NeetToAwake ()
	{
//		GM = GameObject.FindObjectOfType<Stage0_GM> ();
		selfTran = GetComponentInParent<Moster> ().transform;
		self = GetComponentInParent<Moster> ();
	}



	public override void StartSKill ()
	{
		if (name != "") {
			LogManager.Log (GetComponentInParent<Moster> ().name + "使出了" + name + "!");
//			if (skillType == SkillTpye.Calling)
//				GM.CallMoster ();
		}
	}

	public bool IsUseSkill(float distance,float angle){
		if (!CD.isDone)
			return false;

		if (Mathf.Abs (angle) > minAngle) {
			return false;
		}

		switch(decideToUse){

		case MosterSkillDecideElement.None:
			return true;

		case MosterSkillDecideElement.Distance:
			if (distance < this.distance) {
				return true;
			}
			break;

		case MosterSkillDecideElement.Movement:
			break;
		}
		return false;
	}


}
