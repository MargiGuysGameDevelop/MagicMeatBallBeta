using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MeatBallStatus : NetworkBehaviour {

	Text DebugText;
	int clickTime = 0;

	[SyncVar(hook="OnHPChange")]
	public float HP;
	/*[SyncVar]
	public float MP;*/

	//public float damage;

	public float MaxHP;
	//public float MaxMP;

	public int currentWeapon = 0; //default =0

	void OnHPChange (float newHP){
		HP = newHP;
		GetComponent<Combat> ().SetHpValue ();
	}


	// Use this for initialization
	void Awake () {
		DebugText = GameObject.Find ("DebugCanvas/Text").GetComponent<Text>();
		MaxHP = 100f;
		HP = MaxHP;
		//MP = MaxMP;
	}

	void OnEnabled(){
		
		GetComponent<Combat> ().SetHpValue ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			clickTime++;
			CmdDebugText (clickTime);
		}
	}

	[Command]
	void CmdDebugText(int click){
		DebugText.text = click.ToString ();
	}

}
