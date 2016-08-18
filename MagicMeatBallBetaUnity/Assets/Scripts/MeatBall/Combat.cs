using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;


public class Combat : NetworkBehaviour {
	public MeatBallStatus selfStatus;



	void Awake () {
		selfStatus = GetComponent<MeatBallStatus> ();
	}
	// Update is called once per frame
	void Update () {
		if(isLocalPlayer)
			if (Input.GetKeyDown (KeyCode.K)) {
				TakeDamage (5f);
			}
		//*no way to check whether player Hp is sync*//

	}


	/**take damage only on sever**/
	[ServerCallback]
	public void TakeDamage(float damage){
		selfStatus.HP -= damage;
		//RpcTakeDamage ();
	}

	/*
	[ClientRpc]
	public void RpcTakeDamage(){
		SetHpValue ();
	}
	*/


}
