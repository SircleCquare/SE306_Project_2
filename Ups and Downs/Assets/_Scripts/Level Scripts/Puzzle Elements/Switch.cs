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
    public SwitchState defaultState = SwitchState.OFF;
    public enum SwitchState { ON, OFF };
	
	/*
		Fire switch when loading scene to ensure all game objects are in right state
	*/
    void Start() {
        foreach (Switchable target in targetList)
        {
            switch (defaultState)
            {
                case (SwitchState.OFF):
                    target.deactivate();
                    break;
                case (SwitchState.ON):
                    target.activate();
                    break;
            }
        }
	}
	/*
		Toggles the state of this switch
	*/
	public void toggle() {
        foreach (Switchable target in targetList)
        {
            target.toggle();
        }
    }
}
