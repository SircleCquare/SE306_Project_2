using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// The controller for the finish scene.
/// </summary>
public class FinishController : MonoBehaviour
{
    /// <summary>
    /// The name of the level just completed.
    /// </summary>
    public Text levelNameText;

    /// <summary>
    /// The time taken to complete the level, in seconds.
    /// </summary>
    public Text timeText;

    /// <summary>
    /// The number of times the player died during the level.
    /// </summary>
    public Text deathsText;

    /// <summary>
    /// The player's score.
    /// </summary>
    public Text scoreText;

    public Text timeMultiplier;
    public Text deathMultiplier;

    public List<GameObject> scoreObjects;
    public GameObject highScoreEntryGroup;
    public InputField highScoreNameInput;
    public GameObject nextLevelButton;

    private bool savedHighScoreName = false;

    // Name of the last level, to hide "next level" button
    private const string lastLevelName = "Level 2";

    // Use this for initialization
    void Start()
    {
        // Show raw information
        levelNameText.text = "Completed " + ApplicationModel.levelName + "!";
        deathsText.text = ApplicationModel.deathCount.ToString();
        timeText.text = ApplicationModel.time.ToString("0.0") + " seconds";

        // Show scores and multipliers to explain how score is used calculated
        timeMultiplier.text = "x" + (ApplicationModel.timeMultiplier).ToString("0.00");
        deathMultiplier.text = "x" + (ApplicationModel.deathMultiplier).ToString("0.00");
        scoreText.text = ApplicationModel.score.ToString("#,##0");
    }

    // Update is called once per frame
    void Update()
    {
        // Hide next level button if final level
        if (ApplicationModel.levelName == lastLevelName)
        {
            nextLevelButton.SetActive(false);
        }

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

    public void SaveHighScore()
    {
        var gameData = GameData.GetInstance();
        gameData.AddHighScore(ApplicationModel.levelName, highScoreNameInput.text, ApplicationModel.score);
        gameData.Save(); // persist new high score

        savedHighScoreName = true; // hide high score entry form
    }
}
