using UnityEngine;
using System.Collections;

/**
    The coin collectible provides a direct score benefit to the player that picks it up.

*/
public class Coin : Collectible {

	public override void onPickup() {
		base.onPickup();
		Debug.Log("Coin picked up");
		//Play a coin-specific sound?
	}
}
