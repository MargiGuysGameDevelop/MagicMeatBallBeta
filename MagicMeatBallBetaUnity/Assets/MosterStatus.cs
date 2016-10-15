using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MosterStatus : NetworkBehaviour{

	FunPhsics phsics;
	Moster self;

	[SyncVar]
	public float HP;
	[SyncVar]
	public float EP;

	public float MaxHP;
	public float MaxEP;

	bool dead = false;

	//States
	public bool isInvincible = false;


	void Awake(){
		self = GetComponent<Moster> ();
		phsics = GetComponent<FunPhsics> ();
	}

	[ServerCallback]
	void Update () {

		if (Time.timeScale == 0)
			return;

		if (dead)
			return;

		CaculateAnimPra ();
	}

	[ServerCallback]
	public void CaculateAnimPra(){
		bool isGround = Physics.Raycast (transform.position,Vector3.down,0.2f);
		self.CmdSetAnimFloat ("YVelocity",phsics.GetYVelocity());
		self.CmdSetAnimBool ("OnGround",isGround);
	}

	public void Damage(float hp,float fatigue,Vector3 direction,int netID){
		HP -= hp;
		EP -= fatigue;
		if (HP <= 0f) {
			HP = 0f;
			self.Dead ();
			return;
		}
		if (EP <= 0f) {
			EP = 0f;
			self.Hurt ();
			if(direction != Vector3.zero)
				phsics.CmdAddForce (direction.x,direction.y,direction.z);
		}
	}
}
