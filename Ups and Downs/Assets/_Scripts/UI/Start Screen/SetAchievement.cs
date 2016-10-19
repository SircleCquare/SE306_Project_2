using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetAchievement : MonoBehaviour
{
    public Text NameText;
    public Text DescriptionText;

    public void SetDetails(string achievementName, string achievementDescription, bool locked)
    {
        NameText.text = achievementName;
        DescriptionText.text = achievementDescription; 
    }
}
