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

	public void RetryLevel()
	{
		int levelNumber = ApplicationModel.levelNumber;
		SceneManager.LoadScene (levelNumber + 2);	//someone get rid of these magic numbers ;_;
	}

	//Don't use this. The functionality can be achieved using LevelSelect#loadLevel("Start")
	public void QuitToMainMenu()
	{
		SceneManager.LoadScene (0);
	}
}
