﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MeatBall : NetworkBehaviour {
	/*
	private enum MeatBallState{ Movement,Jump };

	[SerializeField]
	MeatBallState meatBallState;
*/
	AnimatorStateInfo currentState; 
	Animator meatBallAnimator;

	public MeatBallStatus selfStatus;

	public float vertical;
	public float horizontal;

	public float rotateAngel;
	public float meatBallSpeed;

	bool jumpingBool;
	float jumpTimer;

	float jumpX;
	float jumpZ;

	//public GameObject playerView;
	// Use this for initialization

	private GameObject sceneCamera;
	private HpCanvas hpCanvas;

	[SerializeField]Weapon[] rightHandWeaponList;


	void Awake(){

		sceneCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		meatBallAnimator = GetComponent<Animator> ();
		selfStatus = GetComponent<MeatBallStatus> ();

		#region Weapon
		SortWeaponCode ();
		CloseAllWeapon ();
		OpenWeapon ();
		#endregion

		//hpCanvas = GetComponentInChildren<HpCanvas> ();
		//sceneCamera.SetActive (false);
	}

	void Start () {

		if (isLocalPlayer) {
			
			gameObject.name = "ME";
			gameObject.tag = "Player";
			gameObject.transform.Find ("amerture/root").tag = "PlayerBodyBone";
			//sceneCamera.SetActive (true);
			sceneCamera.GetComponent<CameraController>().Initial();
			//hpCanvas.Initial();
		}
	}


	// Update is called once per frame
	void Update () {
		//ensure player control himself
		if(!isLocalPlayer)
			return;
		//move
		Move ();

		//jump
		if (Input.GetKeyDown("space")) {
			Jump();
		}
		if (IsJumpping ()) {
			JumpMove (10);
		}
		DelaySetJump ();

		//Attack
		if (Input.GetMouseButtonDown (0)) {
			GeneralAttack ();
		}

		//skill




	}

	#region init
	void SortWeaponCode(){
		//get weapon
		rightHandWeaponList = GetComponentsInChildren <Weapon>();

		//sort
		var gameObjectArrayBuffer = GetComponentsInChildren <Weapon>();

		for(int i=0;i<gameObjectArrayBuffer.Length;i++){
			var weapon = gameObjectArrayBuffer [i].GetComponent<Weapon> ();
			rightHandWeaponList [weapon.weaponCode] = gameObjectArrayBuffer [i];
		}
	}

	public void CloseAllWeapon(){
		foreach(Weapon weapon in rightHandWeaponList){
			weapon.gameObject.SetActive(false);
		}
	}

	public void OpenWeapon(){
		rightHandWeaponList [selfStatus.currentWeapon].gameObject.SetActive(true);
	}
	#endregion
		
	void GeneralAttack(){
		rightHandWeaponList [selfStatus.currentWeapon].GetComponent<Weapon> ().SetAttackKeepTime (0.1f,0.3f);
	}



	void Move(){
		currentState = meatBallAnimator.GetCurrentAnimatorStateInfo (0);


		vertical = Input.GetAxis("Vertical"); //sw
		horizontal = Input.GetAxis("Horizontal"); //ad

		float mouseMoveX = Input.GetAxis("Mouse X");
		transform.Rotate(Vector3.up , mouseMoveX * 75f * Time.deltaTime);

		meatBallAnimator.SetFloat("Vertical",vertical);
		meatBallAnimator.SetFloat("Horizontal",horizontal);
	}

	#region Jump
	void DelaySetJump(){

		if (jumpingBool) {
			if (Time.time > jumpTimer + 0.5f)
				jumpingBool = false;
		}
	}


	public void Jump(){
		if (CanJump() ) {
			jumpX = horizontal;
			jumpZ = vertical;
			jumpingBool = true;
			jumpTimer = Time.time;
			CmdSetAnimTrigger ("Jump");
		}
	}
	private bool IsJumpping(){
		if (currentState.nameHash == Animator.StringToHash ("Base Layer.OrginJump2.5"))
			return true;
		else
			return false;
	}
	private bool CanJump(){
		//check state
		if (currentState.nameHash == Animator.StringToHash ("Base Layer.Movement") && !jumpingBool )
			return true;
		else
			return false;
	}
	private void JumpMove(float distance){
		if (isLocalPlayer) {
			Vector3 translate= new Vector3 (jumpX,0,jumpZ);

			float dis = 0;
			if (jumpX >= 0)
				dis += jumpX;
			else
				dis -= jumpX;
			if (jumpZ >= 0)
				dis += jumpZ;
			else
				dis -= jumpZ;
			dis *= 0.5f; //dis max=1 , min=0;

			//Debug.Log (translate);
			transform.Translate (translate.normalized*distance*dis*Time.deltaTime);
		}
	}
	#endregion 

	#region Network
	[Command]
	public void CmdSetAnimTrigger(string triggerName)
	{
		RpcSetAnimTrigger(triggerName);
	}

	[ClientRpc]
	public void RpcSetAnimTrigger(string triggerName)
	{
		meatBallAnimator.SetTrigger(triggerName);
	}
	#endregion 

}