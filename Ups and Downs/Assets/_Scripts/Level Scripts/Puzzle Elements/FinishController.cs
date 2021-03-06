﻿using UnityEngine;
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
    /// The number of coins collected, out of the total.
    /// </summary>
    public Text coinsText;

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
    public Text coinScore;

    /// <summary>
    /// The achievement popup.
    /// </summary>
    public GameObject achievementPopUp;

    /// <summary>
    /// Sound played when achievement pops
    /// </summary>
    public AudioClip achievementAudioClip;

    /// <summary>
    /// The text to display for the achivement.
    /// </summary>
    public Text achievementText;

    public List<GameObject> scoreObjects;
    public GameObject highScoreEntryGroup;
    public InputField highScoreNameInput;
    public GameObject nextLevelButton;

    // Time remaining to display the achievement popup.
    private float achievementPopUpCountdown = 2.0f;

    private bool achievementDisplayed = false;
    private bool savedHighScoreName = false;

    // Name of the last level, to hide "next level" button
    private const string lastLevelName = "Level 2";

    private Queue<string> achievementQueue = new Queue<string>(); 

    // Use this for initialization
    void Start()
    {
        // Show raw information
        levelNameText.text = "Completed " + ApplicationModel.levelName + "!";
        coinsText.text = ApplicationModel.coinsFound.ToString();
        deathsText.text = ApplicationModel.deathCount.ToString();
        timeText.text = ApplicationModel.time.ToString("0.0") + " seconds";

        var gameData = GameData.GetInstance();

        // Show scores and multipliers to explain how score is used calculated
        coinScore.text = (gameData.CoinScore * 100).ToString("#,##0");
        timeMultiplier.text = "x" + (ApplicationModel.timeMultiplier).ToString("0.00");
        deathMultiplier.text = "x" + (ApplicationModel.deathMultiplier).ToString("0.00");
        scoreText.text = ApplicationModel.score.ToString("#,##0");

        // Unlock level completed achievement
        UnlockAchievement(ApplicationModel.levelName);

        // Unlock score based achievements
        var score = ApplicationModel.score;
        if (score >= 10000)
        {
            UnlockAchievement("10,000 Club");
        }
        if (score >= 50000)
        {
            UnlockAchievement("50,000 Club");
        }
        if (score >= 75000)
        {
            UnlockAchievement("75,000 Club");
        }
        if (score >= 100000)
        {
           UnlockAchievement("100,000 Club"); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Hide next level button if final level
        if (ApplicationModel.levelName == lastLevelName)
        {
            nextLevelButton.SetActive(false);
        }

        if (achievementDisplayed)
        {
            // Handle hiding an achievement if it is visible
            TryHideAchivementPopup();
        }else if (achievementQueue.Count > 0)
        {
            // Unlock any queued up achievements
            UnlockAchievement(achievementQueue.Dequeue());
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
        // If an achievement is currently being shown, queue this one to be shown later
        if (achievementDisplayed)
        {
            achievementQueue.Enqueue(achievementName);
            return;
        }

        // Show an achievement
        Achievements.UnlockAchievement(achievementName, achievementPopUp, achievementText, GameController.Singleton.GetGameData(), achievementAudioClip);
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
