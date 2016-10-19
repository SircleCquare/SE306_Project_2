﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Script for setting the text of an achievement
/// </summary>
public class SetAchievement : MonoBehaviour
{
    public Text NameText;
    public Text DescriptionText;

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
