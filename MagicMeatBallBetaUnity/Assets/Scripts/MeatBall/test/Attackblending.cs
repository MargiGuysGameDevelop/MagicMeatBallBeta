using UnityEngine;
using System.Collections;

public class Attackblending : MonoBehaviour {

	Animator an;

	// Use this for initialization
	void Start () {
		an = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Mouse0))
			an.SetInteger ("AttackKind",1);
	}
}
