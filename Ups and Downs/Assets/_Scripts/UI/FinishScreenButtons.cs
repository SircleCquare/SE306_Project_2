﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FinishScreenButtons : MonoBehaviour {

	public void NextLevelButton(int index)
	{
	    SceneManager.LoadScene(index);
	}

	public void NextLevelButton()
	{
		int levelNum = GameData.GetInstance ().LevelNumber;
		if (levelNum < SceneManager.sceneCountInBuildSettings) {
			SceneManager.LoadScene ("Level " + ++levelNum);
		} else {
			SceneManager.LoadScene ("Start");
		}
	}

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
