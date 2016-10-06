using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

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
	//場上怪物 管理
	public List<Moster> mosters;
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
	//None情況下，計算過關所需時間
	float timer = 0f;
	//update
	delegate void update();
	update updateFunction;

	public override void NeedToAwake ()
	{
		currRoom.onTriggerEnter = StageStart;
		currRoom.onTriggerExit = StageExit;
	}

	void StageStart(){
		if (!isEnterStage) {
			isEnterStage = true;
			currRoom.CloseBackDoor ();
			StagePlaying ();
		}
	}

	void StagePlaying(){
		currentStage++;
		currRoom.CloseForwardDoor (transform);
		CreateMosters  ();
		return;
		switch(roomType){
		case Stage0RoomType.BOSS:
			LogManager.Log ("您選中魔王關!你即將接受0A的洗禮!");
			CreateBoss ();
			break;
		case Stage0RoomType.ManyMoster:
			CreateMosters  ();
			LogManager.Log ("您抽中無雙關卡，準備大殺四方吧!");
			break;
		case Stage0RoomType.Suvival:
			LogManager.Log ("甚麼?是生存關卡?");
			Falling ();
			break;
		case Stage0RoomType.None:
			LogManager.Log ("恭喜您直接過關~");
			None ();
			break;
		default:
			LogManager.Log ("怎麼會這樣?Bug出現了!請來信通知!");
			break;
		}
	}
		
	void StagePass(){
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

	public void CreateMosters(){
		for(int i =0;i<B;i++){
			int ramdon =  Random.Range (0,mosterType.Length);
			GameObject moster = Instantiate (mosterType[ramdon],
				transform.position,transform.rotation) as GameObject;
			Moster mosterScript = moster.GetComponent<Moster> ();
			mosters.Add (mosterScript);
			mosterScript.myIndexOnGM = mosters.Count - 1;
			moster.name = "Moster" + (mosters.Count -1).ToString();
			mosterScript.deadFunc = RemoveMosterByIndex;
			NetworkServer.Spawn (moster);
		}
		updateFunction = ManyMosterUpdate;
	}

	void RemoveMosterByIndex(){
		mosters.Remove (mosters[mosters.Count -1 ]);
	}

	public void CreateBoss(){
		for(int i =0;i<A;i++){
			int ramdon =  Random.Range (0,mosterType.Length);
			GameObject moster = Instantiate (mosterType[ramdon],
				transform.position,transform.rotation) as GameObject;
			Moster mosterScript = moster.GetComponent<Moster> ();
			NetworkServer.Spawn (moster);
			mosters.Add (mosterScript);
		}
		updateFunction = BossUpdate;
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
		if (mosters.Count == 0) {
			mosters.Clear();
			StagePass ();
			updateFunction = null;
		}
	}

	void FallingUpdate(){
		timer += Time.deltaTime;
		if (timer >= C) {
			StagePass ();
			updateFunction = null;
			timer = 0;
		}
	}

	void NoneUpdate(){
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
}