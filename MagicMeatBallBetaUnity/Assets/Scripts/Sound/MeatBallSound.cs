using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeatBallSound : MonoBehaviour {

	private List<AudioSource> audioSourceList;


	void Awake(){
		audioSourceList = new List<AudioSource> (GetComponents<AudioSource> ());
	}


	public void PlaySwordAttackSound(){
		SoundManager.instance.PlaySound (GetIdleAudioSource(),GameSound.instance.swordWeaponSound);
	}

	public void PlayHurtSound(){
		SoundManager.instance.PlaySound (GetIdleAudioSource(),GameSound.instance.hurtSound);
	}





	private AudioSource GetIdleAudioSource(){
		if (audioSourceList.Count == 0) {
			return CreatNewAudioSource ();
		} else {
			foreach (AudioSource audioSource in audioSourceList) {
				if (!audioSource.isPlaying)
					return audioSource;
			}
			return CreatNewAudioSource ();
		}
	}

	private AudioSource CreatNewAudioSource(){
		AudioSource newAudioSource = gameObject.AddComponent <AudioSource>() as AudioSource;
		newAudioSource.mute = false;
		newAudioSource.loop = false;
		audioSourceList.Add (newAudioSource);
		return newAudioSource;
	}

}
