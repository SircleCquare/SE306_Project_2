using UnityEngine;
using System.Collections;

public class AchievementsDisplay : MonoBehaviour
{
    public GameObject ScrollPaneContent;
    public GameObject AchievementPrefab;



    public void OnEnable()
    {
        Debug.Log("Called");
        var gameData = GameData.LoadInstance();

        var awardedAchievements = gameData.awardedAchievements;
        var allAchievements = Achievements.achievementList;

        foreach (var achievement in allAchievements.Keys)
        {
            var achievementObject = Instantiate(AchievementPrefab);
            var achievementScript = achievementObject.GetComponent<SetAchievement>();


            if (awardedAchievements.Contains(achievement))
            {
                achievementScript.SetDetails(achievement, allAchievements[achievement], true);
            }
            else
            {
                achievementScript.SetDetails(achievement, "Locked: Keep playing to unlock", false);
            }

            achievementObject.transform.SetParent(ScrollPaneContent.transform);
            var achivevementTransform = achievementObject.GetComponent<RectTransform>();
            achivevementTransform.localScale = new Vector3(1f, 1f, 1f);
            achivevementTransform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            achivevementTransform.localPosition = new Vector3(achivevementTransform.position.x, 0f, 0f);

        }




    }


}
