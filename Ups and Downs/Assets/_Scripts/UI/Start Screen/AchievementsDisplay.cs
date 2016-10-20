using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Logic to display achievements in panel on start screen. 
/// </summary>
public class AchievementsDisplay : MonoBehaviour
{
    /// <summary>
    /// Scroll pane holding all the achievements
    /// </summary>
    public GameObject ScrollPaneContent;

    /// <summary>
    /// A prefab to base achievement game objects on for display
    /// </summary>
    public GameObject AchievementPrefab;


    /// <summary>
    /// Method to compute unlocked achievements when achivement panel opened. 
    /// </summary>
    void OnEnable()
    {
        // Find all achievements and awarded achievements. 
        var gameData = GameData.LoadInstance();
        var awardedAchievements = gameData.awardedAchievements;
        var allAchievements = Achievements.achievementList;

        // Clear what is currently shown in panel
        var oldAchievementCount = ScrollPaneContent.transform.childCount;
        for (var i = oldAchievementCount - 1 ; i >= 0 ; i--)
        {
            var child = ScrollPaneContent.transform.GetChild(i);
            Destroy(child.gameObject);
        }

        // Queue to hold locked achievements so they can be disaplyed below unlocked ones
        var lockedAchievementQueue = new Queue<GameObject>();

        // Show unlocked achievements
        foreach (var achievement in allAchievements.Keys)
        {
            // Create object to show achievenment
            var achievementObject = Instantiate(AchievementPrefab);
            var achievementScript = achievementObject.GetComponent<SetAchievement>();

            if (awardedAchievements.Contains(achievement))
            {
                // Show an unlocked achievement
                achievementScript.SetDetails(achievement, allAchievements[achievement], false);
                DisplayAchievement(achievementObject);
            }
            else
            {
                // Show an achivement that's still locked
                achievementScript.SetDetails("<LOCKED> " + achievement, "Locked: Keep playing to unlock", true);
                lockedAchievementQueue.Enqueue(achievementObject);
            }
        }

        // Show locked achievements below unlocked ones (for better logical/visual grouping for user) 
        while (lockedAchievementQueue.Count > 0)
        {
            DisplayAchievement(lockedAchievementQueue.Dequeue());
        }

    }

    /// <summary>
    /// Add an acheivement to be displayed in the scroll pane
    /// </summary>
    /// <param name="achievementObject"></param>
    private void DisplayAchievement(GameObject achievementObject)
    {
        // Add to the scroll pane of achievements
        achievementObject.transform.SetParent(ScrollPaneContent.transform);

        /* Set the display settings of each achievement so that it is not transformed at an 
         * angle to the parent because of the screen rotation
         */
        var achivevementTransform = achievementObject.GetComponent<RectTransform>();
        achivevementTransform.localScale = new Vector3(1f, 1f, 1f);
        achivevementTransform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        achivevementTransform.localPosition = new Vector3(achivevementTransform.position.x, 0f, 0f);
    }


}
