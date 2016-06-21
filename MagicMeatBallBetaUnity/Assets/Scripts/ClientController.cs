using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ClientController : NetworkBehaviour {
	public GameObject camera;
	// Use this for initialization
	void Start () {
		if(!isLocalPlayer){
			
			Destroy (camera);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
