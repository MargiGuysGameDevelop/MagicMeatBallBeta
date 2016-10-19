using UnityEngine;
using System.Collections;

[System.Serializable]
public class ColdDown{
	
	[SerializeField]
	[Range(0f,100f)]
	protected float _value = 0f;
	public float currentValue = 0f;
	public float value{
		get{ return _value;}
		set { value = Mathf.Clamp (value, 0f, 9999f);}
	}
	public bool isDone {
		get{ return currentValue <= 0f ? true : false;}
	}
	/// <summary>
	/// 開始跑CD
	/// </summary>
	public void Count(){
		currentValue = _value;
	}
	public void Timer(){
		if (currentValue > 0f) {
			currentValue -= Time.deltaTime;
		}
	}
	bool isEnterCount = true;
	public void Timer(float newValue){
		newValue = Mathf.Clamp (newValue,0,9999f);
		if (!isEnterCount) {
			currentValue -= Time.deltaTime;
			if (currentValue <= 0f) {
				isEnterCount = true;
			}
		} else {
			currentValue = newValue;
			isEnterCount = false;
		}
	}

}
