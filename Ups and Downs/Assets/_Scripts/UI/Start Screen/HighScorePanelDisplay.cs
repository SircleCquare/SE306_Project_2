using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Script for showing high scores in High Score panel. 
/// </summary>
public class HighScorePanelDisplay : MonoBehaviour
{
    /// <summary>
    /// Drop down for choosing level to view. 
    /// </summary>
    public Dropdown LevelDropdown;

    /// <summary>
    /// Prefab to base generated high score rows on
    /// </summary>
    public GameObject HighScorePrefab;

    /// <summary>
    /// Scroll pane holding all the high scores
    /// </summary>
    public GameObject ScrollPaneContent;

    /// <summary>
    /// Add all levels that have associated high scores to the dropdown of highscores
    /// and show the highscores for the current level. 
    /// </summary>
    void OnEnable()
    {
        var gameData = GameData.LoadInstance();

        // Get rid of all current options
        LevelDropdown.options.Clear();

        if (gameData.GetHighScores().Keys.Count != 0)
        {
            // Add options for each level that has high scores
            foreach (var level in gameData.GetHighScores().Keys)
            {
                LevelDropdown.options.Add(new Dropdown.OptionData(level));
            }
        }
        else
        {
            LevelDropdown.options.Add(new Dropdown.OptionData("No high scores"));
        }

        // This is to reset the text shown in the dropdown box
        LevelDropdown.value = 1;
        LevelDropdown.value = 0; 

        ShowHighScores();
    }

    /// <summary>
    /// Display the high scores for the currently selected level
    /// </summary>
    public void ShowHighScores()
    {
        // Get level name
        var levelName = LevelDropdown.options[LevelDropdown.value].text;

        // Get game data holding high scores
        var gameData = GameData.LoadInstance();

        // Clear what is currently shown in panel, except the header row (child 0) 
        var oldScoreCount = ScrollPaneContent.transform.childCount;
        for (var i = oldScoreCount - 1; i > 0; i--)
        {
            var child = ScrollPaneContent.transform.GetChild(i);
            Destroy(child.gameObject);
        }

        var orderedHighScoreList = gameData.GetOrderedHighScoresForLevel(levelName);
        if (orderedHighScoreList.Count == 0)
        {
            // Show a message if no high scores to show
            CreateHighScoreRow(new HighScoreValue(levelName, 0, "No high scores to show"), false);
        }
        else
        {        
            // Show each high score
            foreach (var highScore in gameData.GetOrderedHighScoresForLevel(levelName))
            {
                CreateHighScoreRow(highScore);
            }
        }
       
    }

    /// <summary>
    /// Create an individual high score row
    /// </summary>
    /// <param name="highScore">the HighScoreValue representing the highscore row</param>
    /// <param name="showPoints">true if the high score's points value should be shown, false otherwise.
    /// Used when the row is to be used for infomative text rather than a full high score (i.e. when 
    /// no high scorees are available to show.</param>
    private void CreateHighScoreRow(HighScoreValue highScore, bool showPoints = true)
    {
        // Create object to show high score
        var highScoreObject = Instantiate(HighScorePrefab);
        var highScoreScript = highScoreObject.GetComponent<SetHighScoreRow>();

        // Set score details. Only show points if needed. 
        if (showPoints)
        {
            highScoreScript.SetDetails(highScore.playerName, highScore.pointsValue);
        }
        else
        {
            highScoreScript.SetNameOnly(highScore.playerName);
        }

        // Add to the scroll pane of panel
        highScoreObject.transform.SetParent(ScrollPaneContent.transform);

        /* Set the display settings of each row so that it is not transformed at an 
         * angle to the parent because of the screen rotation
         */
        var highScoreTransform = highScoreObject.GetComponent<RectTransform>();
        highScoreTransform.localScale = new Vector3(1f, 1f, 1f);
        highScoreTransform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        highScoreTransform.localPosition = new Vector3(highScoreTransform.position.x, 0f, 0f);
    }

}
