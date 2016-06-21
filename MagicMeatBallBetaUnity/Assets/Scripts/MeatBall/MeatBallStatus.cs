using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MeatBallStatus : NetworkBehaviour {

	[SyncVar(hook="OnHPChange")]
	public float HP;
	/*[SyncVar]
	public float MP;*/

	public float damage;

	public float MaxHP;
	//public float MaxMP;

	void OnHPChange (float newHP){
		HP = newHP;
		GetComponent<Combat> ().SetHpValue ();
	}


	// Use this for initialization
	void Awake () {
		HP = MaxHP;
		//MP = MaxMP;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
