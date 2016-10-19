using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
/// <summary>
/// GameData is the main class for storing and retrieving the state of the game.
/// It can be serialized to file and retrieved later to store the game state between sessions.
/// Data stored includes achievements unlocked, high scores, the current level number, number of coins found,
/// number of deaths and the current level's play time.
/// </summary>
public class GameData
{

    [NonSerialized] private static GameData instance;

    [NonSerialized] private static string saveFile;

    /// <summary>
    /// A list of achievements the player has unlocked
    /// </summary>
    public List<string> awardedAchievements { get; set; }

    /// <summary>
    /// A dictionary of highscores for levels.
    ///
    /// The key is the string name of the level.
    ///
    /// The value is a sorted dictionary of the highscores.
    /// For this dictionary, the key is the score and the value is the player's name.
    /// This dictionary will have at most 5 highscores in it. Only the highest five scores are kept
    /// (as specified by MAX_HIGH_SCORES)
    ///
    /// This cannot be serialized, so it is converted to a set of arrays for storage at save time.
    /// </summary>
    [NonSerialized] Dictionary<string, List<HighScoreValue>> HighScores;

    /// <summary>
    /// An unsorted list for storing HighScores so they can be serialized.
    /// </summary>
    private List<HighScoreValue> highScoreList;

    /// <summary>
    /// Maximum number of high scores stored per level
    /// </summary>
    private const int MAX_HIGH_SCORES = 5;

    /// <summary>
    /// The highest level unlocked by the player across sessions.
    /// </summary>
    public int HighestLevelUnlockedNumber { get; set; }

    /// <summary>
    /// The total number of coins in the currently active level.
    /// </summary>
    public int TotalNumberOfCoins { get; set; }

    /// <summary>
    /// The number of the currently playing level, as specified in the build settings order.
    /// </summary>
    public int LevelNumber { get; set; }

    /// <summary>
    /// The number of Coins the player has found in this play through
    /// </summary>
    public int CoinsFound { get; set; }

    /// <summary>
    /// The score as contributed by coins (each coin can have a custom point setting)
    /// </summary>
    public int CoinScore { get; set; }

    /// <summary>
    /// The number of times the player has died in this play through
    /// </summary>
    public int Deaths { get; set; }

    /// <summary>
    /// The current level time (in milliseconds)
    /// </summary>
    public float Time { get; set; }

    /// <summary>
    /// The number of hearts (lives) the player has remaining in this level.
    /// </summary>
    public int Heart { get; set; }

    /// <summary>
    /// The currently held special inventory item (unused)
    /// </summary>
    private SpecialItem itemType = SpecialItem.None;
    private int itemIndex = -1;

    /// <summary>
    /// The maximum number of lives.
    /// </summary>
    public const int MAX_HEALTH = 5;

    private GameData()
    {
        // Initialise level state
        clearLevelState();

        // List to store names of achievements
        awardedAchievements = new List<String>();
        HighScores = new Dictionary<string, List<HighScoreValue>>(5);
    }

    /// <summary>
    /// Obtain an instance of the game data.
    /// </summary>
    /// <returns></returns>
    public static GameData GetInstance()
    {
        return instance ?? (instance = new GameData());
    }

    /*
     * Clears all game data related to only the current level (i.e. for when a level is reset or completed)
     */
    public void clearLevelState()
    {
        CoinsFound = 0;
        CoinScore = 0;
        Heart = MAX_HEALTH;
        Time = 0.0f;
        Deaths = 0;
    }

    /*
    * Accessor methods for the inventory item (currently unused)
    */
    #region Inventory
    public SpecialItem getItemType()
    {
        return itemType;
    }

    public int getItemIndex()
    {
        return itemIndex;
    }

    /// <summary>
    /// Set the inventory to hold the specialItem.
    /// </summary>
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

    #endregion


    #region HighScores

    /// <summary>
    /// Store the dictionary of high scores as a list for serialization.
    /// </summary>
    private void HighScoresToList()
    {
        highScoreList = new List<HighScoreValue>();

        // Add the scores for each level to a list
        foreach (var level in HighScores.Keys)
        {
            highScoreList.AddRange(HighScores[level]);
        }
    }

    /// <summary>
    /// Converts highscores from list to dictionary for quick retreival
    /// </summary>
    private void HighScoresFromList()
    {
        // Add each score to the dictionary
        foreach (var score in highScoreList)
        {
            // Create a list to hold scores for a level if none exists
            if (!HighScores.ContainsKey(score.LevelName))
            {
                HighScores.Add(score.LevelName, new List<HighScoreValue>());
            }

            HighScores[score.LevelName].Add(score);
        }
    }

    /// <summary>
    /// Test if the current score is in the top five high scores.
    /// </summary>
    /// <returns>True if this is score is higher than the lowest score on the leaderboard,
    ///          or there is space to add highscores to the leaderboard</returns>
    public bool IsHighScore(string levelName, int score)
    {
        var highScores = GetOrderedHighScoresForLevel(levelName);

        return highScores.Count < MAX_HIGH_SCORES || score >= highScores[highScores.Count - 1].PointsValue;
    }

    /// <summary>
    /// Add a high score for a level.
    ///
    /// Will not add the score if the player didn't actually get a highscore.
    /// </summary>
    /// <param name="levelName">The name of the level</param>
    /// <param name="playerName">The name of the player the score is for</param>
    /// <param name="score">The score the player achieved</param>
    public void AddHighScore(string levelName, string playerName, int score)
    {
        if (IsHighScore(levelName, score))
        {
            var highScores = GetOrderedHighScoresForLevel(levelName);

            // Remove lowest high score if needed
            if (highScores.Count >= 5)
            {
                highScores.RemoveRange(4, highScores.Count - 4);
            }

            // Add highscore
            highScores.Add(new HighScoreValue(levelName, score, playerName));

            highScores.Sort();
        }
    }

    /// <summary>
    /// Get an ordered dictionary of high scores for a level.
    /// </summary>
    /// <param name="levelName"></param>
    /// <returns></returns>
    public List<HighScoreValue> GetOrderedHighScoresForLevel(string levelName)
    {
        // Create a dictionary to store high scores if none exists
        if (!HighScores.ContainsKey(levelName))
        {
            HighScores[levelName] = new List<HighScoreValue>(5);
        }

        HighScores[levelName].Sort();

        return HighScores[levelName];
    }

    #endregion

    /*
     * The persistence region handles serializing and deserializing the game data from disk so that
     * game state can persist between sessions.
     */
    #region Persistence
    /// <summary>
    /// Load the game data from player prefs
    /// </summary>
    /// <returns>The loaded game data instance</returns>
    public static GameData LoadInstance()
    {
        if (!PlayerPrefs.HasKey("data"))
        {
            // If no save exists, create new game data and save
            Debug.Log("No save found");
            CreateFreshSave();
        }
        else
        {
            // Get serialized data as binary array to deserialize
            var binaryData = Convert.FromBase64String(PlayerPrefs.GetString("data"));

            // Load file
            using (var binaryStream = new MemoryStream(binaryData))
            {
                try
                {
                    var serializer = new BinaryFormatter();
                    instance = serializer.Deserialize(binaryStream) as GameData;
                    Debug.Log("Save data loaded");
                }
                catch (SerializationException)
                {
                    // If the load is not possible because the save is corrupt, create a new one
                    // TODO either remove this when no more changes will occur to game data or show message to user to inform them it happened
                    Debug.Log("Save data corrupted, creating new one");
                    CreateFreshSave();
                }

            }
        }

        if (instance.HighScores == null)
        {
            instance.HighScores = new Dictionary<string, List<HighScoreValue>>();
        }

        // Convert high scores for usage
        instance.HighScoresFromList();

        return instance;
    }

    /// <summary>
    /// Return all high scores achieved.
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, List<HighScoreValue>> GetHighScores()
    {
        return HighScores;
    }

    /// <summary>
    ///  Save the game data to a save file.
    /// </summary>
    public void Save()
    {
        // Convert high scores for serialization
        HighScoresToList();

        // Serialise data and save to save player prefs
        using (var binaryStream = new MemoryStream())
        {
            var serializer = new BinaryFormatter();
            serializer.Serialize(binaryStream, this);

            // Stored as a string in player prefs
            PlayerPrefs.SetString("data", Convert.ToBase64String(binaryStream.ToArray()));
        }

        PlayerPrefs.Save();

        Debug.Log("Game saved");
    }

    /// <summary>
    /// Create a new save
    /// </summary>
    private static void CreateFreshSave()
    {
        instance = new GameData();
        Debug.Log("New save file created");
        instance.Save();
    }

    #endregion

}
