using UnityEngine;
using System.Collections;

public class OnStateExitSetBool : StateMachineBehaviour {
	[SerializeField]
	string boolName;
	[SerializeField]
	bool setBool;

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetBool (boolName, setBool);
	}
}
