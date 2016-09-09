using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MeatBall : NetworkBehaviour {
	
	//all the meatballs' animations & skills do in this script
	

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

	float dodgeX;
	float dodgeY;

	//public GameObject playerView;
	// Use this for initialization

	private GameObject sceneCamera;
	//private HpCanvas hpCanvas;

	public Weapon rightHandWeapon;


	void Awake(){

		sceneCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		meatBallAnimator = GetComponent<Animator> ();
		selfStatus = GetComponent<MeatBallStatus> ();

		#region Weapon
//		SortWeaponCode ();
//		CloseAllWeapon ();
//		OpenWeapon ();
		RoadWeapon();
		#endregion

		//hpCanvas = GetComponentInChildren<HpCanvas> ();
		//sceneCamera.SetActive (false);
	}

	void Start () {

		if (isLocalPlayer) {
			gameObject.tag = "Player";
			gameObject.transform.Find ("amerture/root").tag = "PlayerBodyBone";
			//sceneCamera.SetActive (true);
			sceneCamera.GetComponent<CameraController>().Initial();
			//hpCanvas.Initial();
		}
		gameObject.name = selfStatus.playerName;
	}


	public override void OnStartLocalPlayer ()
	{

		gameObject.name = selfStatus.playerName;
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
		//DelaySetJump ();

		//Attack
		if (Input.GetMouseButtonDown (0)) {
			CmdSetAnimBool ("Attack",true);
			/*CmdGeneralAttack ();*/
		}

		//dodge
		if(Input.GetKeyDown(KeyCode.LeftShift)){
			Dodge ();
		}

		//skill

		//rolling
		if(Input.GetKeyDown(KeyCode.LeftAlt)){
			CmdSetAnimBool ("Rolling",true);
		}else if(Input.GetKeyUp(KeyCode.LeftAlt)){
			CmdSetAnimBool("Rolling",false);
		}
	}

	#region init
	public void RoadWeapon(){
		rightHandWeapon = GetComponentInChildren<Weapon> ();
	}
	/*void SortWeaponCode(){
		//get weapon
		rightHandWeapon = GetComponentsInChildren <Weapon>();

		//sort
		var gameObjectArrayBuffer = GetComponentsInChildren <Weapon>();

		for(int i=0;i<gameObjectArrayBuffer.Length;i++){
			var weapon = gameObjectArrayBuffer [i].GetComponent<Weapon> ();
			rightHandWeapon [weapon.weaponCode] = gameObjectArrayBuffer [i];
		}
	}

	public void CloseAllWeapon(){
		foreach(Weapon weapon in rightHandWeapon){
			weapon.gameObject.SetActive(false);
		}
	}

	public void OpenWeapon(){
		rightHandWeapon [selfStatus.currentWeapon].gameObject.SetActive(true);
	}*/
	#endregion


	#region Attacktion
	[ServerCallback]
	public void GeneralAttack(float attackStartTime,float attackKeepTime){
		rightHandWeapon.SetAttackKeepTime (attackStartTime,attackKeepTime);
	}

	[Command]
	void CmdGeneralAttack(){
		rightHandWeapon.SetAttackKeepTime (0.1f,0.7f);
	}

	[ServerCallback]
	//只要Server的Coilder開並讓其他玩家扣HP就夠了
	public void AttackColliderOn(){
		rightHandWeapon.WeaponCoilderOn ();
	}

	[ServerCallback]
	public void AttackColliderOff(){
		rightHandWeapon.WeaponCoilderOff();
	}

	[Command]
	public void CmdProject(){
		Debug.Log ("發射物體");
		RpcProject ();
	}

	[ClientRpc]
	public void RpcProject(){
		
	}

	[ServerCallback]
	public void Invincible(){
		Debug.Log ("無敵");
	}
		

	#endregion

	#region Movement
	void Move(){
		currentState = meatBallAnimator.GetCurrentAnimatorStateInfo (0);

		if (meatBallAnimator.GetBool ("Dodge"))
			return;
		vertical = Input.GetAxis("Vertical"); //sw
		horizontal = Input.GetAxis("Horizontal"); //ad

		float mouseMoveX = Input.GetAxis("Mouse X");
		transform.Rotate(Vector3.up , mouseMoveX * 75f * Time.deltaTime);

		CmdSetAnimFloat("Vertical",vertical);
		CmdSetAnimFloat("Horizontal",horizontal);
	}


	#endregion

	#region Jump
	/*void DelaySetJump(){

		if (jumpingBool) {
			if (Time.time > jumpTimer + 0.5f)
				jumpingBool = false;
		}
	}*/


	public void Jump(){
		jumpX = horizontal;
		jumpZ = vertical;
		//jumpingBool = true;
		//jumpTimer = Time.time;
		CmdSetAnimBool ("Jump",true);
	}
	private bool IsJumpping(){
		if (currentState.nameHash == Animator.StringToHash ("Base Layer.OrginJump2.5"))
			return true;
		else
			return false;
	}
	/*
	private bool CanJump(){
		//check state
		if (currentState.nameHash == Animator.StringToHash ("Base Layer.Movement") && !jumpingBool )
			return true;
		else
			return false;
	}*/
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

	#region Dodge
	void Dodge(){
		/*
		dodgeX = dodgeY = 0f;

		if (horizontal > 0.5f)
			dodgeX = 1f;
		else if (horizontal < -0.5f) {
			dodgeX = -1f;
		} else if (vertical < 0.5f)
			dodgeY = -1f;
		else  
			dodgeY = 1f;
		*/
		CmdSetAnimFloat ("Horizontal",horizontal);
		CmdSetAnimFloat ("Vertical",vertical);

		CmdSetAnimBool ("Dodge",true);
	}
	#endregion

	#region AnimationParameterSeendingNetwork
	[Command]
	public void CmdInitAnim(){
		RpcInitAnim ();
	}

	[ClientRpc]
	public void RpcInitAnim(){
		Debug.Log ("重生");
		meatBallAnimator.Play ("Movement");
	}
		
	[Command]
	public void CmdSetAnimFloat(string boolName,float setFloat)
	{
		RpcSetAnimFloat(boolName,setFloat);
	}

	[ClientRpc]
	public void RpcSetAnimFloat(string boolName,float setFloat)
	{
		meatBallAnimator.SetFloat(boolName,setFloat);
	}


	[Command]
	public void CmdSetAnimBool(string boolName,bool boolState)
	{
		RpcSetAnimBool(boolName,boolState);
	}

	[ClientRpc]
	public void RpcSetAnimBool(string boolName,bool boolState)
	{
		meatBallAnimator.SetBool(boolName,boolState);
	}
		
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
