using UnityEngine;
using System.Collections;

public class Stage0_GM : GameManager {

	public int currentStage = 0;

	#region Stage0Parameters
	/// <summary>
	/// BossNumber
	/// </summary>
	/// <value>A.</value>
	public int A{
		get{
			int number;
			if (currentStage < 11)
				number = 1;
			else if (currentStage < 31)
				number = 2;
			else
				number = currentStage % 10;
			return number;
		}
	}
	/// <summary>
	/// mosterNumber
	/// </summary>
	/// <value>The b.</value>
	public int B{
		get{
			int number; 
			if (currentStage < 11)
				number = 20;
			else if(currentStage < 21)
				number =  30;
			else if(currentStage <31)
				number = 40;
			else 
				number = currentStage +15; 
			return number;
		}
	}
	/// <summary>
	/// fallingTimes
	/// </summary>
	/// <value>The c.</value>
	public int C{
		get{
			int number; 
			if (currentStage < 6)
				number = 5;
			else if(currentStage < 21)
				number =  currentStage%5+1;
			else 
				number = currentStage%5 +2; 
			return number;
		}
	}
	/// <summary>
	/// NoHurt%
	/// </summary>
	/// <value>The d.</value>
	public int D {
		get {
			int number; 
			if (currentStage < 6)
				number = 25;
			else if (currentStage < 21)
				number = 20;
			else
				number = 15; 
			return number;
		}
	}
	/// <summary>
	/// Damage
	/// </summary>
	/// <value>The e.</value>
	public int E{
		get{
			return currentStage / 5 + 1;
		}
	}
	#endregion
}
