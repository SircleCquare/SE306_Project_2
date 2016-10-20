using UnityEngine;
using System.Linq;
using System.Collections;

/// <summary>
/// The Story Text Trigger holds customisable text that is triggered (made visible)
/// when the player hits an invisible trigger in the level. This is used to advance
/// the story based on where the player is in the level.
/// </summary>
public class StoryTextTrigger : MonoBehaviour {

  /// <summary>
  /// The text that should be triggerd on coming in range of this trigger.
  /// </summary>
  private GameObject[] storyText;

  /// <summary>
  /// External other targets that should be made active upon reaching this trigger.
  /// </summary>
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
    // Activate all text.
		foreach (GameObject text in storyText) {
			text.SetActive (true);
		}
	}
}
