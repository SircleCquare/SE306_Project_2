using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EventSystemChecker : MonoBehaviour
{
	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLoaded;
	}

	//OnLevelWasLoaded is called after a new scene has finished loading
	void OnLoaded(Scene scene, LoadSceneMode mode)
	{
		//If there is no EventSystem (needed for UI interactivity) present
		if(!FindObjectOfType<EventSystem>())
		{
			//The following code instantiates a new object called EventSystem
			GameObject obj = new GameObject("EventSystem");

			//And adds the required components
			obj.AddComponent<EventSystem>();
			obj.AddComponent<StandaloneInputModule>().forceModuleActive = true;
		}
	}
}
