using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

using Prototype.NetworkLobby;

public class GameManager : NetworkBehaviour {
	//MeatBallStatus[] playerList;
	//[SyncVar]
	//List<MeatBallStatus> playerList = new List<MeatBallStatus>();

	public MeatBallStatus[] playerList = new MeatBallStatus[0];

	public int playerNumber;
	// Use this for initialization
	/*
	void OnStartLocalPlayer(){
		
	}*/

	void Update(){
		playerNumber  = LobbyManager.s_Singleton.numPlayers;

		if (playerList.Length != playerNumber) {
			//playerList = new MeatBallStatus[playerNumber];
			playerList = Object.FindObjectsOfType<MeatBallStatus> ();
		}
	}
	void GetAllPlayer(){
		
		playerList = GetComponents<MeatBallStatus> ();
	}



}
