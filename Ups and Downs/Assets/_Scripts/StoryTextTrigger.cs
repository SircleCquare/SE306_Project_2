using UnityEngine;
using System.Collections;

public class StoryTextTrigger : MonoBehaviour {

	public GameObject[] storyText;

	void Start () {
		foreach (GameObject text in storyText) {
			text.SetActive (false);
		}
	}
	
	void OnTriggerEnter(Collider other){
		foreach (GameObject text in storyText) {
			text.SetActive (true);
		}
	}
}
