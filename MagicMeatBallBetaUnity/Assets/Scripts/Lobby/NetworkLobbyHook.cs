using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook 
{
	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
	{
		
		LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
		MeatBallStatus meatBall = gamePlayer.GetComponent<MeatBallStatus>();
		meatBall.playerName = lobby.playerName;
		meatBall.nameColor = lobby.playerColor;
		meatBall.currentWeapon = lobby.weaponCode;
//		Debug.Log (meatBall.currentWeapon);
	}


}
