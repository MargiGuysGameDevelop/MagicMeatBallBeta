using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MeatBallStatus : NetworkBehaviour {



	[SyncVar]
	public float HP;
	/*[SyncVar]
	public float MP;*/

	//public float damage;

	public float MaxHP;
	//public float MaxMP;

	public int currentWeapon = 0; //default =0



	// Use this for initialization
	void Awake () {
		MaxHP = 100f;
		HP = MaxHP;
		//MP = MaxMP;
	}

	void OnEnabled(){
		
		GetComponent<Combat> ().SetHpValue ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
