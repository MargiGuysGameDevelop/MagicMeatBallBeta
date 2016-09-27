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


		GameSound.instance.LoadAllClip ();

		IniAudioSource ();
		BGMVolume = 0.38f;
		effectVolume = 0.5f;

	}

	void IniAudioSource(){
		OnLevelWasLoaded (0);
	}

	public void PlayButtonEffect(){
		instance.buttonEffectSound.clip = GameSound.instance.buttonEffectClip;
		buttonEffectSound.volume = effectVolume;
		instance.buttonEffectSound.Play ();
	}

	public void RanddomBGMPlay(params AudioClip[] clips){
		int randomIndex = Random.Range(0, clips.Length);
		BGM.clip = clips[randomIndex];
		BGM.Play();
	}

	public void PlaySound(AudioSource source, AudioClip clip){
		source.clip = clip;
		source.volume = effectVolume;
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
