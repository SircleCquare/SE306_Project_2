using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that stores and manages achievements in the game.
/// </summary>
public class Achievements {

	/*
	* The Dictionary of all achievements in the game. Each Achievement has a name and a description
	* which explains the criteria for earning it.
	*/
    public static Dictionary<string, string> achievementList = new Dictionary<string, string>()
    {
        {"Tutorial", "Completed the Tutorial"},
        {"Level 1", "Completed Level One"},
        {"Level 2", "Completed Level Two"},
        {"10,000 Club", "Get a score of over 10,000 on any level" },
        {"50,000 Club", "Get a score of over 50,000 on any level" },
        {"75,000 Club", "Get a score of over 50,000 on any level" },
        {"100,000 Club", "Get a score of over 100,000 on any level" }
    };

    /*
    * Record an achievement as unlocked and display it, if this is the first time it was unlocked
    */
    public static void UnlockAchievement(string achievementName, GameObject achievementPopUp, Text achievementText, GameData gameData, AudioClip achievementSound)
    {
        // If this achievement has not already been awarde, then display it and update the game state.
        if (!gameData.awardedAchievements.Contains(achievementName))
        {
            // Record achievement
            gameData.awardedAchievements.Add(achievementName);

            // Display achievement pop up
            achievementText.text = Achievements.achievementList[achievementName];
            achievementPopUp.SetActive(true);

            // Get the camera's position in space for supporting directional sound
            Vector3 cameraPos = Camera.main.transform.position;
            // Round to whole number due to spurious issue with directional sound.
            cameraPos.z = Mathf.Round(cameraPos.z);

            // Play achievement sound at approximately the camera's location 
            AudioSource.PlayClipAtPoint(achievementSound, cameraPos, 1f);

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
