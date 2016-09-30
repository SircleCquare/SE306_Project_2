using UnityEngine;
using System.Collections;

/**
	A switch can be activated by the player when they are close enough by pressing E.
	
	A switch will then toggle the state of all attached Switchables to the new state of
	this switch.

*/
public  class Switch : MonoBehaviour {
	
	public Switchable[] targetList;
	
	/** Default state of swtich */
	public bool initialState = false;
	
	/*
		Fire switch when loading scene to ensure all game objects are in right state
	*/
	void Start() {
		initialState = !initialState;
		toggle();
	}
	/*
		Toggles the state of this switch
	*/
	public void toggle() {
		initialState = !initialState;
		for (int i = 0; i < targetList.Length; i++) {
			Switchable target = targetList[i];
			if (initialState) {
				target.activate();
			} else {
				target.deactivate();
			}
		}
	}
}
