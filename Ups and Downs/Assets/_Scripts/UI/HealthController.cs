using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HealthController : MonoBehaviour {

	public GameObject[] hearts = new GameObject[5];

	private int heartCount = GameController.MAX_HEALTH;

    // Hides a graphic for the rightmost heart, to inform the player they have lost a life
	public void hideLastHeart() {
		if (heartCount > 0) {
            heartCount--;
            hearts[heartCount].SetActive (false);
		}
	}

    // Shows the grpahic for the leftmost missing heart, to inform the player they have gained a life
    public void showLastHeart() {
		if (heartCount < GameController.MAX_HEALTH) {
			hearts[heartCount].SetActive (true);
			heartCount++;
		}
	}

    // Makes all hearts visible, to reset the state of lives on a scene load
	public void showAllHearts() {
		foreach (GameObject h in hearts) {
			h.SetActive (true);
		}
	}
}
