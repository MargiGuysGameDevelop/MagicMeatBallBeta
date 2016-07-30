using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

using Prototype.NetworkLobby;
public class MeatBallStatus : NetworkBehaviour {

	Text NameText;
	Text HPText ;
	Slider HPSlider;

	[SyncVar]
	public string playerName;
	[SyncVar]
	public Color nameColor;
	[SyncVar]
	public float HP;
	/*[SyncVar]
	public float MP;*/

	//public float damage;

	public float MaxHP;
	//public float MaxMP;

	public int currentWeapon = 0; //default =0

	public LobbyPlayer[] lobbyPlayers;

	// Use this for initialization
	void Awake () {
		MaxHP = 100f;
		HP = MaxHP;
		//MP = MaxMP;


		TextInit ();
	}

	// Update is called once per frame
	void Update () {
		SetHpValue ();	
	}


	void TextInit(){
		Text[] texts;
		texts = GetComponentsInChildren<Text> ();
		foreach(Text te in texts){
			if (te.name == "HpText")
				HPText = te;
			else if (te.name == "NameText")
				NameText = te;
		}
		HPText.text = HP.ToString();
		HPSlider = GetComponentInChildren<Slider> ();
		HPSlider.value = HP;
		NameText.text = playerName;
		//NameText.color = nameColor;

	}

	public override void OnStartLocalPlayer (){
		
		NameText.text = playerName;
	}
	public void SetHpValue(){
		NameText.text = playerName;
		HPSlider.value = HP;
		HPText.text = HP.ToString ();
		//Debug.Log ("set HP : " + selfStatus.HP);
	}

}
