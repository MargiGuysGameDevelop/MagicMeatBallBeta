using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MeatBallSuitDisplay : MonoBehaviour {

	[SerializeField]
	Camera camera;

	Animator anim;

	SuitList suitList;

	void Awake(){
		anim = GetComponent<Animator> ();
		suitList = GetComponentInChildren<SuitList> ();
	}
		
	void Start(){
		if (this.camera) {
			
		}
	}

	void Update(){
//		if (anim) {
//			anim.SetBool ("OnGround", true);
//		}

	}

	public void SuitChange(int input){
		suitList.InitialSuit (input);
	}

	public void AnimatorInitial(){}
}
