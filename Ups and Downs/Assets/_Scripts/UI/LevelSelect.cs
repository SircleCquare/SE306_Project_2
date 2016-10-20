using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {
    // Load given level
	public void loadLevel(string name) {
		SceneManager.LoadScene (name);
	}

}
