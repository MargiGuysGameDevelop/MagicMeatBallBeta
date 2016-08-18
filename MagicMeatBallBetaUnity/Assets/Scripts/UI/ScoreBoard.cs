using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class ScoreBoard : MonoBehaviour {

	[SerializeField]
	GameObject DataUI;

	void Awake(){
		DataUI.SetActive (false);
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Tab)){
			DataUI.SetActive (true);
		}else if(Input.GetKeyUp(KeyCode.Tab)){
			DataUI.SetActive (false);
		}
	}

	#region DataLayout
	[SerializeField]
	GameObject property;
	[SerializeField]
	GameObject[] playerData  = new GameObject[9];
	[SerializeField]
	GameObject dataContainer;

	public string[] propertyName = {"玩家名稱","分數","殺人數","死亡數"};


	[ContextMenu("RefreshDataProperty")]
	public void RefreshDataPrefab(){
		float width = 1f /( (float) propertyName.Length );
		var proRect = property.GetComponentsInChildren<RectTransform> ();
		int horizontalNumber = 0;
		int nameNumber = 0;
		for(int i=1;i<proRect.Length;i++){
			if (i % 2 != 0) {
				RectExtend(proRect[i],new Vector2(width*horizontalNumber,0f),new Vector2(width*(horizontalNumber+1),1f));
				horizontalNumber++;
			} else {
				RectExtend (proRect[i],new Vector2(0f,0f),new Vector2(1f,1f));

				var text = proRect [i].gameObject.GetComponent<Text> ();

				if (text) {
					text.text = propertyName [nameNumber];
					nameNumber++;
				}
					
			}
		}
	}


	[ContextMenu("RefreshLayout")]
	public void RefreshLayout(){

		float height = 1f/((float)playerData.Length + 1f) ;

		RectExtend(property.GetComponent<RectTransform>(),new Vector2(0f,1f-height),new Vector2(1f,1f));

		for(int i=0;i<playerData.Length;i++){
			int j = i+2;
			int k= i+1;
			RectExtend(playerData[i].GetComponent<RectTransform>(),
				new Vector2(0f,1f-height*j),new Vector2(1f,1f-height*k));
		}
	}

	void RectExtend(RectTransform rect,Vector2 minCroodinate,Vector2 maxCroodinate){
		rect.anchorMax = maxCroodinate;
		rect.anchorMin = minCroodinate;

		rect.offsetMin = rect.offsetMax = Vector2.zero;
	}

	#endregion
}
