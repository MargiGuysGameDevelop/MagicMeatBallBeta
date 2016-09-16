using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MeatBall : NetworkBehaviour {
	
	//all the meatballs' animations & skills do in this script
	

	AnimatorStateInfo currentState; 
	Animator meatBallAnimator;

	public MeatBallStatus selfStatus;
	public CapsuleCollider bodyCollider;

	bool isMoveable = true;

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
		bodyCollider = GetComponent<CapsuleCollider> ();
		var suitList = GetComponentInChildren<SuitList> ();
		suitList.InitialSuit ();
		var skillList = GetComponentInChildren<SkillManager> ();
		skillList.Initial ();
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
		
		if (Time.timeScale == 0)
			return;

		//move
		if(isMoveable)
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


	#region AnimationEvent
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

	public void Project(int skillNumber){
		if(isLocalPlayer)
			CmdProject ();
	}

	[Command]
	public void CmdProject(){
		var projection = Instantiate(rightHandWeapon.projection,
			transform.position,transform.rotation) as GameObject;
		SkillProjection skillProjection = projection.GetComponent<SkillProjection> ();
		skillProjection.selfStatus = this.selfStatus;
		skillProjection.attackedList.Add (this.GetComponent<Combat> ());
		skillProjection.damage = rightHandWeapon.damage;
		NetworkServer.Spawn (projection);
	}

//	[ClientRpc]
//	public void RpcProject(){
//		
//	}

	public void PlayHurtEffect(GameObject input){
//		rightHandWeapon.effect = GetComponentInChildren<SkillManager> ().skillList [skillNumber].effect;
//		rightHandWeapon.damage = GetComponentInChildren<SkillManager> ().skillList [skillNumber].GetDamage ();
//		GetComponent<Combat>().hurtEffect = input;
	}

//	[ClientRpc]
//	public void RpcProject(){
////		Weapon.Project (transform.position,Quaternion.Euler(transform.forward));
//	}

	[ServerCallback]
	public void SetInvincibleTrue(){
		CmdInvincle (true);
//		Debug.Log ("無敵");
	}

	[ServerCallback]
	public void SetInvincibleFalse(){
		CmdInvincle (false);
//		Debug.Log ("沒無敵");
	}

	[Command]
	void CmdInvincle(bool item){
		selfStatus.isInvincible = item;
	}

	[ServerCallback]
	public void Cancle(){
		RpcCancle ();
	}

	[ClientCallback]
	public void SetMoveableFalse(){
		isMoveable = false;
	}

	[ClientCallback]
	public void SetMoveableTrue(){
		isMoveable = true;
	}

	[ClientRpc]
	public void RpcCancle(){
//		Debug.Log ("Cancle");
		AttackColliderOff ();
	}
		
	[ServerCallback]
	public void SuperArmor(){
		CmdSuperArmor ();
	}

	[Command]
	void CmdSuperArmor(){
		selfStatus.EP += 300f;
	}

	[ServerCallback]
	public void CancleSuperArmor(){
		CmdCancleSuperArmor ();
	}

	[Command]
	public void CmdCancleSuperArmor(){
		selfStatus.EP = selfStatus.EP > 100f ? 100f : selfStatus.EP;
	}


	public void Initial(){
		CancleSuperArmor ();
		SetInvincibleFalse ();
		SetMoveableTrue ();
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
		
		CmdSetAnimFloat ("Horizontal",horizontal);

		CmdSetAnimFloat ("Vertical",vertical==0f&&horizontal==0f? 1f:vertical);

		CmdSetAnimBool ("Dodge",true);
	}
	#endregion

	#region AnimationParameterSeendingNetwork

	public bool IsSkillable(){
		return meatBallAnimator.GetBool ("Skillable");
	}

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
	public void CmdSetAnimInt(string boolName,int setInt)
	{
		RpcSetAnimInt(boolName,setInt);
	}

	[ClientRpc]
	public void RpcSetAnimInt(string boolName,int setFloat)
	{
		meatBallAnimator.SetInteger(boolName,setFloat);
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

	[Command]
	public void CmdSetSkillLayer()
	{
		RpcSetSkillLayer();
	}

	[ClientRpc]
	public void RpcSetSkillLayer()
	{
		meatBallAnimator.SetLayerWeight (3,1f);
	}
		
	public bool IsHurt(){
		return meatBallAnimator.GetBool ("Hurt");
	}
	#endregion 

}
