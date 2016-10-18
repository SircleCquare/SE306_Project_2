using UnityEngine;
using System.Collections;

public class FinishBlock : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		//check if player
        if (other.tag == GameController.PLAYER_TAG)
        {
			GameController controller = GameController.Singleton;
			controller.setFinishedLevel (true);
		}
		// call the appropriate property setter. The property setter must check the side and set the variables.
	}

	void OnTriggerExit(Collider other){
        if (other.tag == GameController.PLAYER_TAG)
        {
			GameController controller = GameController.Singleton;
			controller.setFinishedLevel (false);
		}
	}
}
