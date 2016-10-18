using UnityEngine;
using System.Collections;

public class Invisibility : Collectible {
    public float InvisiblityTime = 5f;

    public override void onPickup() {
        base.onPickup();
        getGameController().getDarkPlayer().MakeInvisible(InvisiblityTime);
    }
}
