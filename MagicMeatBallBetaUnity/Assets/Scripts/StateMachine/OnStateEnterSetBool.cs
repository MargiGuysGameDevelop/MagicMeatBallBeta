using UnityEngine;
using System.Collections;

public class OnStateEnterSetBool : StateMachineBehaviour {
	[SerializeField]
	string boolName;
	[SerializeField]
	bool setBool;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetBool (boolName, setBool);
	}
}
