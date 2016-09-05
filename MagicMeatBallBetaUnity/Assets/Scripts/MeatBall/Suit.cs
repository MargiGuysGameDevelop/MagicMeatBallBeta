using UnityEngine;
using System.Collections;

public class Suit : MonoBehaviour {

	public bool ifHands = false;

	public GameObject rightWeapon;
	public GameObject leftWeapon;
	public GameObject cloth;
	public GameObject rightShose;
	public GameObject leftShose;
	public GameObject hat;
	public GameObject cloak;

	public void SetRightWeapon(GameObject parent){
		if(rightWeapon)
		rightWeapon.transform.parent = parent.transform;
	}

	public void SetLeftWeapon(GameObject parent){
		if(leftWeapon)
		leftWeapon.transform.parent = parent.transform;
	}

	public void SetCloth(GameObject parent){
		if(cloth)
		cloth.transform.parent = parent.transform;
	}

	public void SetRightShose(GameObject parent){
		if(rightShose)
		rightShose.transform.parent = parent.transform;
	}

	public void SetLeftShose(GameObject parent){
		if(leftShose)
		leftShose.transform.parent = parent.transform;
	}

	public void SetHat(GameObject parent){
		if(hat)
		hat.transform.parent = parent.transform;
	}

	public void SetCloak(GameObject parent){
		if(cloak)
		cloak.transform.parent = parent.transform;
	}
		
}
