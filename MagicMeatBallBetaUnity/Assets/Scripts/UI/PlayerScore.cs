using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerScore : NetworkBehaviour {

	public string[] testData;

	Text[] dataProperty;

	void Awake(){
		dataProperty = GetComponentsInChildren<Text> ();
	}

	[ContextMenu("ChangeDataInEditor")]
	public void ChangeDataTest(){
		dataProperty = GetComponentsInChildren<Text> ();
		ChangeData (testData);
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
