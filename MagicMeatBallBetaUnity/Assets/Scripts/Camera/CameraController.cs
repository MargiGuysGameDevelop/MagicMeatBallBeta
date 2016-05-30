using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject target; 
	public float moveSmooth;
	public float rotateSensitivity;



	void Start () {
		transform.SetParent(transform.parent.parent);


	}

	// Update is called once per frame
	void Update () {
		RotateAroundTarget();
		Follow();
	}

	void RotateAroundTarget(){
		float mouseMoveX = Input.GetAxis("Mouse X");
		transform.Rotate(Vector3.up , mouseMoveX * rotateSensitivity * Time.deltaTime);
	}

	void Follow(){

		transform.position = Vector3.Lerp( transform.position, target.transform.position, moveSmooth * Time.deltaTime);
	}

}
