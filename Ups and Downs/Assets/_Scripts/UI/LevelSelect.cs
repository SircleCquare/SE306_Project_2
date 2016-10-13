using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {
	public void loadLevel(string name) {
		SceneManager.LoadScene (name);
	}

}
