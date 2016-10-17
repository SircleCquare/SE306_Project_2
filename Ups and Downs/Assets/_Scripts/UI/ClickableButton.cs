using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ClickableButton : MonoBehaviour {
	
	public int levelNumber = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnClick() {
		SceneManager.LoadScene("Level " + levelNumber);
	}
}
