using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillUIManager : MonoBehaviour {

	public Image[] iconList = new Image[4];

	Skill[] skills;
	public Animator[] skillAnimator = new Animator[5];

	public Image icon_figure;

	public void InitialSkills(Skill[] value){
		skills = value;
	}

	[ContextMenu("更新Animator")]
	void Start(){
		for(int i=0;i<iconList.Length;i++){
			skillAnimator [i] =iconList[i].GetComponentInParent<Animator> ();
		}
	}

	void Update(){
		if (Time.timeScale == 0)
			return;

		for(int i=0;i<4;i++){
			if (!skills [i+1].CD.isDone && iconList[i].fillAmount == 1f) {
//				iconList[i].fillAmount = 1f- skills[i+1].CD.currentValue/skills[i+1].CD.value;
				skillAnimator [i].speed =1/ skills [i + 1].CD.value;
				skillAnimator [i].Play("SkillCD");
			}
		}
	}
}
