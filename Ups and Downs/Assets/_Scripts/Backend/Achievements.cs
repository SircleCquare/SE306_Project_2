using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to store the list of achievements in the game
/// </summary>
public class Achievements {

    public static Dictionary<string, string> achievementList = new Dictionary<string, string>()
    {
        {"Tutorial", "Completed the Tutorial"},
        {"Level 1", "Completed Level One"},
		{"I'm rich", "Collected your first coin"},
        {"100 coins", "Collected 100 coins total"}
    };

    /*
    * Record an achievement as unlocked and display it, if this is the first time it was unlocked
    */
    public static void UnlockAchievement(string achievementName, GameObject achievementPopUp, Text achievementText, GameData gameData)
    {
        // Only pop achievement description when achievement first awarded
        if (!gameData.awardedAchievements.Contains(achievementName))
        {
            // Record achievement
            gameData.awardedAchievements.Add(achievementName);

            // Display achievement pop up 
            achievementText.text = Achievements.achievementList[achievementName];
            achievementPopUp.SetActive(true);

            Debug.Log("Unlocked achievement: " + achievementName);

            gameData.Save();
        }

    }

    /*
     * Hide the achievement pop up
     */
    public static void HideAchivementPopup(GameObject achievementPopUp)
    {
        achievementPopUp.SetActive(false);
    }
}
