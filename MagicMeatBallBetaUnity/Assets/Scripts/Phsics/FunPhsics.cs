using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FunPhsics : NetworkBehaviour {

	//Unity Component
	Rigidbody rigid;
	Transform trans;
	RaycastHit hit;
	Vector3 hitPoint;
	LayerMask layer;

	#region 飛行(fly)

	#region 垂直參數
	bool isGround = true;
	float distanceWithGround = 0f;
	Vector3 vectorWithGround = Vector3.zero;

	public bool IsGround{
		get{ return isGround; }
	}

	public Vector3 FallDistance{
		get{return Physics.gravity / Application.targetFrameRate;}
	}

	float lastYPosition = 0f;
	float Yvelocity;
	#endregion

	#region 水平參數
	Vector3 currentDirection;

	float speed = 2f;

	public Vector3 CurrentDirection{
		set{ currentDirection = value.normalized * speed; }
	}
	#endregion

	#region 優化措施
	public bool pause = false;
	bool justGround = false;
	#endregion
	#endregion

	#region 等速(push)
	float pushTimes = 0f;
	Vector3 pushVector = Vector3.zero;
	#endregion

	void InitialParameter(){
		justGround = true;
		wholeTimes = 0f;
		initialYVelocity = 0f;
		force = Vector3.zero;
	}

	void Awake(){
		rigid = GetComponent<Rigidbody> ();
		trans = GetComponent<Transform> ();
	}

	void Start(){
		layer = 1 << 2;
		layer = ~layer;
	}

	void FixedUpdate(){

		if (Time.timeScale == 0f)
			return;

		rigid.velocity = new Vector3( rigid.velocity.x, 0f, rigid.velocity.z) ;

		if (!pause) {
			OnGround ();
			if (force != Vector3.zero || !isGround) {
				//飛
				FlyY ();
				FlyXZ ();
				Yvelocity = (trans.position.y - lastYPosition) * 10f; 
				if (lastYPosition != trans.position.y) 
					lastYPosition = trans.position.y;
			} else if (!justGround && isGround) {
				//剛著地
				InitialParameter();
			}else{
				//平常狀態
				//受力推擊(Push)
				if (pushTimes > 0f) {
//					var localPushVector = trans.InverseTransformDirection(pushVector);
					trans.Translate (pushVector * Time.deltaTime,Space.World);

				} else if (pushVector != Vector3.zero) {
					pushTimes = 0f;
					pushVector = Vector3.zero;
				}
					
			}
			if (pushTimes > 0f) {
				pushTimes -= Time.deltaTime;
			} else if(pushTimes <0f){
				pushTimes = 0f;
			}
		}
	}

	void Update(){

	}

	void OnGround(){
//		Debug.Log ("0");
		var fall = !Physics.Raycast (trans.position,Vector3.down,out hit,10f,
//			LayerMask.NameToLayer("Ignore Raycast")
			layer
			,QueryTriggerInteraction.Ignore); 


		if (!fall) {
			distanceWithGround = Vector3.Distance (hit.point, trans.position);
			if (distanceWithGround <= 0.15f) {
				isGround = true;
			} else {
				#if UNITY_EDITOR
				Debug.DrawLine(trans.position,hit.point,Color.blue);
				#endif
				vectorWithGround = distanceWithGround * Vector3.down;
				isGround = false;
			}
		} else {
			#if UNITY_EDITOR
			Debug.DrawLine(trans.position,hit.point,Color.red);
			#endif
			isGround = false;
		}
		hitPoint = hit.point;
	}

	#region 飛行中
	float timer = 0f;
	void FlyY(){
		timer += Time.deltaTime;
		justGround = false;

		float y	= hitPoint.y;;

		if (!isGround )
			y = (initialYVelocity + timer * Physics.gravity.y);
		else if (timer > wholeTimes / 2) {
			InitialParameter ();
			return;
		} else if(timer < wholeTimes / 2){
			y = (initialYVelocity + timer * Physics.gravity.y);
		}

		y = Mathf.Clamp (y, -10f, 10f);

		trans.Translate (0f,y * Time.deltaTime,0f);
	}

	void FlyXZ(){
		if(force.x != 0f || force.z != 0f){
			var XZTrans = new Vector3 (force.x,0f,force.z);
			transform.Translate (XZTrans*Time.deltaTime,relativeTo:Space.World);
		}
	}
	#endregion

	#region 施力
	public Vector3  force = Vector3.zero;
	float wholeTimes = 0f;
	float initialYVelocity = 0f;
	 
	[Command]
	public void  CmdAddForce(float xSpeed, float yHight, float zSpeed){
		RpcAddForce (xSpeed,yHight,zSpeed);
	}

	[ClientRpc]
	public void  RpcAddForce(float xSpeed, float yHight, float zSpeed){
		force += new Vector3( xSpeed, yHight, zSpeed);
		wholeTimes = Mathf.Sqrt (Mathf.Abs(8f*force.y/Physics.gravity.y));
		initialYVelocity = -Physics.gravity.y* wholeTimes/2f;
		timer = 0f;

	}

	[ClientRpc]
	/// <summary>
	/// Push，平面的等速位移。(Y==0!)
	/// </summary>
	/// <param name="times">Times.</param>
	/// <param name="input">Input.</param>
	/// <param name="scale">Scale.</param>
	public void RpcPushEqualVelocity(float times,Vector3 input) {
		pushVector += input;
		pushVector.y = 0f;
		pushTimes += times;
	}

	[Command]
	/// <summary>
	/// Push，平面的內插位移。(Y==0!)
	/// </summary>
	/// <param name="times">Times.</param>
	/// <param name="input">Input.</param>
	/// <param name="scale">Scale.</param>
	public void CmdPushEqualVelocity(float times,Vector3 input) {
		RpcPushEqualVelocity (times, input);
	}
	#endregion

	#region 公開
	public bool GetGroundBool(){
		return isGround;
	}

	public float GetYVelocity(){
		return Yvelocity;
	}
	#endregion

}
