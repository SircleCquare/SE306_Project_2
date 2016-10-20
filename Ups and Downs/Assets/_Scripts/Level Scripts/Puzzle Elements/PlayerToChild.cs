using UnityEngine;
using System.Collections;

/// <summary>
/// PlayerToChild is a script to prevent characters and boxes slipping off moving platforms.<br>
/// To use this script, create an empty child of a moving platform.
/// Add a Trigger Collider to that child, where the collider mesh extends above the height of the platform.
/// Then attach this script to that empty child.
/// </summary>
public class PlayerToChild : MonoBehaviour {

    // if player enters collider, attach player to self, and thus make the players transforms relative to self
	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag (GameController.PLAYER_TAG)) {
			other.gameObject.transform.SetParent (this.gameObject.transform);
		}
		if (other.gameObject.CompareTag (GameController.WEIGHTED_TAG)) {
			if (! other.gameObject.transform.parent.CompareTag (GameController.PLAYER_TAG)) {
				other.gameObject.transform.SetParent (this.gameObject.transform);
			}
		}
	}

    // if player enters collider, dettach player from self, and thus make the players transforms no longer relative to self
    void OnTriggerExit(Collider other){
		if (other.gameObject.CompareTag (GameController.PLAYER_TAG)){
			other.gameObject.transform.SetParent (null);
		}
		if (other.gameObject.CompareTag (GameController.WEIGHTED_TAG)) {
			if (! other.gameObject.transform.parent.CompareTag (GameController.PLAYER_TAG)) {
				other.gameObject.transform.SetParent (null);
			}
		}
	}
}
