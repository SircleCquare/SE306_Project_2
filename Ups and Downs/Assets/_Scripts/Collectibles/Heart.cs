using UnityEngine;
using System.Collections;

/// <summary>
/// A heart collectible will restore one player life.
/// </summary>
public class Heart : Collectible
{

    public override void onPickup()
    {
        if (getGameController().addHeart())
        {
            base.onPickup();
            Debug.Log("Heart Added");
        }
        else
        {
            Debug.Log("Full Hearts");
        }
    }
}
