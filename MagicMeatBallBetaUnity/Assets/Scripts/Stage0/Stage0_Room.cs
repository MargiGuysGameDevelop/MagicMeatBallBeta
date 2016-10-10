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
	//GM
	public Stage0_GM GM;

	//開關門
	public Stage0Door backDoors;
	public Stage0Door[] forwardDoors;
	//通道
	public Stage0_Channel[] channels;

	public delegate void TriggerActionVoid ();
	public delegate bool TriggerActionBool ();
	public TriggerActionVoid onTriggerEnter;
	public TriggerActionBool onTriggerExit;

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
		if(other.GetComponent<MeatBall>())
			onTriggerEnter ();
	}
		
	[ServerCallback]
	void OnTriggerExit(){
		if (onTriggerExit()) {
			foreach (Stage0_Channel channel in channels) {
				channel.onTriggerEnter = GoToNextStage ;
				channel.onTriggerEnter += CloseForwardDoor ;
			}
		}
	}

	public Vector3 RandomPosition(){
		Vector3 position = new Vector3 (
			roomTran.position.x + Random.Range(-15,15),
			0f,
			roomTran.position.z + Random.Range(-15,15)
		);
		return position;
	}

	[ServerCallback]
	void GoToNextStage(Transform trans){
		foreach(Stage0Door door in forwardDoors){
			door.CloseDoor ();
		}
		//新房間
		Vector3 newRoomLocation = roomTran.position + trans.forward * 70f;
		var newRoom = Instantiate (room,newRoomLocation,trans.rotation) as GameObject;
		newRoom.name = "StageRoom" + (GM.currentStage+1);
		var newRoomScript = newRoom.GetComponent<Stage0_Room> ();
		newRoomScript.backDoors.OpenDoor ();
		newRoomScript.GM = this.GM;
		newRoomScript.backDoors.onTriggerExit = DestoryRoom;
		GM.currentRoom = newRoomScript;
		GM.isClearStage = false;
		GM.isEnterStage = false;
		//Networking Generate
		NetworkServer.Spawn (newRoom);
	}

	void DestoryRoom(){
		if(this.gameObject)
			NetworkServer.Destroy (this.gameObject);
	}

	public void CloseForwardDoor(Transform trans){
		foreach (Stage0Door door in forwardDoors)
			door.CloseDoor ();
	}

	public void OpenForwardDoor(){
		foreach (Stage0Door door in forwardDoors)
			door.OpenDoor ();
	}
		
	public void CloseBackDoor(){
		backDoors.CloseDoor ();
	}

	public void OpenBackDoor(){
		backDoors.OpenDoor ();
	}
}
