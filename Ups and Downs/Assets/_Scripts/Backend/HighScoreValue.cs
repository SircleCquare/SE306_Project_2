using System;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;

[Serializable]
public class HighScoreValue : IComparable<HighScoreValue>
{
    public int Score { get; set; }

    public string PlayerName { get; set; }

    public DateTime AchievedTime { get; set; }

    public HighScoreValue(int score, string playerName)
    {
        Score = score;
        PlayerName = playerName;

        AchievedTime = DateTime.Now;
    }

    /// <summary>
    /// Compare this HighScoreValue to another, based on score. 
    /// 
    /// If two scores are the same, compares them based on the time they were achieved as well. 
    /// </summary>
    /// <param name="other"></param>
    /// <returns> 1 if this is a lower score, -1 if this is a higher score. If the scores are equal,
    ///  returns the result of comparing the time the scores were achieved.</returns>
    public int CompareTo(HighScoreValue other)
    {
        var result = Score.CompareTo(other.Score) * -1;

        return result == 0 ? AchievedTime.CompareTo(other.AchievedTime) : result;
    }
}
