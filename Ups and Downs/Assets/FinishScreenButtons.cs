using UnityEngine;
using System.Collections;

public class FinishScreenButtons : MonoBehaviour {

	public void NextLevelButton(int index)
	{
		Application.LoadLevel(index);
	}

	public void NextLevelButton(string levelName)
	{
		Application.LoadLevel(levelName);
	}

	public void NextLevelButtonPredictive()
	{
		int levelNumber = ApplicationModel.levelNumber;
		Application.LoadLevel (levelNumber + 1);
	}
}
