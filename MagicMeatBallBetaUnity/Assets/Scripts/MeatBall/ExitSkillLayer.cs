using UnityEngine;
using System.Collections;

public class ExitSkillLayer : StateMachineBehaviour {

//	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
//		SetSkillLayer0 (animator);
//	}
//
	[SerializeField]
	int SkillIndex = 0;

	public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetInteger ("SkillInt",0);
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetBool ("Skillable",true);
//		Debug.Log ("ExitState");
//		SetSkillLayer0 (animator);
	}

	void SetSkillLayer0(Animator an){
		an.SetLayerWeight (3,0f);
	}
		
}
