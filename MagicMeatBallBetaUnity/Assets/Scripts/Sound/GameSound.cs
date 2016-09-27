using UnityEngine;
using System.Collections;

public class GameSound : MonoBehaviour {

	//this script carries all the GameScene Sound
	public static GameSound instance = null;

	public AudioClip[] LobbyBGMClips;
	public AudioClip[] GameBGMClips;

	public AudioClip buttonEffectClip;

	public AudioClip hurtSound;
	public AudioClip walkSound;
	public AudioClip deadSound;
	public AudioClip rollingSound;

	public AudioClip swordWeaponSound;

	public AudioClip fireBallSound;
	public AudioClip AriaSound;

	void Awake ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

	}

	void Update () {
		
	}

	public void LoadAllClip(){
		instance = this;
		LobbyBGMClips = Resources.LoadAll<AudioClip> ("Sound/BGM/Lobby");
		GameBGMClips = Resources.LoadAll<AudioClip> ("Sound/BGM/Game");

		buttonEffectClip = Resources.Load<AudioClip> ("Sound/Effect/UI/Button1");

		hurtSound = Resources.Load<AudioClip> ("Sound/Effect/Fighting/Hurt");
		walkSound = Resources.Load<AudioClip> ("");
		deadSound = Resources.Load<AudioClip> ("");
		rollingSound = Resources.Load<AudioClip> ("");

		swordWeaponSound = Resources.Load<AudioClip> ("Sound/Effect/Fighting/Sword1");

		fireBallSound = Resources.Load<AudioClip> ("");
		AriaSound = Resources.Load<AudioClip> ("");

	}

}
