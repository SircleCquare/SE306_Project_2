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

    public int health { get; set; }

    public int MAX_HEALTH = 100;

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
        health = MAX_HEALTH;
        time = 0.0f;
    }
}
