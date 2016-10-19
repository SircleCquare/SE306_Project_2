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

		Vector3 pos = Camera.main.transform.position;
		pos.z = Mathf.Round(pos.z);
		AudioSource.PlayClipAtPoint (pickupSound, pos, 1f);
        controller.foundCoin(score);
        base.onPickup();
		Debug.Log("Coin picked up");
	}
}
