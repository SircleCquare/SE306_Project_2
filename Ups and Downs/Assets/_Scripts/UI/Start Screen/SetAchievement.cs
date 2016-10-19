using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Script for setting the text of an achievement shown on the main menu
/// </summary>
public class SetAchievement : MonoBehaviour
{
    public Text NameText;
    public Text DescriptionText;

    /// <summary>
    /// Set the details of an achievement row in the main achievements menu
    /// </summary>
    /// <param name="achievementName"></param>
    /// <param name="achievementDescription"></param>
    /// <param name="locked">True if the player has not unlocked the achievement</param>
    public void SetDetails(string achievementName, string achievementDescription, bool locked)
    {

        // Gray out the text if the achievement is locked. 
        if (locked)
        {
            NameText.color = Color.gray;
            DescriptionText.color = Color.gray;
        }

        // Set the text
        NameText.text = achievementName;
        DescriptionText.text = achievementDescription;

       
    }
}
