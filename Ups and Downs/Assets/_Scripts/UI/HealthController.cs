using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HealthController : MonoBehaviour {

	public GameObject[] hearts = new GameObject[5];

	private int heartCount = GameController.MAX_HEALTH;

	public void hideLastHeart() {
		if (heartCount > 0) {
            heartCount--;
            hearts[heartCount].SetActive (false);
		}
	}

	public void showLastHeart() {
		if (heartCount < GameController.MAX_HEALTH) {
			hearts[heartCount].SetActive (true);
			heartCount++;
		}
	}

	public void showAllHearts() {
		foreach (GameObject h in hearts) {
			h.SetActive (true);
		}
	}
}
