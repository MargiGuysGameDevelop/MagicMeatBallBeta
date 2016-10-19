using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerScore : NetworkBehaviour {

	public string[] currentData;

	public Text[] dataProperty;

	void Start(){
		dataProperty = GetComponentsInChildren<Text> ();
	}

	[ContextMenu("GetTextInEditor")]
	public void GetText(){
		dataProperty = GetComponentsInChildren<Text> ();
	}

//	public void ChangeDataTest(string[] data){
//		dataProperty = GetComponentsInChildren<Text> ();
//		ChangeData (data);
//	}

	public void ChangeData(string[] data){
		for (int i = 0; i < dataProperty.Length; i++) {
			dataProperty [i].text = data [i];
		}
	}
}
