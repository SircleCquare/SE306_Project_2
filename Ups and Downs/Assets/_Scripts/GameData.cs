using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{

    public int LevelNumber { get; set; }

    public Side currentSide { get; set; }

    /** The number of Coins the player has found in this play through */
    public int coinsFound { get; set; }

    public int totalNumberOfCoins { get; set; }

    public float time { get; set; }

    public int heart { get; set; }

    private SpecialItem itemType = SpecialItem.None;
    private int itemIndex = -1;

    public int MAX_HEALTH = 5;

    public List<string> awardedAchievements; 

    public GameData()
    {
        // Initialise level state
        clearLevelState(); 

        // List to store names of achievements
        awardedAchievements = new List<String>();  
    }

    /*
     * Clears all game data related to only the current level (i.e. for when a level is reset or completed)
     */
    public void clearLevelState()
    {
        coinsFound = 0;
        heart = MAX_HEALTH;
        time = 0.0f;
    }


    public SpecialItem getItemType()
    {
        return itemType;
    }

    public int getItemIndex()
    {
        return itemIndex;
    }

    public void setInventoryItem(SpecialCollectible specialItem)
    {
        if (specialItem != null)
        {
            itemType = specialItem.itemType;
            itemIndex = specialItem.index;
        } else
        {
            itemType = SpecialItem.None;
            itemIndex = -1;
        }
        
    }

}
