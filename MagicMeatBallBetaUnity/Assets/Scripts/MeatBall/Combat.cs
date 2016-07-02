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

	/*take damege is current by local this version*/
	public void TakeDamage(float damage){
		if (!isServer)
			return;
		
		selfStatus.HP -= damage;
		SetHpValue ();
	}


	public void SetHpValue(){
		GetComponentInChildren<Slider> ().value = selfStatus.HP;
	}
}
