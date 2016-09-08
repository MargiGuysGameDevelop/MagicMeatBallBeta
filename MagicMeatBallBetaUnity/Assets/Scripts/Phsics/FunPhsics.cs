﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FunPhsics : NetworkBehaviour {

	//Unity Component
	Rigidbody rigid;
	Transform trans;
	RaycastHit hit;
	Vector3 hitPoint;

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

	void FixedUpdate(){

		rigid.velocity = new Vector3( rigid.velocity.x, 0f, rigid.velocity.z) ;

		if (!pause) {
			if (Input.GetKeyDown (KeyCode.E))
				RpcAddForce (1f, 2f, 1f);
			OnGround ();
			if (force != Vector3.zero || !isGround) {
				//飛
				FlyY ();
				FlyXZ ();
			} else if (!justGround && isGround) {
				//剛著地
				InitialParameter();
			}

				//平常狀態
		}
	}

	void OnGround(){
		var fall = !Physics.Raycast (trans.position,Vector3.down,out hit); 


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
			transform.Translate (XZTrans*Time.deltaTime);
		}
	}
	#endregion

	#region 施力
	public Vector3  force = Vector3.zero;
	float wholeTimes = 0f;
	float initialYVelocity = 0f;
	 
	[ClientRpc]
	public void  RpcAddForce(float xSpeed, float yHight, float zSpeed){
		force += new Vector3( xSpeed, yHight, zSpeed);
		wholeTimes = Mathf.Sqrt (Mathf.Abs(8f*force.y/Physics.gravity.y));
		initialYVelocity = -Physics.gravity.y* wholeTimes/2f;
		timer = 0f;

	}
	#endregion

	#region 公開
	public bool GetGroundBool(){
		return isGround;
	}

	public float GetZVelocity(){
		return rigid.velocity.z;
	}
	#endregion

}