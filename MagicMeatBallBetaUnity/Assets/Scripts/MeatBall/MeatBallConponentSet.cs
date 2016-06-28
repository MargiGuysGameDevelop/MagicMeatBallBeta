using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEditor;

#region NetWork
[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(NetworkAnimator))]
#endregion

#region game
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
#endregion

#region script
[RequireComponent(typeof(MeatBallMove))]
[RequireComponent(typeof(MeatBallStatus))]
[RequireComponent(typeof(Combat))]
[RequireComponent(typeof(ClientController))]
#endregion

public class MeatBallConponentSet : MonoBehaviour {


	public void SetUp(){
		Animator an = GetComponent<Animator> ();
		an.applyRootMotion = true;

		NetworkIdentity nid = GetComponent<NetworkIdentity> ();
		nid.localPlayerAuthority = true;

		NetworkTransform ntr = GetComponent<NetworkTransform> ();
		ntr.transformSyncMode = NetworkTransform.TransformSyncMode.SyncTransform;
		ntr.syncRotationAxis = NetworkTransform.AxisSyncMode.AxisY;

		NetworkAnimator nAn = GetComponent<NetworkAnimator> ();
		nAn.animator = an;
		/*bug
		if(an.parameterCount != 0)
		for (int i = 0; i < an.parameterCount; i++) {
				nAn.SetParameterAutoSend (i,true);
		}*/

		CapsuleCollider cap = GetComponent<CapsuleCollider> ();
		cap.center = new Vector3 (0f,0.95f,0f);
		cap.radius = 0.62f;
		cap.height = 2f;
		cap.direction = 1;

		Rigidbody rig = GetComponent<Rigidbody> ();
		rig.constraints = RigidbodyConstraints.FreezeRotation;

		MeatBallMove move = GetComponent<MeatBallMove> ();
		move.meatBallSpeed = 4;

		MeatBallStatus sta = GetComponent<MeatBallStatus> ();
		sta.damage = 20;
		sta.MaxHP = 100;

		Combat com = GetComponent<Combat> ();
		com.selfStatus = sta;

		ClientController client = GetComponent<ClientController> ();

		CameraController cc = GetComponentInChildren<CameraController> ();
		cc.target = this.gameObject;
	}

}

[CustomEditor(typeof(MeatBallConponentSet))]
public class MeatBallConponentSetUI :Editor{
	public override void OnInspectorGUI(){
		if (GUILayout.Button ("初始化物件")) {
			((MeatBallConponentSet)target).SetUp();
		}
	}
}
