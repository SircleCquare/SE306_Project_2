using UnityEngine;
using System.Collections;

/**
 * A persistent class that persists through scene changes.
 * ApplicationModel stores game state in conjunction with GameData.
 * */
public static class ApplicationModel {

	public static int score;
	public static float time;
    public static string levelName;
	public static int levelNumber;
	public static int coinsFound;
	public static int totalCoins;
    public static int deathCount;

}
