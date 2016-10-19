using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Set the details of row showing high score. 
/// </summary>
public class SetHighScoreRow : MonoBehaviour {

    public Text PlayerName;
    public Text PointsValue;

    /// <summary>
    /// Set the details of a high score row in the main high score menu
    /// </summary>
    /// <param name="playerName"></param>
    /// <param name="playerScore"></param>
    public void SetDetails(string playerName, int pointsValue)
    {
        // Set the text
        PlayerName.text = playerName;
        PointsValue.text = pointsValue.ToString("#,##0");
    }

    /// <summary>
    /// Only set the player name, so points are not shown. Used when the row 
    /// will contain informative text rather than a high score. 
    /// </summary>
    /// <param name="playerName"></param>
    public void SetNameOnly(string playerName)
    {
        PlayerName.text = playerName;
    }

}
