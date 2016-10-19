using UnityEngine;
using System.Collections;

/// <summary>
/// The coin collectible provides a customisable score bonus when picked up.
/// </summary>
public class Coin : Collectible {

  /// <summary>
  /// The value the coin should add onto the score when picked up. default is 10 points.
  /// NOTE: this will not translate to a direct increase in the final level score of 10 points.
  /// The actual scoring calculation depends as well on level time and number of deaths.
  /// </summary>
  public int score = 10;

  /// <summary>
  /// The sound to play on picking this coin up.
  /// </summary>
  public AudioClip pickupSound;

  /// <summary>
  /// Override the onPickup() method in the Collectible parent.
  /// This method is called when the coin is picked up by colliding with the player.
  /// </summary>
	public override void onPickup() {

      GameController controller = GameController.Singleton;
      if (controller == null)
      {
          Debug.LogError("Could not find active Game Controller Object");
          return;
      }

      // Get the camera's position in space.
      Vector3 cameraPos = Camera.main.transform.position;

      // Round to whole number due to spurious issue with directional sound.
      cameraPos.z = Mathf.Round(cameraPos.z);

      // Play the pickup sound at (approximately) the camera's position.
      AudioSource.PlayClipAtPoint (pickupSound, cameraPos, 1f);

      // Notify the game controller that a coin has been picked up.
      controller.foundCoin(score);
      base.onPickup();
	}
}
