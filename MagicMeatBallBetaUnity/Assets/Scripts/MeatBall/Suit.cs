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

	public void AllOpen(){
		SetAll (true);
	}

	public void AllClose(){
		SetAll (false);
	}

	public void SetAll(bool input){
		if(rightWeapon)
			rightWeapon.SetActive (input);
		if(leftWeapon)
			leftWeapon.SetActive (input); 
		if(cloth)
			cloth.SetActive (input);
		if(rightShose)
			rightShose.SetActive (input); 
		if(leftShose)
			leftShose.SetActive (input); 
		if(hat)
			hat.SetActive (input);
		if(cloak)
			cloak.SetActive (input);
	}
		
}
