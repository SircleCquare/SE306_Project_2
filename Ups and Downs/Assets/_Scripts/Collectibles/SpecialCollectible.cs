using UnityEngine;
using System.Collections;

/// <summary>
/// A special collectible is one which the player can pick up and store in their inventory for use
/// at a later point.
/// 
/// </summary>
public class SpecialCollectible : Collectible
{
    // The type of a SpecialCollectible indicates what sort of functionality it provides.
    public SpecialItem itemType = SpecialItem.None;

    /* 
     * The index of a SpecialCollectible is an optional field which can store extra information
     *  about the specific instance of the collectible.
     *  
     *  E.g. Keys with different indexs open different doors.
     */
    public int index;

    public override void onPickup()
    {
        
        GameController controller = getGameController();
        PlayerController player = controller.getActivePlayer();
        if (player.addToInventory(this))
        {
            base.onPickup();
        }
        
    }
}
