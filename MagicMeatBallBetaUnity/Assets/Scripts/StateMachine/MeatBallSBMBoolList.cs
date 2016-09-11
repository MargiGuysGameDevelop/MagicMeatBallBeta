using UnityEngine; 
using System.Collections; 

public class MeatBallSBMBoolList : StateMachineBehaviour {

	[Header("選取要修改的項目")]
	[SerializeField]
	protected bool Attack = false;  
	[SerializeField]
	protected bool Jump= false; 
	[SerializeField]
	protected bool Rolling= false; 
	[SerializeField]
	protected bool Dodge= false; 
	[SerializeField]
	protected bool Dead= false; 
	[SerializeField]
	protected bool Hurt= false; 
	[SerializeField]
	protected bool OnGround= false; 
	[SerializeField]
	protected bool HitFly= false; 
//	[SerializeField]
//	[Header("無用")]
//	protected bool Skillable = false;

	[Header("上面選取的項目與下方會相同")]
	[SerializeField]
	protected bool isOn = false;

	public void SetAllBool(Animator anim){
		if (Attack)
			anim.SetBool("Attack",isOn);
		if (Jump)
			anim.SetBool("Jump",isOn);
		if (Rolling)
			anim.SetBool("Rolling",isOn);
		if (Dodge)
			anim.SetBool("Dodge",isOn);
		if (Dead)
			anim.SetBool("Dead",isOn);
		if (Hurt)
			anim.SetBool("Hurt",isOn);
		if (OnGround)
			anim.SetBool("OnGround",isOn);
		if (HitFly)
			anim.SetBool("HitFly",isOn);
	}
		
	[ContextMenu("攻擊預設")]
	public void SetAttack(){
		Attack = true;
		Jump = Rolling = Dodge = true;
		Dead = Hurt = OnGround = HitFly  = false;
		isOn = false;
	}

	[ContextMenu("跳躍滾動預設")]
	public void SetJumpAndRoll(){
		AllFalse ();
		Jump = true;
		Dodge = true;
		Rolling = true;
//		Skillable = true;
		isOn = false;
	}

	[ContextMenu("死亡被攻擊擊飛預設")]
	public void SetHurt(){
		AllTrue ();
		isOn = false;
	}
		

	[ContextMenu("全部取消")]
	public void SetNoGround(){
		AllTrue ();
		isOn = false;
	}

	public void AllFalse(){
		Attack = 
		Jump = Rolling = Dodge =
				Dead = Hurt = OnGround = HitFly  = false;
	}

	public void AllTrue(){
		Attack = 
			Jump = Rolling = Dodge =
				Dead = Hurt = OnGround = HitFly = true;
	}
}
