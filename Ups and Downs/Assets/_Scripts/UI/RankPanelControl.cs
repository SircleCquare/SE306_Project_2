using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{

    public Text boardLabel;

    public List<GameObject> scoreObjects;

    // Use this for initialization
    void Start()
    {
        DisplayHighScores(ApplicationModel.levelName);
    }

    /*
     * Show high scores in the list of high scores
     * TODO (low priority) - move to a better approach than hard coded score fields (table?) if time allows
     */
    void DisplayHighScores(string levelName)
    {

        boardLabel.text = levelName + " High Score";

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
