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

	[SyncVar]
	public int playerNumber;
	// Use this for initialization
	/*
	void OnStartLocalPlayer(){
		
	}*/

	#region ScoreBoradUI
	public ScoreBoard scoreBoard;
	#endregion

	void Update(){
		
		UpdatePlayerNumber ();

		if (playerList.Length != playerNumber) {
			//playerList = new MeatBallStatus[playerNumber];
			playerList = Object.FindObjectsOfType<MeatBallStatus> ();

			RpcFindAndSortPlayer ();
		}

		if (Input.GetKeyDown (KeyCode.Tab)) {
			RefreshPlayerScore ();
		}
	}

	[ServerCallback]
	void UpdatePlayerNumber(){
		playerNumber  = LobbyManager.s_Singleton.numPlayers;
	}

//	[ClientRpc]
	void RpcFindAndSortPlayer(){
		#if UNITY_EDITOR
			PrintAllPlayerID ();
		#endif
		for(int i=0;i<playerList.Length;i++){
			var curr = int.Parse(playerList [i].GetPlayerNetId ());
			for(int j= i ;j<playerList.Length;j++){
				var next = int.Parse (playerList [j].GetPlayerNetId ());
				if (curr > next) {
					var currObject = playerList [i];
					playerList [i] = playerList [j];
					playerList [j] = currObject;
				}
			}
		}
	}

	void GetAllPlayer(){
		playerList = GetComponents<MeatBallStatus> ();
	}

	void RefreshPlayerScore(){
		for(int i=0;i<playerList.Length;i++){
			scoreBoard.ChangeData (i, playerList [i].name, 0, 0, 0);
		}
	}

	void PrintAllPlayerID(){
		foreach(MeatBallStatus player in playerList){
			Debug.Log (player.playerName + ":" + player.GetPlayerNetId());
		}
	}


}
