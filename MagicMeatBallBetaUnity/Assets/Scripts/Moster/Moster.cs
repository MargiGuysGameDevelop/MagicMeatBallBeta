using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public enum MosterType{
	Moster,
	Boss
};

public enum MarginalType{
	Pilow,
	Spray
}


public class Moster :  NetworkBehaviour{
	public MosterType type = MosterType.Moster;
	public float attackRange;

	Combat player;

	NavMeshAgent nav;
	Animator anim;
	Transform trans;
	public MosterStatus selfStatus;

	public delegate void DeathDelegate();
	public DeathDelegate deadFunc;
	Combat selfComat;

	//stage
	public int myIndexOnGM = 0;

	void Awake(){
		//self property
		nav = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		trans = transform;
		selfStatus = GetComponent<MosterStatus> ();
		selfComat = GetComponent<Combat> ();
	}

	public void NeedToUpdate(){
		if (Time.timeScale == 0f)
			return;

		if (nav.destination == trans.position || player==null)
			return;
		
		if (Vector3.Distance (nav.destination, trans.position) <= attackRange) {
			anim.SetBool ("Attack",true);
		}
	}

	public void SetKindOfMoster(int kind){
		anim.SetLayerWeight (kind,1f);
	}

	public void Dead(){
		if (deadFunc != null)
			deadFunc ();
		selfStatus.isInvincible = true;
		CmdSetAnimBool ("Dead",true);
		if(deadFunc != null)
			deadFunc ();
		selfComat.enabled = false;
	}

	public void Hurt(){
		CmdSetAnimBool ("Hurt",true);
	}
		
	public bool IsInvincble(){
		return selfStatus.isInvincible;
	}

	#region BaseAnim
	[Command]
	public void CmdInitAnim(){
		RpcInitAnim ();
	}

	[ClientRpc]
	public void RpcInitAnim(){
		//		Debug.Log ("重生");
		anim.Play ("Movement");
	}

	[Command]
	public void CmdSetAnimFloat(string boolName,float setFloat)
	{
		RpcSetAnimFloat(boolName,setFloat);
	}

	[ClientRpc]
	public void RpcSetAnimFloat(string boolName,float setFloat)
	{
		anim.SetFloat(boolName,setFloat);
	}

	[Command]
	public void CmdSetAnimInt(string boolName,int setInt)
	{
		RpcSetAnimInt(boolName,setInt);
	}

	[ClientRpc]
	public void RpcSetAnimInt(string boolName,int setInt)
	{
		anim.SetInteger(boolName,setInt);
	}



	[Command]
	public void CmdSetAnimBool(string boolName,bool boolState)
	{
		RpcSetAnimBool(boolName,boolState);
	}

	[ClientRpc]
	public void RpcSetAnimBool(string boolName,bool boolState)
	{
		anim.SetBool(boolName,boolState);
	}

	[Command]
	public void CmdSetAnimTrigger(string triggerName)
	{
		RpcSetAnimTrigger(triggerName);
	}

	[ClientRpc]
	public void RpcSetAnimTrigger(string triggerName)
	{
		anim.SetTrigger(triggerName);
	}
	#endregion


}
