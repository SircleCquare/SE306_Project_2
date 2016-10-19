using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class GameData
{

    [NonSerialized] private static GameData instance;

    [NonSerialized] private static string saveFile;

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
    [NonSerialized] Dictionary<string, SortedDictionary<int, string>> HighScores;

    /// <summary>
    /// An array for storing high score strings for serialization. 
    /// 
    /// The first element is the level name. The second element is the player's name. 
    /// The indexes correleate with the indexes in the high score values array which 
    /// contain the score that matches that player name and value. 
    /// </summary>
    private string[,] highScoreLevelAndPlayerName;

    /// <summary>
    /// An array that stores high score values. 
    /// Each row corresponds to a the same row in the highScoreLevelAndPlayerName array. 
    /// </summary>
    private int[] highScoreValues; 
     

    /// <summary>
    /// Maximum number of high scores stored per level 
    /// </summary>
    private const int MAX_HIGH_SCORES = 5; 

    public int HighestLevelUnlockedNumber { get; set; }

    public int TotalNumberOfCoins { get; set; }
    
    public int LevelNumber { get; set; }

    /** The number of Coins the player has found in this play through */
    public int CoinsFound { get; set; }

    /** The score from coins (each coin can have it's own point setting) */
    public int CoinScore { get; set; }

    public float Time { get; set; }

    public int Heart { get; set; }

    private SpecialItem itemType = SpecialItem.None;
    private int itemIndex = -1;

    public const int MAX_HEALTH = 5;

    /// <summary>
    /// Set the location of the save file
    /// </summary>
    static void SetSaveFile()
    {
        saveFile = saveFile ?? Application.persistentDataPath + "save.ser";
    }

    private GameData()
    {
        // Initialise level state
        clearLevelState(); 

        // List to store names of achievements
        awardedAchievements = new List<String>();  
        HighScores = new Dictionary<string, SortedDictionary<int, string>>(5);
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
    }

    #region Inventory
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

    #endregion


    #region HighScores

    /// <summary>
    /// Store the dictionary of high scores as arrays for serialization.
    /// </summary>
    private void HighScoresToArrays()
    {
        var numScores = 0;

        foreach (var key in HighScores.Keys)
        {
            numScores += HighScores[key].Count; 
        } 

        highScoreLevelAndPlayerName = new string[numScores, 2];
        highScoreValues = new int[numScores];

        var i = 0; 

        foreach (var level in HighScores.Keys)
        {
            foreach (var score in HighScores[level])
            {
                highScoreLevelAndPlayerName[i, 0] = level;
                highScoreLevelAndPlayerName[i, 1] = score.Value;

                highScoreValues[i] = score.Key;

                i++; 
            }
        }
    }

    /// <summary>
    /// Read the 2 arrays storing high score values and build the dictionary of highscores. 
    /// </summary>
    private void HighScoresFromArrays()
    {
        var numScores = highScoreValues.Length;

        for (var i = 0; i < numScores; i++)
        {
            var levelName = highScoreLevelAndPlayerName[i, 0];
            var playerName = highScoreLevelAndPlayerName[i, 1];
            var score = highScoreValues[i];

            if (!HighScores.ContainsKey(levelName))
            {
                HighScores[levelName] = new SortedDictionary<int, string>(); 
            }

            HighScores[levelName].Add(score, playerName);
        }
    }

    /// <summary>
    /// Test if the current score is in the top five high scores. 
    /// </summary>
    /// <returns>True if this is score is higher than the lowest score on the leaderboard, 
    ///          or there is space to add highscores to the leaderboard</returns>
    public bool IsHighScore(string levelName, int score)
    {
        var highScores = GetOrderedHighScoresForLevel(levelName).Keys;

        return highScores.Count < MAX_HIGH_SCORES || score > highScores.Min();
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
            if (!(highScores.Count < 5))
            {
                highScores.Remove(highScores.Keys.Min());
            }

            // Add highscore
            highScores[score] = playerName;
        }
    }

    /// <summary>
    /// Get an ordered dictionary of high scores for a level
    /// </summary>
    /// <param name="levelName"></param>
    /// <returns></returns>
    public SortedDictionary<int, string> GetOrderedHighScoresForLevel(string levelName)
    {
        // Create a dictionary to store high scores if none exists
        if (!HighScores.ContainsKey(levelName))
        {
            HighScores[levelName] = new SortedDictionary<int, string>();
        }

        return HighScores[levelName];
    }

    #endregion

    #region Persistence
    /// <summary>
    /// Load the game data from a save file
    /// </summary>
    /// <returns>The loaded game data instance</returns>
    public static GameData LoadInstance()
    {
        SetSaveFile(); 

        if (!File.Exists(saveFile))
        {
            // If no save exists, create new game data and save
            Debug.Log("No save file found");
            CreateFreshSave();
        }
        else
        {
            // Load file
            using (var saveFileStream = File.Open(saveFile, FileMode.Open))
            {
                try
                {
                    var serializer = new BinaryFormatter();
                    instance = serializer.Deserialize(saveFileStream) as GameData;
                    saveFileStream.Close();
                    Debug.Log("Save data loaded");
                }
                catch (SerializationException)
                {
                    // If the load is not possible because the save file is corrupt, create a new one
                    // TODO either remove this when no more changes will occur to game data or show message to user to inform them it happened
                    Debug.Log("Save data corrupted, creating new one");
                    // Need to close save file first to allow it to be overwritten in another method
                    saveFileStream.Close();
                    CreateFreshSave();
                }

            }
        }

        // Convert high scores for usage
        instance.HighScoresFromArrays();

        return instance;
    }

    /// <summary>
    ///  Save the game data to a save file. 
    /// </summary>
    public void Save()
    {
        // Convert high scores for serialization
        HighScoresToArrays();

        // Remove existing save data
        if (!File.Exists(saveFile))
        {
            Debug.Log("Remove existing save file to be replaced");

            File.Delete(saveFile);
        }

        // Serialise data and save to save file
        using (var saveFileStream = File.Create(saveFile))
        {
            var serializer = new BinaryFormatter();
            serializer.Serialize(saveFileStream, this);
            saveFileStream.Close();
        }

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
