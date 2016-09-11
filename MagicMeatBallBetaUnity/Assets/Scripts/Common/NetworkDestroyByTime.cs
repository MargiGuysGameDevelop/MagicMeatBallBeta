using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkDestroyByTime : NetworkBehaviour {

	public float time;

	void Update(){
		if (time > 0f)
			time -= Time.deltaTime;
		else {
			NetworkServer.Destroy (this.gameObject);
		}
	}
}
