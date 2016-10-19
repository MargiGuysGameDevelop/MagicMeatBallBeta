using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MeatBallSyncRotation : NetworkBehaviour {
	Transform meatBallTransform;

	[SyncVar]
	Quaternion meatBallServerRotation;

	[SerializeField]
	float lerp = 15;

	// Use this for initialization
	void Awake () {
		meatBallTransform = transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		SetRotationOnOtherClientLerp ();
		SendLocalPlayerRotationToServer ();
	}

	void SetRotationOnOtherClientLerp (){
		if (!isLocalPlayer) {
			meatBallTransform.rotation = Quaternion.Lerp (meatBallTransform.rotation,meatBallServerRotation , Time.deltaTime*lerp);
		}
	}

	[Command]
	void CmdSendClientRotationToServer (Quaternion meatBallRot){
		meatBallServerRotation = meatBallRot;
	}

	[ClientCallback]
	void SendLocalPlayerRotationToServer(){
		if (isLocalPlayer)
			CmdSendClientRotationToServer (meatBallTransform.rotation);
	}

}
