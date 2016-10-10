using UnityEngine;
using System.Collections;

public class SpecialCollectible : Collectible
{
    public override void onPickup()
    {
        Debug.Log("Picked");
        GameController controller = getGameController();
        PlayerController player = controller.getActivePlayer();
        player.addToInventory(this);
        base.onPickup();
    }
}
