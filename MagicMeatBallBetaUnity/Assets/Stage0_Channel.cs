using UnityEngine;
using System.Collections;

public class Stage0_Channel : MonoBehaviour {

	public delegate void TriggerEvent(Transform trans);
	public TriggerEvent onTriggerEnter;
	public TriggerEvent onTriggerExit;

	void OnTriggerEnter(Collider other){
		if (other.GetComponent<MeatBall> ())
		if(onTriggerEnter != null)onTriggerEnter (transform);
	}

	void OnTriggerExit(Collider other){
		if (other.GetComponent<MeatBall> ()){
			if(onTriggerExit != null)onTriggerExit (transform);
		}
	}
}
