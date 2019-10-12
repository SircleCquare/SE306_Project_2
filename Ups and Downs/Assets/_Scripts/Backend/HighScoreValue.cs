using System;

/// <summary>
/// A class that represents a high score for a level. 
/// </summary>
[Serializable]
public class HighScoreValue : IComparable<HighScoreValue>
{
    /// <summary>
    /// The level the score is for
    /// </summary>
    public string levelName { get; set; }

    /// <summary>
    /// The point value of the high score
    /// </summary>
    public int pointsValue { get; set; }

    /// <summary>
    /// The name of the player that achieved this high score
    /// </summary>
    public string playerName { get; set; }

    /// <summary>
    /// The time the score was achieved (used to only keep the most recent score. 
    /// </summary>
    public DateTime AchievedTime { get; set; }

    public HighScoreValue(string levelName, int pointsValue, string playerName)
    {
        this.levelName = levelName; 
        this.pointsValue = pointsValue;
        this.playerName = playerName;

        AchievedTime = DateTime.Now;
    }

    /// <summary>
    /// Compare this HighScoreValue to another, based on score. 
    /// 
    /// If two scores are the same, compares them based on the time they were achieved as well. 
    /// </summary>
    /// <param name="other"></param>
    /// <returns> 1 if this is a lower score, -1 if this is a higher score. If the scores are equal,
    ///  returns the result of comparing the time the scores were achieved 
    /// (1 meaning earlier, 0 meaning same time, -1 meaning later).</returns>
    public int CompareTo(HighScoreValue other)
    {
        var result = pointsValue.CompareTo(other.pointsValue);

        return (result == 0 ? AchievedTime.CompareTo(other.AchievedTime) : result) * -1;
    }
}
