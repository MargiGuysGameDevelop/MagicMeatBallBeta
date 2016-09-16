using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook 
{
	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
	{
		
		LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
		MeatBallStatus selfStatus = gamePlayer.GetComponent<MeatBallStatus>();
		selfStatus.playerName = lobby.playerName;
		selfStatus.nameColor = lobby.playerColor;
		selfStatus.currentWeapon = lobby.weaponCode;

	}


}
