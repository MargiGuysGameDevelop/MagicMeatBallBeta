using UnityEngine;
using System.Collections;

public class IniOnLobbyScene : MonoBehaviour {


	void OnLevelWasLoaded(int level)
	{
		if (level == 0) {
			Time.timeScale = 1;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.Confined;
		}
	}
}
