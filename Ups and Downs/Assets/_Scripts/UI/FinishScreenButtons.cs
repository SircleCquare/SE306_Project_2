using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FinishScreenButtons : MonoBehaviour {

	public void NextLevelButton(int index)
	{
	    SceneManager.LoadScene(index);
	}

	public void NextLevelButton(string levelName)
	{
	    SceneManager.LoadScene(levelName);
	}

	public void NextLevelButtonPredictive()
	{
		int levelNumber = ApplicationModel.levelNumber;
	    SceneManager.LoadScene(levelNumber + 3);
	}
}
