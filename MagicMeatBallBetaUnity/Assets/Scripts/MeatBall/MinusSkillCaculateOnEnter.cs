using UnityEngine;
using System.Collections;

public class MinusSkillCaculateOnEnter : StateMachineBehaviour {

	[SerializeField]
	public int numder = 1;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetInteger ("SkillCaculate",animator.GetInteger("SkillCaculate") - numder);
	}


}
