using UnityEngine;
using System.Collections;

public class DeadIfTouch : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player"){
			var meatBallCombat = other.GetComponent<Combat> ();
			var meatBall = other.GetComponent<MeatBall> ();
//			if(meatBall != null)
				meatBall.SetInvincibleFalse ();
			var MBS = meatBallCombat.GetComponent<MeatBallStatus> ();
			meatBallCombat.TakeDamage (100000f,
				MBS.attacker,
				1000f,Vector3.zero);
//			if (MBS.HP >= 0) {
//				MBS.HP = 0f;
//				MBS.CheckIsDead ();
//			}
		}
	}
}
