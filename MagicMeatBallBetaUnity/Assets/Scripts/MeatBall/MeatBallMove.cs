using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MeatBallMove : NetworkBehaviour {
	/*
	private enum MeatBallState{ Movement,Jump };

	[SerializeField]
	MeatBallState meatBallState;
*/
	AnimatorStateInfo currentState; 
	Animator meatBallAnimator;

	public float vertical;
	public float horizontal;

	public float rotateAngel;
	public float meatBallSpeed;
	public GameObject playerView;
	// Use this for initialization


	void Start () {
		meatBallAnimator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {
		//ensure player control himself
		if(!isLocalPlayer)
			return;

		currentState = meatBallAnimator.GetCurrentAnimatorStateInfo (0);


		vertical = Input.GetAxis("Vertical"); //sw
		horizontal = Input.GetAxis("Horizontal"); //ad

		float mouseMoveX = Input.GetAxis("Mouse X");
		transform.Rotate(Vector3.up , mouseMoveX * 75f * Time.deltaTime);

		meatBallAnimator.SetFloat("Vertical",vertical);
		meatBallAnimator.SetFloat("Horizontal",horizontal);

		if (Input.GetKeyDown("space")) {
			Jump();
		}


	}





	public void Jump(){
		if (CanJump()) {
			CmdSetAnimTrigger ("Jump");
		}
	}
	private bool CanJump(){
		//check state
		if (currentState.nameHash == Animator.StringToHash ("Base Layer.Movement"))
			return true;
		else
			return false;
	}


	[Command]
	public void CmdSetAnimTrigger(string triggerName)
	{
		if(!isServer)
		{
			meatBallAnimator.SetTrigger(triggerName);
		}
		RpcSetAnimTrigger(triggerName);
	}

	[ClientRpc]
	public void RpcSetAnimTrigger(string triggerName)
	{
		meatBallAnimator.SetTrigger(triggerName);
	}


}
