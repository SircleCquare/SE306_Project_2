using UnityEngine;
using System.Collections;

public class FinishBlock : MonoBehaviour {

	public GameController controller;

	void OnTriggerEnter(Collider other){
		//check if player
		if (other.tag == "Player") {
			controller.setFinishedLevel (true);
		}
		// call the appropriate property setter. The property setter must check the side and set the variables.
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "Player") {
			controller.setFinishedLevel (false);
		}
	}
}
