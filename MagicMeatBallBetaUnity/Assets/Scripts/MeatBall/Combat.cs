using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;


public class Combat : NetworkBehaviour {
	public MeatBallStatus selfStatus;
	Text HPText ;

	void Awake () {
		selfStatus = GetComponent<MeatBallStatus> ();
		HPText = GetComponentInChildren<Text> ();
		HPText.text = selfStatus.HP.ToString();
	}
	// Update is called once per frame
	void Update () {

	}


	/**take damage only on sever**/
	[Command]
	public void CmdTakeDamage(float damage){
		selfStatus.HP -= damage;
		RpcTakeDamage ();
	}

	[ClientRpc]
	public void RpcTakeDamage(){
		SetHpValue ();
	}


	public void SetHpValue(){
		GetComponentInChildren<Slider> ().value = selfStatus.HP;
		HPText.text = selfStatus.HP.ToString ();
		//Debug.Log ("set HP : " + selfStatus.HP);
	}
}
