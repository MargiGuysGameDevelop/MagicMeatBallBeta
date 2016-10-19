using UnityEngine;
using System.Collections;

public class OnStateEnterSetBool : MeatBallSBMBoolList {


	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		SetAllBool(animator);
	}
		
}
