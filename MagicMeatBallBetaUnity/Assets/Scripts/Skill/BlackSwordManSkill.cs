using UnityEngine;
using System.Collections;

public class BlackSwordManSkill : Skill {

	override public void StartSKill(){
		base.StartSKill ();
//		Debug.Log (skillNumber);
		if(skillNumber == 1){
			var phsics = GetComponentInParent <FunPhsics>();
//			Debug.Log (meatBallTran.name);
			var force = meatBallTran.forward;
			force.y = 0;
			force = force.normalized * 4f;
			phsics.RpcPush (1f,force);
		}
	}

}
