using UnityEngine;
using System.Collections;

/// <summary>
/// The coin collectible provides a direct score benefit when picked up.
/// </summary>
//[RequireComponent (typeof(AudioSource))]
public class Coin : Collectible {
    public int score = 10;
	public AudioClip pickupSound;

	public override void onPickup() {
        GameController controller = getGameController();
        if (controller == null)
        {
            Debug.LogError("Could not find active Game Controller Object");
            return;
        }
		AudioSource.PlayClipAtPoint (pickupSound, Camera.main.transform.position);
        controller.foundCoin(score);
        base.onPickup();
		Debug.Log("Coin picked up");
		//Play a coin-specific sound?
		//WISH GRANTED

	}
}
