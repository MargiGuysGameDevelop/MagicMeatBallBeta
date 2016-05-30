using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MeatBallMove : NetworkBehaviour {

	Animator meatBallAnimator;
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
		
		float vertical = Input.GetAxis("Vertical"); //sw
		float horizontal = Input.GetAxis("Horizontal"); //ad

		if( vertical < 0)
			vertical *= -1f;
		if(horizontal < 0)
			horizontal *= -1f;

		Direct(vertical,horizontal);
		Move(vertical,horizontal);



	}

	void Move( float vertical, float horizontal){
		if(vertical + horizontal > 0.1f){
			gameObject.transform.Translate( Vector3.forward* meatBallSpeed* Time.deltaTime);
		}
	}

	void Direct( float vertical, float horizontal){




		meatBallAnimator.SetFloat("Move",vertical + horizontal);
		//rotate immediately
		if(Input.GetKey("w") && Input.GetKey("a")){
			rotateAngel = 315;
			gameObject.transform.eulerAngles = new Vector3( 0, playerView.transform.eulerAngles.y + rotateAngel, 0);
		}
		else if(Input.GetKey("w")  && Input.GetKey("d")){
			rotateAngel = 45f;
			gameObject.transform.eulerAngles = new Vector3( 0, playerView.transform.eulerAngles.y + rotateAngel, 0);
		}
		else if(Input.GetKey("s") && Input.GetKey("a")){
			rotateAngel = 225f;
			gameObject.transform.eulerAngles = new Vector3( 0, playerView.transform.eulerAngles.y + rotateAngel, 0);
		}
		else if(Input.GetKey("s") && Input.GetKey("d")){
			rotateAngel = 135f;
			gameObject.transform.eulerAngles = new Vector3( 0, playerView.transform.eulerAngles.y + rotateAngel, 0);
		}
		else if(Input.GetKey("a")){
			rotateAngel = 270f;
			gameObject.transform.eulerAngles = new Vector3( 0, playerView.transform.eulerAngles.y + rotateAngel, 0);
		}
		else if(Input.GetKey("d")){
			rotateAngel = 90f;
			gameObject.transform.eulerAngles = new Vector3( 0, playerView.transform.eulerAngles.y + rotateAngel, 0);
		}
		else if(Input.GetKey("w")){
			rotateAngel = 0f;
			gameObject.transform.eulerAngles = new Vector3( 0, playerView.transform.eulerAngles.y + rotateAngel, 0);
		}
		else if(Input.GetKey("s")){
			rotateAngel = 180f;
			gameObject.transform.eulerAngles = new Vector3( 0, playerView.transform.eulerAngles.y + rotateAngel, 0);
		}
	}

}
