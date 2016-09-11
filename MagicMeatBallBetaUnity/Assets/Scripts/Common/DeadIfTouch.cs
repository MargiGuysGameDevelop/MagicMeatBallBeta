using UnityEngine;
using System.Collections;

public class DeadIfTouch : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		var meatBallCombat = other.GetComponent<Combat> ();
		if (other != null){
			var meatBall = other.GetComponent<MeatBall> ();
			meatBall.SetInvincibleFalse ();
			var MBS = meatBallCombat.GetComponent<MeatBallStatus> ();
			meatBallCombat.TakeDamage (100000f,
				MBS.attacker,
				1000f
				,Vector3.zero);
			if (MBS.HP >= 0)
				MBS.HP = 0f;
		}
	}
}
