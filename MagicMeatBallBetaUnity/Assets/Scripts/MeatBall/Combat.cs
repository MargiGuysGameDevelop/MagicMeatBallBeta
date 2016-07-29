using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;


public class Combat : NetworkBehaviour {
	public MeatBallStatus selfStatus;
	Text HPText ;
	Slider HPSlider;
	void Awake () {
		selfStatus = GetComponent<MeatBallStatus> ();

		HPText = GetComponentInChildren<Text> ();
		HPText.text = selfStatus.HP.ToString();
		HPSlider = GetComponentInChildren<Slider> ();
		HPSlider.value = selfStatus.HP;
	}
	// Update is called once per frame
	void Update () {
		if(isLocalPlayer)
			if (Input.GetKeyDown (KeyCode.K)) {
				TakeDamage (5f);
			}
		//*no way to check whether player Hp is sync*//
		SetHpValue ();
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

	public void SetHpValue(){
		HPSlider.value = selfStatus.HP;
		HPText.text = selfStatus.HP.ToString ();
		//Debug.Log ("set HP : " + selfStatus.HP);
	}
}
