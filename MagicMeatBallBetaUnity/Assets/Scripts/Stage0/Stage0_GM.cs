using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Stage0_GM : GameManager {

	public int currentStage = 0;

	#region Stage0Parameters
	/// <summary>
	/// BossNumber
	/// </summary>
	/// <value>A.</value>
	public int A{
		get{
			int number;
			if (currentStage < 11)
				number = 1;
			else if (currentStage < 31)
				number = 2;
			else
				number = currentStage % 10;
			return number;
		}
	}
	/// <summary>
	/// mosterNumber
	/// </summary>
	/// <value>The b.</value>
	public int B{
		get{
			int number; 
			if (currentStage < 11)
				number = 20;
			else if(currentStage < 21)
				number =  30;
			else if(currentStage <31)
				number = 40;
			else 
				number = currentStage +15; 
			return number;
		}
	}
	/// <summary>
	/// fallingTimes
	/// </summary>
	/// <value>The c.</value>
	public int C{
		get{
			int number; 
			if (currentStage < 6)
				number = 5;
			else if(currentStage < 21)
				number =  currentStage-currentStage%5+1;
			else 
				number = currentStage-currentStage%5 +2; 
			return number;
		}
	}
	/// <summary>
	/// NoHurt%
	/// </summary>
	/// <value>The d.</value>
	public int D {
		get {
			int number; 
			if (currentStage < 6)
				number = 25;
			else if (currentStage < 21)
				number = 20;
			else
				number = 15; 
			return number;
		}
	}
	/// <summary>
	/// Damage
	/// </summary>
	/// <value>The e.</value>
	public int E{
		get{
			return currentStage / 5 + 1;
		}
	}
	#endregion   

	/// 房間種類
	[SerializeField]
	Stage0RoomType roomType = Stage0RoomType.None;
	//怪物種類
	public GameObject[] mosterType;
	string[] mosterName;
	public GameObject[] bossType;
	string[] bossName;
	public GameObject[] fallingType;
	string[] fallingName;
	//場上怪物 管理
	public List<Moster> mosters;
	//掉落物
	public List<GameObject> fallingThings;
	//目前房間
	[SerializeField]
	Stage0_Room currRoom;
	public Stage0_Room currentRoom{
		set{ 
			roomType = (Stage0RoomType)Random.Range(0,3);
			value.onTriggerEnter = StageStart;
			value.onTriggerExit = StageExit;
			transform.position = value.transform.position;
			currRoom = value; 
		}
	}
	/// 通過關卡?
	public bool isClearStage = false;
	/// 進入關卡
	public bool isEnterStage = false;
	//，計算過關所需時間
	float timer = 0f;
	//Falling情況下，計算何時該掉落物品
	float fallingCounter = 1f;
	//update
	delegate void NormalDelegate();
	NormalDelegate updateFunction;

	//UI
	public Text timeCounter;
	public Text stageName;

	public override void NeedToAwake ()
	{
		currRoom.onTriggerEnter = StageStart;
		currRoom.onTriggerExit = StageExit;

		mosterName = new string[mosterType.Length];
		for(int i=0;i<mosterType.Length;i++){
			mosterName[i] = mosterType [i].name;
		}
		bossName = new string[bossType.Length];
		for(int i=0;i<bossType.Length;i++){
			bossName[i] = bossType [i].name;
		}

	}

	void StageStart(){
		if(playerList[0] != null)
		playerList [0].DeadFun = NeetToDoInRefreshScoreBoard;

		if (!isEnterStage) {
			isEnterStage = true;
			currRoom.CloseBackDoor ();
			StagePlaying ();
		}
	}

	void StagePlaying(){
		currentStage++;
		currRoom.CloseForwardDoor (transform);
		LogManager.Clear ();
		stageName.text = currentStage.ToString () + ":";
//		CreateMosters  ();
//		CreateBoss();
//		return;
		switch(roomType){
		case Stage0RoomType.BOSS:
			stageName.text += "魔王關";
			LogManager.Log ("選中魔王關!你即將接受0A的洗禮!");
			CreateBoss ();
//			Falling ();
//			CreateMosters  ();
			break;
		case Stage0RoomType.ManyMoster:
			stageName.text += "無雙關";
			CreateMosters  ();
//			CreateBoss ();
//			Falling ();
			LogManager.Log ("抽中無雙關卡，準備大殺四方吧!");
			break;
		case Stage0RoomType.Suvival:
			stageName.text += "生存關";
			LogManager.Log ("生存關卡!小心A車和水桶!");
//			CreateBoss ();
//			CreateMosters  ();
			Falling ();
			break;
		case Stage0RoomType.None:
			stageName.text += "直接過關!";
			LogManager.Log ("恭喜您直接過關~3秒後就可走囉");
			None ();
			break;
		default:
			LogManager.Log ("怎麼會這樣!Bug出現了!請來信通知!");
			break;
		}
	}
		
	void StagePass(){
		timeCounter.text = "剩餘時間:X";
		isClearStage = true;
		timer = 0;
		currRoom.OpenForwardDoor ();
		currRoom = null;
	}

	bool StageExit(){
		//if player enterStage & CleaStage,player can go to nextStage.
		if (isEnterStage && isClearStage) {
			return true;
		} else {
//			if (!isEnterStage || !isClearStage) {
//				LogManager.Log ("尼太猛啦!怎麼辦到的R?\n\r快跟我們講吧!這個BUG必須修!");
//			}
			return false;
		}
	}

	[ServerCallback]
	public void CreateMosters(){
		timeCounter.text = "剩餘時間:X";
		for(int i =0;i<B;i++){
			int ramdon =  Random.Range (0,mosterType.Length);
			GameObject moster = Instantiate (mosterType[ramdon],
				currRoom.RandomPosition(),transform.rotation) as GameObject;
			Moster mosterScript = moster.GetComponent<Moster> ();
			mosters.Add (mosterScript);
			mosterScript.myIndexOnGM = mosters.Count - 1;
			moster.name = mosterName[ramdon] + (mosters.Count -1).ToString();
			mosterScript.player = playerList [0].GetComponent<Combat>();
			mosterScript.addKill = addKillAmount;
			moster.gameObject.SetActive(true);
			NetworkServer.Spawn (moster);
		}
		updateFunction = ManyMosterUpdate;
	}

	void addKillAmount(){
		playerList [0].killAmount++;
		RefreshScoreBoard ();
	}
				

	public void CreateBoss(){
		timeCounter.text = "剩餘時間:X";
		for(int i =0;i<A;i++){
			int ramdon =  Random.Range (0,bossType.Length);
			GameObject boss = Instantiate (bossType[ramdon],
				transform.position,transform.rotation) as GameObject;
			boss.name = bossName [ramdon];
			Moster bossScript = boss.GetComponent<Moster> ();
			bossScript.player =  playerList [0].GetComponent<Combat>();
			mosters.Add (bossScript);
			bossScript.gameObject.SetActive(true);
			bossScript.callMoster = this.CallMoster;
			bossScript.addScore = addScoreAmount ;
			NetworkServer.Spawn (boss);		
		}
		updateFunction = BossUpdate;
	}

	void addScoreAmount(){
		playerList [0].scoreAmount++;
		RefreshScoreBoard ();
	}

	public void Falling(){
		updateFunction = FallingUpdate;
	}

	public void None(){
		updateFunction = NoneUpdate;
	}

	void ManyMosterUpdate(){
		BossUpdate ();
	}

	void BossUpdate(){
		foreach(Moster moster in mosters){
			if (!moster.NeedToUpdate ()) {
				mosters.Remove(moster);
			}
		}
		if (mosters.Count == 0) {
			mosters.Clear();
			StagePass ();
			updateFunction = null;
		}
	}

	void FallingUpdate(){
		timeCounter.text = "剩餘時間:" + ((int)(C - timer)).ToString ();
		timer += Time.deltaTime;
		if (timer > fallingCounter ){
			for(int i=0;i<4*C;i++){
//				int random =  Random.Range (0,1);
//				Debug.Log (random);
				var fall = Instantiate (fallingThings[i%2] , currRoom.RandomPosition()  
					, transform.rotation)as GameObject;
				fall.SetActive (true);
				var	fallScript = fall.GetComponent<SkillProjection> ();
				fallScript.damage = 5*E;
				fallScript.forceY = 2f;
				fallScript.myMainTransform = fall.transform;
				NetworkServer.Spawn (fall);
			}
			fallingCounter += 1f;
		}
		if (timer >= C) {
			StagePass ();
			updateFunction = null;
			timer = 0f;
			fallingCounter = 1f;
		}
	}

	void NoneUpdate(){
		timeCounter.text = "剩餘時間:" + ((int)(3 - timer)).ToString ();
		timer += Time.deltaTime;
		if (timer >= 3f) {
			StagePass ();
			updateFunction = null;
		}
	}

	public override void NeetToUpdate ()
	{
		if(updateFunction != null)
			updateFunction ();
	}
		
	public void CallMoster(){
		for(int i=0;i<B/4;i++){
			GameObject moster = Instantiate (mosterType[0],
				currRoom.RandomPosition(),transform.rotation) as GameObject;
			Moster mosterScript = moster.GetComponent<Moster> ();
			mosters.Add (mosterScript);
			mosterScript.myIndexOnGM = mosters.Count - 1;
			moster.name = mosterName[0] + (mosters.Count -1).ToString();
			mosterScript.player = playerList [0].GetComponent<Combat>();
			moster.gameObject.SetActive(true);
			NetworkServer.Spawn (moster);
		}
	}

	public override void NeetToDoInRefreshScoreBoard ()
	{
		if (playerList [0].deathAmount >= 3) {
			if (!scoreBoard)
				scoreBoard = GameObject.FindObjectOfType<ScoreBoard> ();
			scoreBoard.SetScoreBoard (true);
			CmdGameOver (currentStage.ToString ());
		}
	}
}