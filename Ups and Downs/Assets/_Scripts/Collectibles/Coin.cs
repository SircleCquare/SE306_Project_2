using UnityEngine;
using System.Collections;

/**
    The coin collectible provides a direct score benefit when picked up.
*/
public class Coin : Collectible {
    public int score;

	public override void onPickup() {
        GameController controller = getGameController();
        if (controller == null)
        {
            Debug.LogError("Could not find active Game Controller Object");
            return;
        }
        controller.foundCoin();
        base.onPickup();
		Debug.Log("Coin picked up");
		//Play a coin-specific sound?
	}
}
