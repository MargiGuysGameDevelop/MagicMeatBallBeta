using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Prototype.NetworkLobby;

public class PressToLoadScene : MonoBehaviour {

	[SerializeField]
	float waitSec;

	float ableTime;

	[SerializeField]
	string pressKey;

	[SerializeField]
	int loadSceneLevel;


	void OnEnable(){
		ableTime = Time.realtimeSinceStartup + waitSec;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.realtimeSinceStartup > ableTime) {
			if (Input.anyKey) {
				LobbyManager.s_Singleton.GoBackButton ();
			}
		}


	}
}
