using UnityEngine;
using System.Collections;


public class Weapon : MonoBehaviour {
	public float damage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		

	void OnTriggerEnter(Collider other){
		print ("triggeriN");
		Combat combat = other.GetComponent<Combat> ();
		if (combat) {
			//print ("find combat");
			combat.TakeDamage (damage);
		}

	}


}
