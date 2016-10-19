using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public enum Stage0DoorKind{
	For,
	Back
}

public class Stage0Door : MonoBehaviour {
	//doorType
	public Stage0DoorKind doorType = Stage0DoorKind.For;
	//if this bool true,door opening.
	public bool isDoorOpen  = false;

	MeatBall player;

	public delegate void DoorDelegate();
	public DoorDelegate onTriggerEntter;
	public DoorDelegate onTriggerExit;

	Vector3 orginPosition;

	void Awake(){
		orginPosition = transform.position;
	}

	void OnTriggerEnter(Collider other){
		if (onTriggerEntter != null)
			onTriggerEntter ();
	}

	void OnTriggerExit(Collider other){
		if (onTriggerExit != null)
			onTriggerExit ();
	}

	[ContextMenu("開門")]
	public void OpenDoor(){
//		if (isDoorOpen)
//			return;
		transform.position = orginPosition + new Vector3 (0f,4f,0f);
//		transform.localScale *= new Vector3 (0.5f,1f,1f);
		isDoorOpen = true;
	}

	[ContextMenu("關門")]
	public void CloseDoor(){
		transform.position = orginPosition;
//		transform.localScale = new Vector3 (1f,1f,1f);
		isDoorOpen = false;
	}

	public void ChangeDoorState(){
		if (transform.localScale.x == 1f)
			OpenDoor ();
		else
			CloseDoor ();
	}
}
