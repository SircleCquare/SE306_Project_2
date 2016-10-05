using System; 

[Serializable]
public class GameData {

    public int LevelNumber;
    public Side currentSide; 

    /** The number of Coins the player has found in this play through */
    public int coinsFound;
    public int totalNumberOfCoins;

    public float time { get; set; } 

    public int health;
    public const int MAX_HEALTH = 100;

    public GameData()
    {
        coinsFound = 0;
        health = MAX_HEALTH;
        time = 0.0f; 
    }
	
	
}
