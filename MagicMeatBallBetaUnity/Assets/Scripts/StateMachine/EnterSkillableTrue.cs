using UnityEngine;
using System.Collections;

public class EnterSkillableTrue : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetBool ("Skillable",true);
		SetSkillLayer0 (animator);
//		Debug.Log ("StartState");
	}

	void SetSkillLayer0(Animator an){
		an.SetLayerWeight (3,0f);
	}
}
