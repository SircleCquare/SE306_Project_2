using UnityEngine;
using System.Collections;

/// <summary>
/// The invisibility collectible disables the interactions of enemies
/// with the Dark player for a configurable time.
/// This can be used as a level mechanic to allow area denial by enemies
/// until the collectible can be obtained.
/// </summary>
public class Invisibility : Collectible {

  /// <summary>
  /// The duration of the invisibility effect.
  /// </summary>
  public float InvisiblityTime = 5f;

  public override void onPickup() {
    base.onPickup();
    // Make the Dark Player invisible for a time.
    GameController.Singleton.getDarkPlayer().MakeInvisible(InvisiblityTime);
  }
}
