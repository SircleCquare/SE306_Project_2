using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FinishScreenButtons : MonoBehaviour {

    // Loads the given levels scene
	public void NextLevelButton(int index)
	{
	    SceneManager.LoadScene(index);
	}

    // Loads the next levels scene
	public void NextLevelButton()
	{
		int levelNum = GameData.GetInstance ().LevelNumber;
		if (levelNum < SceneManager.sceneCountInBuildSettings) {
			SceneManager.LoadScene ("Level " + ++levelNum);
		} else {
			SceneManager.LoadScene ("Start");
		}
	}

    // Reloads the current scene
	public void RetryLevel()
	{
		int levelNum = GameData.GetInstance ().LevelNumber;
		if (levelNum <= 0) {
			SceneManager.LoadScene ("Tutorial");
		} else {
			SceneManager.LoadScene ("Level " + levelNum);
		}
	}

	//Don't use this. The functionality can be achieved using LevelSelect#loadLevel("Start")
	public void QuitToMainMenu()
	{
		SceneManager.LoadScene (0);
	}
}
