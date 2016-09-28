using UnityEngine;
using System.Collections;




public class SoundManager : MonoBehaviour {

	public AudioSource BGM;
	public AudioSource buttonEffectSound;
	public static SoundManager instance = null;

	//public AudioClip[] LobbyBGMClips;
	//public AudioClip[] GameBGMClips;

	//public AudioClip buttonEffectClip;

	public float BGMVolume;
	public float effectVolume;

	void Awake ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		
		DontDestroyOnLoad (gameObject);

		instance.BGMVolume = 0.28f;
		instance.effectVolume = 0.45f;

	}

	void Start(){
		GameSound.instance.LoadAllClip ();

		IniAudioSource ();
	}

	void IniAudioSource(){
		OnLevelWasLoaded (0);
	}

	public void PlayButtonEffect(){
		buttonEffectSound.clip = GameSound.instance.buttonEffectClip;
		buttonEffectSound.volume = SoundManager.instance.effectVolume;
		instance.buttonEffectSound.Play ();
	}

	public void RanddomBGMPlay(params AudioClip[] clips){
		int randomIndex = Random.Range(0, clips.Length);
		Debug.Log (randomIndex);
		BGM.clip = clips[randomIndex];
		BGM.volume = SoundManager.instance.BGMVolume;
		BGM.Play();
	}

	public void PlaySound(AudioSource source, AudioClip clip){
		source.clip = clip;
		source.volume = SoundManager.instance.effectVolume;
		source.Play ();
	}

	void OnLevelWasLoaded(int level){
		if (level == 0) {
			RanddomBGMPlay (GameSound.instance.LobbyBGMClips);
		} 
		if (level == 1) {
			RanddomBGMPlay (GameSound.instance.GameBGMClips);
		}
	}
}
