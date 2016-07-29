using UnityEngine;
using System.Collections;

public class GeneralAttackState : StateMachineBehaviour {
	[SerializeField]
	float attackBoolStartTime;
	[SerializeField]
	float attackBoolKeepTime;


	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.gameObject.GetComponent<MeatBall> ().GeneralAttack (attackBoolStartTime,attackBoolKeepTime);
	}
}
