using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndBoard : MonoBehaviour {
	public Text winnerIndex;
	public RectTransform endBoardBG;

	private float targetWidth = 600;
	private float targetHeight = 400;

	private float width;
	private float height;

	public static string winnerName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (endBoardBG.localScale.x < 1) {
			endBoardBG.localScale += new Vector3 (0.02f, 0f, 0f);
		}
		if (endBoardBG.localScale.y < 1) {
			endBoardBG.localScale += new Vector3(0,0.02f,0f);
			winnerIndex.text = winnerName;
		}
	}

	void OnEnable(){
		endBoardBG.localScale = Vector3.zero;
	}

	public static void SetWinnerName(string wName){
		winnerName = wName;
	}
}
