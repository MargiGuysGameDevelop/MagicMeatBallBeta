using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

using Prototype.NetworkLobby;
public class MeatBallStatus : NetworkBehaviour {



	[SyncVar]
	public bool isDead;
	public bool isDyingAniPlaying;

	Text NameText;
	Text HPText ;
	Slider HPSlider;

	[SyncVar]
	public string playerName;
	[SyncVar]
	public Color nameColor;
	[SyncVar]
	public float HP;
	[SyncVar]
	public float MP;

	[SyncVar]
	public int playerID;

	[SyncVar]
	public int scoreAmount;
	[SyncVar]
	public int deathAmount;
	[SyncVar]
	public int killAmount;

	//public float damage;

	public float MaxHP;
	//public float MaxMP;

	public int currentWeapon = 0; //default =0

	//id
//	[SyncVar]
	public int playerNetId = 0;
	public bool allreadyGetNetId = false;

	void Awake () {
		MaxHP = 100f;
		HP = MaxHP;
		//MP = MaxMP;
		TextInit ();
	}

	// Update is called once per frame
	void Update () {
		SetHpValue ();	
		if (isDead) 
		{
			if (isDyingAniPlaying) {
				PlayerDie ();
			}
			else {
			
			}
		}
	}



	void TextInit(){
		Text[] texts;
		texts = GetComponentsInChildren<Text> ();
		foreach(Text te in texts){
			if (te.name == "HpText")
				HPText = te;
			else if (te.name == "NameText")
				NameText = te;
		}
		HPText.text = HP.ToString();
		HPSlider = GetComponentInChildren<Slider> ();
		HPSlider.value = HP;
		NameText.text = playerName;
		//NameText.color = nameColor;

	}


	public override void OnStartLocalPlayer (){
		//GameObject.Find ("GameManager").GetComponent<GameManager> ().AddPlayer (this);
		int PID = (int)GetComponent<NetworkIdentity> ().netId.Value;
		CmdSetPlayerID (PID);
		//NameText.text = playerName;
	}

	[Command]
	void CmdSetPlayerID(int PID){
		playerID = PID;
	}	

	public void SetHpValue(){
		NameText.text = playerName;
		HPSlider.value = HP;
		HPText.text = HP.ToString ();
		//Debug.Log ("set HP : " + selfStatus.HP);
	}

	[ServerCallback]
	public bool CheckIsDead(){
		if (HP > 0) {
			return false;
		} else {
			isDyingAniPlaying = true;
			isDead = true;
			return true;
		}
	}

	void PlayerDie(){
		
	}

	public string GetPlayerNetId(){
		return GetComponent<NetworkIdentity> ().netId.ToString();
	}
		

}
