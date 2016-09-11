using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

using Prototype.NetworkLobby;
public class MeatBallStatus : NetworkBehaviour {

	/// <summary>
	/// 很常用到我直接Awake抓
	/// </summary>
	public MeatBall meatBall;

	//自定義物理
	FunPhsics phsics;

	[SyncVar]
	public bool isDead = false;
//	看起來用不到
//	public bool isDyingAniPlaying;
	//死亡復活計時器
	public float deadTimer = 0;
	static public float rebrithTime = 3f;

	Text NameText;
	Text HPText ;
	Text EPText;
	Slider HPSlider;

	[SyncVar]
	public string playerName;
	[SyncVar]
	public bool isInvincible = false;
	[SyncVar]
	public Color nameColor;
	[SyncVar]
	public float HP;
	[SyncVar]
	public float MP;
	[SyncVar]
	public float EP; //耐力值
	[SyncVar]
	public int playerID;

	[SyncVar]
	public int scoreAmount;
	[SyncVar]
	public int deathAmount;
	[SyncVar]
	public int killAmount;

	//存放最後攻擊者
	[SyncVar]
	public int attacker = -1;

	float attackerResetTimer;

	public float MaxHP;
	public float MaxEP;
	//public float MaxMP;

	[SyncVar]
	public int currentWeapon = 0; //default =0

	//id
	public int playerNetId = 0;
	public bool allreadyGetNetId = false;


	void Awake () {
		MaxHP = 100f;
		MaxEP = 100f;
		HP = MaxHP;
		EP = MaxEP;
		//MP = MaxMP;
		TextInit ();

		meatBall = GetComponent<MeatBall> ();
		phsics = GetComponent<FunPhsics> ();
		isInvincible = false;
	}

	void Start(){
		meatBall.CmdSetAnimInt ("WeaponKind",currentWeapon);
	}

	// Update is called once per frame
	void Update () {
		if (Time.timeScale == 0)
			return;

		if (Input.GetKeyDown (KeyCode.E)) {
//			EP = 0f;
//			CheckIsHurt (-transform.forward);
			phsics.RpcAddForce (1f, 2f, 1f);
		}

		SetPresentValue ();	
		CaculateAnimPra ();
		ResetAttackerByTime ();
		if (isDead) 
		{
			
			deadTimer += Time.deltaTime;
			if (deadTimer >= rebrithTime) {
				PlayerRebrith ();
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
			else if (te.name == "EPText")
				EPText = te;
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

	public void SetPresentValue(){
		NameText.text = playerName;
		HPSlider.value = HP;
		HPText.text = HP.ToString ();
		EPText.text = EP.ToString ();
		//Debug.Log ("set HP : " + selfStatus.HP);
	}
	public void CaculateAnimPra(){
		bool isGround = Physics.Raycast (transform.position,Vector3.down,0.2f);
		meatBall.CmdSetAnimFloat ("YVelocity",phsics.GetYVelocity());
		meatBall.CmdSetAnimBool ("OnGround",isGround);
	}

	[ServerCallback]
	public void CheckIsHurt(Vector3 direction){
		meatBall.CmdSetAnimBool ("Hurt", true);

		if (EP <= 0f) {
			EP = 0f;
			if (direction != Vector3.zero) {
				meatBall.CmdSetAnimBool ("HitFly",true);
//				Vector3 forceDir = (Vector3.up - transform.forward)*2;
				phsics.RpcAddForce ( direction.x, direction.y, direction.z);
			}
		} 
//		else {
//			if (direction != Vector3.zero) {
//				Vector3 direction = Vector3.up - transform.forward;
//				phsics.RpcAddForce (direction.x, direction.y, direction.z);
//
//			}
//		}
	}


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
	[ServerCallback]	
	void PlayerDie(){
		meatBall.CmdSetAnimBool ("Dead",true);
		deathAmount += 1;
		if (attacker >= 0) {
			GameManager GM = FindObjectOfType<GameManager> ();
			if (GM) {
				MeatBallStatus attackerMeatBallStatus = GM.playerList [GameManager.netToScoreBoradIndex [attacker]];
				Debug.Log ("attacker:" + GameManager.netToScoreBoradIndex [attacker]);
				//if(attacker != -1)
				attackerMeatBallStatus.scoreAmount++;
				attackerMeatBallStatus.killAmount++;
				GM.CmdRefreshScoreBoard ();

				//JudgeIsGameOver
				if (GameManager.JudgeIsGameOver (attackerMeatBallStatus.scoreAmount)) {
					
					GM.CmdGameOver (attackerMeatBallStatus.playerName);
				}

			}
		}
		/*Debug.Log (this.gameObject.name + "被"+
			GameManager.playerSenceData[attacker].gameObject.name + "殺死!");
		GameManager.ChangeScoreData (playerNetId,ScoreKind.death);
		GameManager.ChangeScoreData (attacker,ScoreKind.kill);*/
	}

	[ServerCallback]
	void ResetAttackerByTime(){
		if (attacker != -1) {
			attackerResetTimer = 10;
		}
		if (attackerResetTimer > 0) {
			attackerResetTimer -= Time.deltaTime;
		} else {
			
		}
	}

	[ServerCallback]
	void PlayerRebrith(){


		RpcSetRespawn ();

		HP = MaxHP;
		EP = MaxEP;
		deadTimer = 0f;
		isDead = false;
			
	}

	public int GetPlayerNetId(){
		playerNetId = int.Parse (GetComponent<NetworkIdentity> ().netId.ToString ());
		return playerNetId;
	}

	[ClientRpc]
	void RpcSetRespawn(){
		Transform rebirthTransform = LobbyManager.s_Singleton.GetStartPosition ();
		transform.position = rebirthTransform.position;
		meatBall.CmdInitAnim ();
	}


}
