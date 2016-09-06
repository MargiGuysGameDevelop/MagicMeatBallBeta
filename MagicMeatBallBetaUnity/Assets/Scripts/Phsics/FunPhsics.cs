using UnityEngine;
using System.Collections;

public class FunPhsics : MonoBehaviour {

	//Unity Component
	Rigidbody rigid;
	Transform trans;
	RaycastHit hit;

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

	void Awake(){
		rigid = GetComponent<Rigidbody> ();
		trans = GetComponent<Transform> ();
	}

	void Update(){
		if (!pause) {
			if (Input.GetKey (KeyCode.E))
				Force = new Vector3(1f,3f,1f);
			ifOnGround ();
			if (force != Vector3.zero || !isGround) {
				//飛
				FlyY ();
				FlyXZ ();
			} else if (!justGround && isGround) {
				//剛著地
				justGround = true;
				force = Vector3.zero;
				wholeTimes = 0f;
				initialYVelocity = 0f;
			}
				//平常狀態
		}
	}

	void ifOnGround(){
		var fall = !Physics.Raycast (trans.position,Vector3.down,out hit); 
		#if UNITY_EDITOR
		Debug.DrawLine(trans.position,hit.point,Color.red);
		#endif
		if (!fall) {
			distanceWithGround = Vector3.Distance (hit.point, trans.position);
			if (distanceWithGround <= 0.15f) {
				isGround = true;
			} else {
				vectorWithGround = distanceWithGround * Vector3.down;
				isGround = false;
			}
		} else {
			isGround = false;
			hit.point = trans.position + 0.25f * Vector3.down;
		}
	}

	#region 飛行中
	void FlyY(){

		justGround = false;

		if (Mathf.Abs (trans.position.y - force.y) < 0.3f) {
			if (force.y != hit.point.y) {
				force.y = hit.point.y;
			}else{
				force = Vector3.zero;
			}
		} 

		var YTrans = new Vector3 (trans.position.x,
//			initialYVelocity - 0.5f*Physics.gravity.y*Time.deltaTime
			force.y
			,trans.position.z);

//		trans.Translate (YTrans * Time.deltaTime);
		trans.position = Vector3.Lerp(trans.position,YTrans,damping);
	}

	void FlyXZ(){
		var XZTrans = new Vector3 (force.x,0f,force.z);
		transform.Translate (XZTrans*Time.deltaTime);
	}
	#endregion

	#region 施力
	public Vector3  force = Vector3.zero;
	float wholeTimes = 0f;
	float damping = 0.1f;
	float initialYVelocity = 0f;

	public Vector3 Force{
		set{ 
			force = trans.position + value;
			wholeTimes = Mathf.Sqrt (2*value.y/Physics.gravity.y);
			initialYVelocity = Mathf.Sqrt (2*Physics.gravity.y*value.y);
		}
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
