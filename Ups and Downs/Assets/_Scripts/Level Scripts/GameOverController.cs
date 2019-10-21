using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// The controller for the finish scene.
/// </summary>
public class GameOverController : MonoBehaviour
{
	private bool isGameOver;

	public const string FINISH_SCENE_NAME = "Finish Scene";
	public const string GAME_OVER_SCENE_NAME = "Game Over Scene";

	public Text levelNameText;
    public Text timeText;
	public Text deathsText;
	public Text scoreText;

    public List<GameObject> scoreObjects;
    public GameObject highScoreEntryGroup;
    public InputField highScoreNameInput;

    private bool savedHighScoreName = false;

    // Use this for initialization
    void Start ()
	{
		Debug.Log ("initializing finish screen");

		Debug.Log ("setting screen name: game over screen");
		levelNameText.text = "Game Over";
	}
	
	// Update is called once per frame
	void Update () {

        DisplayHighScores(ApplicationModel.levelName);

        if (!GameData.GetInstance().IsHighScore(ApplicationModel.levelName, ApplicationModel.score) || savedHighScoreName)
        {
            highScoreEntryGroup.SetActive(false);
        }
    }

    /*
     * Show high scores in the list of high scores
     */
    void DisplayHighScores(string levelName)
    {
        var gameData = GameData.GetInstance();
        int i = 0;

        foreach (var highScore in gameData.GetOrderedHighScoresForLevel(levelName))
        {
            if (i >= scoreObjects.Count)
            {
                throw new System.IndexOutOfRangeException("More high scores than available score object entries");
            }

            var currentScoreObj = scoreObjects[i]; i++;
            Text[] fields = currentScoreObj.GetComponentsInChildren<Text>();

            Text nameField = fields[0],
                scoreField = fields[1];

            nameField.text = highScore.playerName;
            scoreField.text = highScore.pointsValue.ToString("#,##0");
        }
    }
}
