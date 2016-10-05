using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public enum Stage0RoomType{
	BOSS  = 0,
	ManyMoster,
	Suvival,
	None
}

public class Stage0_Room : NetworkBehaviour {

	//產生新房間
	[SerializeField]
	GameObject room;
	Transform roomTran;
	/// 房間種類
	[SerializeField]
	Stage0RoomType roomType = Stage0RoomType.None;
	//GM
	public Stage0_GM GM;
	/// 通過關卡?
	public bool isClearStage = false;
	/// 進入關卡
	public bool isEnterStage = false;
	//update
	delegate void update();
	update updateFunction;
	//None情況下，計算過關所需時間
//	[SerializeField]
	float timer = 0f;
//	float destroyTime = 5f;
	public GameObject[] mosterType;
	//場上怪物數量
	public List<Moster> mosters;
	//開關門
	public Stage0Door backDoors;
	public Stage0Door[] forwardDoors;
	//通道
	public Stage0_Channel[] channels;

	void Awake(){
		roomTran = GetComponent<Transform> ();
		channels = GetComponentsInChildren<Stage0_Channel> ();
	}

	void Start () {
		if(GM.currentStage != 0)
			backDoors.OpenDoor ();
	}

	[ServerCallback]
	void OnTriggerEnter(Collider other){
		if (isClearStage)
			return;
		if (!isEnterStage) {
			isEnterStage = true;
		}
		StagePlaying() ;
	}

	[ServerCallback]
	void OnTriggerExit(){

		if (!isEnterStage || !isClearStage) {
			LogManager.Log ("尼太猛啦!怎麼辦到的R?\n\r快跟我們講吧!這個BUG必須修!");
			return;
		}
		//if player enterStage & CleaStage,player can go to nextStage.
		if (isEnterStage && isClearStage) {
			foreach (Stage0_Channel channel in channels) {
				channel.onTriggerEnter = NextStage ;
				channel.onTriggerEnter += CloseAllDoors ;
			}
		}
	}
	
	void Update () {
		if (updateFunction != null)
			updateFunction ();
	}

	void StagePlaying(){
		backDoors.CloseDoor();
		switch(roomType){
		case Stage0RoomType.BOSS:
			LogManager.Log ("您選中魔王關!你即將接受0A的洗禮!");
			BOSS ();
			updateFunction = ClearMosterUpdate ;
			break;
		case Stage0RoomType.ManyMoster:
			ManyMoster ();
			updateFunction = ClearMosterUpdate;
			LogManager.Log ("您抽中無雙關卡，準備大殺四方吧!");
			break;
		case Stage0RoomType.Suvival:
			updateFunction = SuvivalUpdate;
			LogManager.Log ("甚麼?是生存關卡?");
			break;
		case Stage0RoomType.None:
			LogManager.Log ("恭喜您直接過關~");
			updateFunction = NoneUpdate;
			break;
		default:
			LogManager.Log ("怎麼會這樣?Bug出現了!請來信通知!");
			break;
		}
	}
		
	void BOSS(){
		for(int i=0;i < GM.A ;i++){
//			Instantiate (mosterType[0]);
		}
	}

	void ManyMoster(){
		for(int i=0;i < GM.B ;i++){
//			Instantiate (mosterType[0]);
		}
	}

	void ClearMosterUpdate(){
		if (mosters.Count == 0)
			PassStage ();
	}

	void SuvivalUpdate(){
		timer += Time.deltaTime;
		if (timer >= GM.C) {
			PassStage ();
		}
	}

	void NoneUpdate(){
		timer += Time.deltaTime;
		if (timer >= 3f) {
			PassStage ();
		}
	}

	void PassStage(){
		GM.currentStage++;
		foreach (Stage0Door door in forwardDoors)
			door.OpenDoor ();	
		isClearStage = true;
		updateFunction = null;
	}

	[ContextMenu("測試+房")]
	[ServerCallback]
	void NextStage(Transform trans){
		foreach(Stage0Door door in forwardDoors){
			door.CloseDoor ();
		}
		//新房間
		Vector3 newRoomLocation = roomTran.position + trans.forward * 70f;
		var newRoom = Instantiate (room,newRoomLocation,trans.rotation) as GameObject;
		newRoom.name = "StageRoom" + GM.currentStage;
		//give type by random
		var newRoomScript = newRoom.GetComponent<Stage0_Room> ();
		newRoomScript.roomType = (Stage0RoomType)Random.Range(0,3);
		newRoomScript.backDoors.OpenDoor ();
		//GN
		newRoomScript.GM = this.GM;
		newRoomScript.isClearStage = false;
		newRoomScript.isEnterStage = false;
		newRoomScript.backDoors.onTriggerExit = DestoryRoom;
		//Networking Generate
		NetworkServer.Spawn (newRoom);
	}

	void DestoryRoom(){
		Debug.Log ("D");
		NetworkServer.Destroy (this.gameObject);
	}

	void CloseAllDoors(Transform trans){
//		foreach (Stage0Door door in forwardDoors)
//			door.CloseDoor ();
	}
		
}
