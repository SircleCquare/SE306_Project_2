using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {

	private static DontDestroy singleton;

	void Awake()
	{
		if (singleton)
		{
			Destroy(singleton.gameObject);
		}
		singleton = this;
		//Causes UI object not to be destroyed when loading a new scene. If you want it to be destroyed, destroy it manually via script.
		DontDestroyOnLoad(gameObject);
	}

}
