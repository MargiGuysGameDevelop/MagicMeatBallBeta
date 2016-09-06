﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

using Prototype.NetworkLobby;

public enum ScoreKind : int{
	name = 0,
	score,
	kill,
	death
}

public class GameManager : NetworkBehaviour {
	//MeatBallStatus[] playerList;
	//[SyncVar]
	//List<MeatBallStatus> playerList = new List<MeatBallStatus>();

	public MeatBallStatus[] playerList = new MeatBallStatus[0];

	[SyncVar]
	public int playerNumber;

	//建立NetID與MeatBallStatus的關連
	static public Dictionary<int,MeatBallStatus> playerSenceData = 
		new Dictionary<int, MeatBallStatus>();
	//建立NetID與ScoreBoradIndex的關連
	static public Dictionary<int,int> netToScoreBoradIndex = 
		new Dictionary<int,int>();
	// Use this for initialization
	/*
	void OnStartLocalPlayer(){
		
	}*/

	#region ScoreBoradUI
	static public ScoreBoard scoreBoard;

	/// <summary>
	/// 用以更改記分板的欄位數值(+1)，名字請用ChangeName
	/// </summary>
	/// <param name="netID">Net I.</param>
	/// <param name="kind">Kind.</param>
	static public void ChangeScoreData(int netID,ScoreKind kind){
		int scoreBoradIndex = netToScoreBoradIndex [netID];
		switch (kind) {
		case ScoreKind.name:
			Debug.Log ("用錯函式啦!");
			break;
		default:
			scoreBoard.ChangeValueData (scoreBoradIndex,(int)kind);
			break;
		}
	}

	/// <summary>
	/// 更改記分板名字專用~
	/// </summary>
	/// <param name="netID">Net I.</param>
	/// <param name="newName">New name.</param>
	static public void ChangeName(int netID,string newName){
		int scoreBoradIndex = netToScoreBoradIndex [netID];
		scoreBoard.ChangeName (scoreBoradIndex,newName);
	}
	#endregion

	#region UnityInteral
	void Start(){
		playerSenceData.Clear ();
		netToScoreBoradIndex.Clear ();
		scoreBoard = GameObject.FindGameObjectWithTag ("GM")
			.GetComponent<ScoreBoard>();
	}

	void Update(){
		
		UpdatePlayerNumber ();
		if (playerList.Length != playerNumber) {
			//playerList = new MeatBallStatus[playerNumber];
			playerList = Object.FindObjectsOfType<MeatBallStatus> ();
			FindAndSortPlayer ();
			PlayerDataDictionaryInitial ();
			RefreshPlayerScore ();
		}

		if (Input.GetKeyDown (KeyCode.Tab)) {
		}
	}
	#endregion

	#region PlayerIDSort
	[ServerCallback]
	void UpdatePlayerNumber(){
		playerNumber  = LobbyManager.s_Singleton.numPlayers;
	}

	void FindAndSortPlayer(){
		for(int i=0;i<playerList.Length;i++){
			var curr = playerList [i].GetPlayerNetId ();
			for(int j= i ;j<playerList.Length;j++){
				var next = playerList [j].GetPlayerNetId ();
				if (curr > next) {
					var currObject = playerList [i];
					playerList [i] = playerList [j];
					playerList [j] = currObject;
				}
			}
		}
	}

	void PlayerDataDictionaryInitial(){
		//全放進Dictionary
		for(int i =0;i<playerList.Length;i++){
			var ms = playerList [i];
			Debug.Log (ms.gameObject.name + ":" + ms.playerID);
			if (!playerSenceData.ContainsValue (ms)) {
				playerSenceData.Add (ms.playerNetId, ms);
				netToScoreBoradIndex.Add (ms.playerNetId,i);
			}
		}
		#if UNITY_EDITOR
		//		PrintAllPlayerID ();
		#endif
	}
	void GetAllPlayer(){
		playerList = GetComponents<MeatBallStatus> ();
	}
	void RefreshPlayerScore(){
		int index = 0;
		scoreBoard.InitialData ();

		for(index = 0;index<playerList.Length;index++){
			scoreBoard.ChangeName (netToScoreBoradIndex[playerList[index].playerNetId]
				,playerList[index].name);
		}
		for(index = playerList.Length;index <scoreBoard.playerData.Length ;index++){
			scoreBoard.ClearData (index);
		}
	}
	#endregion

	void PrintAllPlayerID(){
//		foreach(MeatBallStatus player in playerList){
//			Debug.Log (player.playerName + ":" + player.GetPlayerNetId());
//		}
//		foreach(KeyValuePair<int,MeatBallStatus> ms in playerSenceData){
//			Debug.Log (playerSenceData[ms.Key].name + ":" + ms.Key);
//		}
	}


}
