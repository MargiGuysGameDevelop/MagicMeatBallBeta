using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogManager : MonoBehaviour {

	[SerializeField]
	static private Text context;

	[ContextMenu("指定訊息框")]
	void GetContext(){
		context = GetComponentInChildren<Text> ();
	}

	void Start(){
		if(!context)
			GetContext ();
		context.text+="Wellcome!";
		Log ("Battle Begin!");
	}

	static public void Log(string message){
		context.text += "\r\n"+ message;
	}

	static public void LogError(string message){
		Log (message);
	}

	static public void Clear(){
		context.text = "";
	}
}
