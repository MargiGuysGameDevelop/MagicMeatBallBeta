using UnityEngine;
using System.Collections;

public class HpCanvas : MonoBehaviour {


	private Transform sceneCameraTransform;
	private Transform followTargetTransform;

	[SerializeField] float offsetY;

	// Use this for initialization
	void Awake () {
	}

	void Start(){
		sceneCameraTransform = GameObject.FindGameObjectWithTag ("MainCamera").transform;
		offsetY = 0.4f;
	}


	// Update is called once per frame
	void Update () {
		if (!followTargetTransform)
			Initial ();
		else {
			Follow ();
			LookAtCamera ();
		}
	}
	void Follow(){
		Vector3 targetPos = followTargetTransform.position;
		targetPos.y += offsetY;

		transform.position = targetPos;
	}

	void LookAtCamera(){
		gameObject.transform.LookAt (sceneCameraTransform.position);
	}


	public void Initial(){
		followTargetTransform = gameObject.transform.parent.Find ("amerture/root");
	}
}
