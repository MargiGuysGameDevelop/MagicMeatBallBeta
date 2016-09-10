using UnityEngine;
using System.Collections;

public class OnStateExitSetBool : MeatBallSBMBoolList {


	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		SetAllBool (animator);
	}
}
