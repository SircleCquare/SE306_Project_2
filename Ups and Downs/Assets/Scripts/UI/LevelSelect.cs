using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

	public void loadTutorial()
    {
        SceneManager.LoadScene("level");
    }

}
