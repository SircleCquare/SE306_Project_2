using UnityEngine;
using System.Collections;

public class StoryTextTrigger : MonoBehaviour {

	private GameObject[] storyText;

	void Start () {
        storyText = new GameObject[transform.childCount];
        for (var i = 0; i < transform.childCount; i++)
        {
            storyText[i] = (GameObject)transform.GetChild(i).gameObject;
            storyText[i].SetActive (false);
        }
	}
	
	void OnTriggerEnter(Collider other){
		foreach (GameObject text in storyText) {
			text.SetActive (true);
		}
	}
}
