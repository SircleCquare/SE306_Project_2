using UnityEngine;
using System.Collections;

public class PlayerToChild : MonoBehaviour {

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
