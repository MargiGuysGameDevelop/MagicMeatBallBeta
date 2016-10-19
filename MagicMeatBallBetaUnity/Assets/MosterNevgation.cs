using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MosterNevgation : NetworkBehaviour {

	public enum MoveType{
		RootMotion,
		Constant
	}
	public MoveType moveType = MoveType.RootMotion;
	[Header("if is RootMotion,it will be ignore")]
	public float walkSpeed;
	float angleSpeed = 40f;

	//self
	Transform myTran;
//	Animator myAnim;
	Moster mySelf;

	//palyer
	Transform playerTrans;
	Vector3 target;

	public float distanceWithPlayer{
		get{ return playerTrans == null ? 
			9999f : Vector3.Distance
			(target, myTran.position);
		}
	}

	void Awake(){
		myTran = transform;
//		myAnim = GetComponent<Animator> ();
		mySelf = GetComponent<Moster> ();
	}

	[ServerCallback]
	public float LookTarget(){
		if (playerTrans == null)
			playerTrans = mySelf.player.transform;

		target = playerTrans.position;
		target.y = 0f;

		var myPosition = myTran.position;
		myPosition.y = 0;

		var relation = target - myPosition;

		var angleWithTarget = Vector3.Angle (myTran.forward,relation);


		if (angleWithTarget <= angleSpeed)
			myTran.LookAt (target);
		else {
			if (Vector3.Cross (myTran.forward, relation).y < 0f) {
				if (angleSpeed > 0f)
					angleSpeed *= -1;
			} else {
				if (angleSpeed < 0f)
					angleSpeed *= -1;
			}
			myTran.Rotate (0f,angleSpeed * Time.deltaTime,0f);
		}

		return angleWithTarget -angleSpeed * Time.deltaTime ;
	}

	[ServerCallback]
	public bool Follow(){
		if (Time.timeScale == 0f)
			return false;

		if (mySelf.player == null)
			return false;
		
		if (playerTrans == null)
			playerTrans = mySelf.player.transform;

		mySelf.CmdSetAnimFloat ("movement",1f);

		if (moveType == MoveType.Constant)
			myTran.Translate (myTran.forward * walkSpeed *Time.deltaTime,Space.World);

		return true;
	}
}
