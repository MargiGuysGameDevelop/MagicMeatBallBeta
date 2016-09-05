using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class ScoreBoard : MonoBehaviour {

	[SerializeField]
	GameObject DataUI;
	public PlayerScore[] playerData;

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

	#region DataInput
	public void ClearData(int index){
		var ps = playerData [index];
		for (int i = 0; i < ps.dataProperty.Length; i++)
			ps.dataProperty [i].text = string.Empty;
	}
	public void ChangeValueData(int index,int item){
		playerData[index].dataProperty[item].text =
			(int.Parse(playerData [index].dataProperty[item].text) +1)
				.ToString();
	}

	public void ChangeName(int index,string name){
		print (index);
		playerData [index].dataProperty [(int)ScoreKind.name].text = name;
		for(int i=1;i<playerData[index].dataProperty.Length;i++){
			playerData [index].dataProperty [i].text = 0.ToString();
		}
	}

	public void InitialData(){
		foreach(PlayerScore ps in playerData){
			ps.GetText ();
		}
	}
	#endregion

	#region DataLayout
	[SerializeField]
	GameObject property;
	[SerializeField]
	GameObject[] playerDataUI  = new GameObject[9];
	[SerializeField]
	GameObject dataContainer;

	public string[] propertyName = {"玩家名稱","分數","殺人數","死亡數"};


	[ContextMenu("RefreshDataProperty")]
	public void RefreshDataPrefab(){
		playerData = GetComponentsInChildren<PlayerScore> ();
		for(int i = 0;i<playerData.Length;i++){
			playerDataUI [i] = playerData [i].gameObject; 
		}
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
		RefreshLayout ();
	}

	public void RefreshLayout(){

		float height = 1f/((float)playerDataUI.Length + 1f) ;

		RectExtend(property.GetComponent<RectTransform>(),new Vector2(0f,1f-height),new Vector2(1f,1f));

		for(int i=0;i<playerDataUI.Length;i++){
			int j = i+2;
			int k= i+1;
			RectExtend(playerDataUI[i].GetComponent<RectTransform>(),
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
