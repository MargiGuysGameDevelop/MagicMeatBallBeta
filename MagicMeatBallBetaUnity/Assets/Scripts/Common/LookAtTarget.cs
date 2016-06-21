using UnityEngine;
using System.Collections;


public class LookAtTarget : MonoBehaviour {

	public GameObject target;
	public string tagName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!target)
			setTarget ();
		
		gameObject.transform.LookAt (target.transform.position);
	}


	public void setTarget()	{
		target = GameObject.FindGameObjectWithTag (tagName);
	}
}
