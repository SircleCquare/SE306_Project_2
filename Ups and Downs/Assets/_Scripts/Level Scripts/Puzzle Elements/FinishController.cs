using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// The controller for the finish scene.
/// </summary>
public class FinishController : MonoBehaviour
{
	public Text levelNameText;
	public Text coinsText;
    public Text timeText;
	public Text deathsText;
	public Text scoreText;

    public GameObject achievementPopUp;
    public Text achievementText;

    public List<GameObject> scoreObjects;
    public GameObject highScoreEntryGroup;
    public InputField highScoreNameInput;

    // Countdown for when to hide achievement popup
    private float achievementPopUpCountdown = 2.0f;

    private bool achievementDisplayed = false;
    private bool savedHighScoreName = false;

    // Use this for initialization
    void Start ()
	{
		levelNameText.text = "Completed " + ApplicationModel.levelName + "!";
		coinsText.text = ApplicationModel.coinsFound + "/" + ApplicationModel.totalCoins;
	    deathsText.text = ApplicationModel.deathCount.ToString();
	    scoreText.text = ApplicationModel.score.ToString();
	    timeText.text = ApplicationModel.time.ToString("0.0") + " seconds";
        UnlockAchievement(ApplicationModel.levelName);
	}
	
	// Update is called once per frame
	void Update () {
        // Handle hiding an achievement if it is visible
        if (achievementDisplayed)
        {
            TryHideAchivementPopup();
        }

        DisplayHighScores(ApplicationModel.levelName);

        if (!GameData.GetInstance().IsHighScore(ApplicationModel.levelName, ApplicationModel.score) || savedHighScoreName)
        {
            highScoreEntryGroup.SetActive(false);
        }
    }

    /*
     * Record an achievement as unlocked and display it, if this is the first time it was unlocked
     */
    void UnlockAchievement(string achievementName)
    {
        Achievements.UnlockAchievement(achievementName, achievementPopUp, achievementText, GameController.Singleton.GetGameData());
        achievementPopUpCountdown = 2.0f;
        achievementDisplayed = true; 
    }

    /*
     * Hides the achievement popup when the achievement countdown ends. 
     */
    void TryHideAchivementPopup()
    {
        achievementPopUpCountdown -= Time.deltaTime;

        // Don't hide popup if countdown time still remains
        if (achievementPopUpCountdown > 0)
        {
            return;
        }

        Achievements.HideAchivementPopup(achievementPopUp);
        achievementDisplayed = false; 
    }

    /*
     * Show high scores in the list of high scores
     * TODO (low priority) - move to a better approach than hard coded score fields (table?) if time allows
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

            nameField.text = highScore.PlayerName;
            scoreField.text = highScore.Score.ToString("#,##0");
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
