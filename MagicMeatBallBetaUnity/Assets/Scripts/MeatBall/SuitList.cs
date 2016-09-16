using UnityEngine;
using System.Collections;

[System.Serializable]
public class Part{
	public GameObject rightHand;
	public GameObject leftHand;
	public GameObject body;
	public GameObject rightLeg;
	public GameObject leftLeg;
	public GameObject head;
	public GameObject back;
}

public class SuitList : MonoBehaviour {

	public Part body;

	public Suit[] kind ;

	[SerializeField]
	MeatBall meatBall;

	void Awake () {
	}

	public void InitialSuit(){
//		Debug.Log ("Su");
		if (meatBall) {
			for(int i=0;i<kind.Length;i++){
				kind [i].gameObject.SetActive (false);
			}
			var currSuit = kind [meatBall.selfStatus.currentWeapon];
			currSuit.gameObject.SetActive (true);
			currSuit.SetCloth (body.body);
			currSuit.SetLeftWeapon (body.leftHand);
			currSuit.SetRightWeapon (body.rightHand);
			currSuit.SetHat (body.head);
			currSuit.SetLeftShose (body.leftLeg);
			currSuit.SetRightShose (body.rightLeg);
			currSuit.SetCloak (body.back);
//			Debug.Log (meatBall.selfStatus.currentWeapon);
		}
	}

	[ContextMenu("重新整理套裝及貢丸Script")]
	void GetSuit(){
		meatBall = GetComponentInParent<MeatBall> ();
		kind = GetComponentsInChildren<Suit> ();
	}
}
