using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SuitDisplayOnDrop : MonoBehaviour {

	MeatBallSuitDisplay display;

	Dropdown dropDown;

	void Awake(){
		dropDown = GetComponentInChildren<Dropdown> ();
		display = GameObject.FindObjectOfType<MeatBallSuitDisplay> ();
	}

	void Start(){
		display.SuitChange (0);
	}

	public void ChangeSuit(){
		display.SuitChange (dropDown.value);
	}
}
