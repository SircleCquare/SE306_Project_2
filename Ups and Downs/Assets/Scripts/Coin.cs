using UnityEngine;
using System.Collections;

/**
    The coin collectible provides a direct score benefit when picked up.
*/
public class Coin : Collectible {
    public int score;

	public override void onPickup() {
        GameObject[] gameControllerList;
        gameControllerList = GameObject.FindGameObjectsWithTag("GameController");
        GameController controller = null;
        foreach (GameObject obj in gameControllerList)
        {
            controller = obj.GetComponent<GameController>();
            if (controller != null)
            {
                break;
            }
        }
        if (controller == null)
        {
            Debug.LogError("Could not find active Game Controller Object");
            return;
        }
        controller.addScore(score);
		Debug.Log("Coin picked up");
		//Play a coin-specific sound?
	}
}
