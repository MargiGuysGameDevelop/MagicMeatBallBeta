using UnityEngine;
using System.Collections;

/********************************/
/*this script need some TAG******/
/*Tag : Player , PlayerBodyBone,*/
/********************************/
public class CameraController : MonoBehaviour {
	public Transform target;
	public Transform focus;
	public Transform playerBodyBone;

	public float moveSmooth;
	public float rotateSensitivity;

	[SerializeField]
	private float cameraDistance;
	public float cameraDistanceMax;
	public float cameraDistanceMin;


	[SerializeField]
	private float focusBallDistance;
	//public float focusBallDistanceMax;
	//public float focusBallDistanceMin;


	[SerializeField]
	private float cameraHeight;
	public float cameraHeightMax;
	public float cameraHeightMin;
	//controlled by Max & Min
	public float cameraHeightRate;

	[SerializeField]
	private float focusBallHeight;
	public float focusBallHeightMax;
	public float focusBallHeightMin;
	//controlled by Max & Min
	public float focusBallHeightRate;

	float offsetY;
	float deltaY;



	public void Initial () {

		//Debug.Log ("ini CALLED!");

		//transform.SetParent(transform.parent.parent);
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		playerBodyBone = GameObject.FindGameObjectWithTag ("PlayerBodyBone").transform;

		GameObject focusBall = new GameObject("Focus");
		focus = focusBall.transform;

		IniCameraVar ();

	}

	// Update is called once per frame
	void LateUpdate () {
		if (focus) {

			deltaY = playerBodyBone.position.y - offsetY;

			MouseScrollWheel ();
			LookUpOrDown ();
			FocusFollow ();
			Follow ();
			transform.LookAt (focus);
		}
	}

	
	void LookUpOrDown(){
		
		float mouseMoveY = Input.GetAxis("Mouse Y");


		focusBallHeight += mouseMoveY * focusBallHeightRate * 0.1f;

		cameraHeight -= mouseMoveY * cameraHeightRate * 0.1f;

		focusBallHeight = Mathf.Clamp (focusBallHeight , focusBallHeightMin, focusBallHeightMax);
		cameraHeight = Mathf.Clamp (cameraHeight, cameraHeightMin, cameraHeightMax);

		
	}

	/*

	void RotateAroundTarget(){
		float mouseMoveX = Input.GetAxis("Mouse X");
		transform.Rotate(Vector3.up , mouseMoveX * rotateSensitivity * Time.deltaTime);
	}*/

	void MouseScrollWheel(){
		float mouseScrollWheel = Input.GetAxis ("Mouse ScrollWheel");

		cameraDistance -= mouseScrollWheel*1.5f;

		cameraDistance = Mathf.Clamp (cameraDistance, cameraDistanceMin, cameraDistanceMax);


	}

	void FocusFollow(){
		Vector3 focusPos;
		focusPos = target.position;
		focusPos += target.forward.normalized * focusBallDistance;
		focusPos.y = focusBallHeight + deltaY;
		focus.position = focusPos;

	}

	void Follow(){
		Vector3 targetPos;
		targetPos = target.position;

		targetPos += -target.forward.normalized * cameraDistance;
		targetPos.y = cameraHeight + deltaY;
		transform.position = Vector3.Lerp( transform.position, targetPos, moveSmooth * Time.deltaTime);

	}



	void IniCameraVar(){
		//camera distance behinde
		cameraDistanceMax = 6f;
		cameraDistanceMin = 2f;
		cameraDistance = (cameraDistanceMax + cameraDistanceMin) / 2.0f;
		//follow slerp para
		moveSmooth = 20f;
		//
		cameraHeightMax = 4f;
		cameraHeightMin = -2f;
		cameraHeight = (cameraHeightMax+cameraHeightMin) / 2.0f;

		focusBallDistance = 10;

		focusBallHeightMax = 18f;
		focusBallHeightMin = -5f;
		focusBallHeight = (focusBallHeightMax + focusBallHeightMin) / 2f;

		offsetY = playerBodyBone.transform.position.y;

		cameraHeightRate = cameraHeightMax - cameraHeightMin;
		focusBallHeightRate = focusBallHeightMax - focusBallHeightMin;

	}
		
}
