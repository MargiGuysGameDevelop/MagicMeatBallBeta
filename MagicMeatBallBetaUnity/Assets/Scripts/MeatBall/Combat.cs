using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;


public class Combat : NetworkBehaviour {
	public MeatBallStatus selfStatus;
	// Use this for initialization
	void Awake () {
		//selfStatus = GetComponent<MeatBallStatus> ();
	}
	// Update is called once per frame
	void Update () {

	}


	/**take damage only on sever**/
	[Command]
	public void CmdTakeDamage(float damage){
		selfStatus.HP -= damage;
	}


	public void SetHpValue(){
		GetComponentInChildren<Slider> ().value = selfStatus.HP;
		Debug.Log ("set HP : " + selfStatus.HP);
	}
}
