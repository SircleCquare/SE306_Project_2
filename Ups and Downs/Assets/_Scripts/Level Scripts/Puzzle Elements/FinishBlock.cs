using UnityEngine;
using System.Collections;

/// <summary>
/// The Finish Block provides an end point to the level. Upon both player characters reaching
/// their respective finish blocks the level ends.
/// </summary>
public class FinishBlock : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		//If the colliding object is a player character
        if (other.tag == GameController.PLAYER_TAG)
        {
			GameController controller = GameController.Singleton;
			controller.setFinishedLevel (true);
            Light glow = GetComponentInChildren<Light>();
            if (glow != null) glow.enabled = true;
		}
	}

	void OnTriggerExit(Collider other){
        //If the colliding object is a player character
        if (other.tag == GameController.PLAYER_TAG)
        {
			GameController controller = GameController.Singleton;
            controller.setFinishedLevel (false);
            Light glow = GetComponentInChildren<Light>();
            if (glow != null) glow.enabled = false;
        }
	}
}
