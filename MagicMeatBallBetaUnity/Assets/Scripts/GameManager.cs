using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

using Prototype.NetworkLobby;

public enum ScoreKind : int{
	name = 0,
	score,
	kill,
	death
}

public class GameManager : NetworkBehaviour {

	//static public GameManager singleton;
	//MeatBallStatus[] playerList;
	//[SyncVar]
	//List<MeatBallStatus> playerList = new List<MeatBallStatus>();

	public MeatBallStatus[] playerList = new MeatBallStatus[0];


	//玩家數量
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

	public GameManager GetGameManager(){
		return this;
	}

	#region ScoreBoradUI
	static public ScoreBoard scoreBoard;
	[Command]
	public void CmdRefreshScoreBoard (){
		RpcRefreshScoreBoard ();
	}

	[ClientRpc]
	public void RpcRefreshScoreBoard(){
		RefreshScoreBoard ();
	}

	public void RefreshScoreBoard(){
		int playerCount = 0;

		foreach(PlayerScore player in scoreBoard.playerData){
			//Debug.Log ("playerCount : " + playerCount);
			if (playerCount < playerNumber) {
				player.dataProperty [(int)ScoreKind.name].text = playerList [playerCount].name.ToString ();
				player.dataProperty [(int)ScoreKind.score].text = playerList [playerCount].scoreAmount.ToString ();
				player.dataProperty [(int)ScoreKind.kill].text = playerList [playerCount].killAmount.ToString ();
				player.dataProperty [(int)ScoreKind.death].text = playerList [playerCount].deathAmount.ToString ();
				playerCount++;
			}
		}

	}


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
		playerList = new MeatBallStatus[0];
		scoreBoard = GameObject.FindGameObjectWithTag ("GM")
			.GetComponent<ScoreBoard>();

		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = false;
	}

	void Update(){


		if (Input.GetKeyDown ("escape")) {
			if (Cursor.visible) {
				Cursor.lockState = CursorLockMode.Confined;
				Cursor.visible = false;
			} else {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}

		}

			
		
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
