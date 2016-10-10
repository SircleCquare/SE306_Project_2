using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

	public static string NAME_TUTORIAL = "Michael's tutorial level";
	public static string NAME_LEVEL_01 = "level";

	public void loadTutorial()
    {
		SceneManager.LoadScene(LevelSelect.NAME_TUTORIAL);
    }

	public void loadLevel01()
	{
		SceneManager.LoadScene (LevelSelect.NAME_LEVEL_01);
	}

}
