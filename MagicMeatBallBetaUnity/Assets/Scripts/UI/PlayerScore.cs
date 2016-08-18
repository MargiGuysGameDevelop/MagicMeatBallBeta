using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {

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

	public void ChangeData(string[] data){
		for (int i = 0; i < dataProperty.Length; i++) {
			dataProperty [i].text = data [i];
		}
	}
}
