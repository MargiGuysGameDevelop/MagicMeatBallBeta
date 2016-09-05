using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

using Prototype.NetworkLobby;
public class MeatBallStatus : NetworkBehaviour {

	[SyncVar]
	public bool isDead = false;
//	看起來用不到
//	public bool isDyingAniPlaying;
	//死亡復活計時器
	public float deadTimer = 0;
	static public float rebrithTime = 3f;

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

	//存放最後攻擊者
	public int attacker = -1;

	public float MaxHP;
	//public float MaxMP;

	public int currentWeapon = 0; //default =0

	//id
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
			deadTimer += Time.deltaTime;
			if (deadTimer >= rebrithTime) {
				GetComponent<MeatBall> ().CmdInitAnim ();
				PlayerRebrith ();
				HP = MaxHP;
				deadTimer = 0f;
				isDead = false;
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
			if (!isDead) {
				isDead = true;
				PlayerDie ();
			}
			return true;
		}
	}
		
	void PlayerDie(){
		GetComponent<MeatBall> ().CmdSetAnimBool ("Dead",true);;
		Debug.Log (this.gameObject.name + "被"+
			GameManager.playerSenceData[attacker].gameObject.name + "殺死!");
		GameManager.ChangeScoreData (playerNetId,ScoreKind.death);
		GameManager.ChangeScoreData (attacker,ScoreKind.kill);
	}

	void PlayerRebrith(){
		GetComponent<MeatBall> ();
	}

	public int GetPlayerNetId(){
		playerNetId = int.Parse (GetComponent<NetworkIdentity> ().netId.ToString ());
		return playerNetId;
	}
		

}
