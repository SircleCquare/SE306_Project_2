using System.Collections.Generic; 

/// <summary>
/// Used to store the list of achievements in the game
/// </summary>
public class Achievements {

    public static Dictionary<string, string> achievementList = new Dictionary<string, string>()
    {
        {"Level 1", "Completed Level One"},
        {"100 coins", "Collected 100 coins total"}
    }; 
}
