using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MeatBallSyncPosition : NetworkBehaviour {

	Transform meatBallTransform;

	[SyncVar]
	Vector3 meatBallServerPosition;

	[SerializeField]
	float lerp = 15;


	// Use this for initialization
	void Awake () {
		meatBallTransform = transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		SetPositionOnOtherClientLerp ();
		SendLocalPlayerPositionToServer ();
	}

	void SetPositionOnOtherClientLerp(){
		if (!isLocalPlayer) {
			meatBallTransform.position = Vector3.Lerp (meatBallTransform.position, meatBallServerPosition, Time.deltaTime * lerp);
		}
	}

	[Command]
	void CmdSendClientPositionToServer(Vector3 pos){
		meatBallServerPosition = pos;
	}

	[ClientCallback]
	void SendLocalPlayerPositionToServer(){
		if (isLocalPlayer) {
			CmdSendClientPositionToServer (meatBallTransform.position);
	
		}
	}
}
