using UnityEngine;
using System.Collections;

public class BlackSwordManSkill : Skill {

	override public void StartSKill(){
//		base.StartSKill ();
//		Debug.Log (skillNumber);
		if(skillNumber == 1){
			var phsics = GetComponentInParent <FunPhsics>();
			var force = meatBallTran.forward;
			force.y = 0;
			force = force.normalized * 32f;
			phsics.CmdPushEqualVelocity (0.4f,force);
		}
	}

}
