using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class GameData
{

    [NonSerialized] private static GameData instance;

    [NonSerialized] private static string saveFile;

    public List<string> awardedAchievements;

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

    /// <summary>
    /// Obtain an instance of the game data. 
    /// </summary>
    /// <returns></returns>
    public static GameData GetInstance()
    {
        return instance ?? (instance = new GameData());
    }

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

        return instance;
    }

    /// <summary>
    ///  Save the game data to a save file. 
    /// </summary>
    public void Save()
    {
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



}
