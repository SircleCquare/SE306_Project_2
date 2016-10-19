using UnityEngine;
using System.Linq;
using System.Collections;

public class StoryTextTrigger : MonoBehaviour {

    private GameObject[] storyText;
    public GameObject[] externalTargets;

    void Start () {
//        storyText = existing.Concat(externalTargets).ToArray();
//        foreach (GameObject text in storyText)
//        {
//            text.SetActive (false);
//        }
        var existing = new GameObject[transform.childCount];
        for (var i = 0; i < transform.childCount; i++)
        {
            existing[i] = (GameObject)transform.GetChild(i).gameObject;
            existing[i].SetActive (false);
        }
        foreach (GameObject obj in externalTargets)
        {
            obj.SetActive (false);
        }
        storyText = existing.Concat(externalTargets).ToArray();
	}
	
	void OnTriggerEnter(Collider other){
		foreach (GameObject text in storyText) {
			text.SetActive (true);
		}
	}
}
