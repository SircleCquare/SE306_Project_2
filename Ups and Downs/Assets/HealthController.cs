using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HealthController : MonoBehaviour {

	public GameObject[] hearts = new GameObject[5];

	private int heartCount = 4;
	public void hideLastHeart() {
		if (heartCount >= 0) {
			hearts [heartCount].SetActive (false);
			heartCount--;
		}
	}

	public void showLastHeart() {
		if (heartCount < 5) {
			hearts [heartCount].SetActive (true);
			heartCount++;
		}
	}

	public void showAllHearts() {
		foreach (GameObject h in hearts) {
			h.SetActive (true);
		}
	}
}
