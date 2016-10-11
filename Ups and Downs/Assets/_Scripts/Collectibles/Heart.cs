using UnityEngine;
using System.Collections;

/// <summary>
/// A heart collectible will restore one player life.
/// </summary>
public class Heart : Collectible
{

    public override void onPickup()
    {
        if (getGameController().getTotalHearts() >= 5)
        {
            Debug.Log("Full Hearts");
            return;
        } else
        {
            getGameController().addHeart();
            base.onPickup();
            Debug.Log("Heart Added");
        }
    }
       

}
