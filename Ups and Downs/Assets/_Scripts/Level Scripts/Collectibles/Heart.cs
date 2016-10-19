using UnityEngine;
using System.Collections;

/// <summary>
/// A heart collectible restores one player life.
/// </summary>
public class Heart : Collectible
{
  public override void onPickup()
  {
    // Attempt to add a life to the player.
    if (GameController.Singleton.addHeart())
    {
        base.onPickup();
    }
  }
}
