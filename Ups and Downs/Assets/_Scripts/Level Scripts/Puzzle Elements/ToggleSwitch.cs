﻿using UnityEngine;
using System.Collections;

/**
	A switch can be activated by the player when they are close enough by pressing E.
	
	A switch will then toggle the state of all attached Switchables to the new state of
	this switch.

*/
public class ToggleSwitch : Switch {

	public Side switchSide = Side.LIGHT;

	/*
		Fire switch when loading scene to ensure all game objects are in right state
	*/
    protected override void Start() {
        base.Start();
		// get switch material color, and set to default red
		gameObject.GetComponent<Renderer>().material.color =  Color.red;
	}
	/*
		Toggles the state of this switch
	*/
	public void toggle() {

		if (GameController.Singleton.getSide() == switchSide)
		{
		    // toggle color of switch
		    gameObject.GetComponent<Renderer>().material.color = gameObject.GetComponent<Renderer>().material.color == Color.red ? Color.green : Color.red;

		    foreach (Switchable target in targetList)
			{
				target.toggle();
			}
		}
    }
}
