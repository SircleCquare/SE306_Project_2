﻿using UnityEngine;
using System.Collections;

public class TriggerSphere : MonoBehaviour {
    
	public Switchable[] targetList;
	
    /**
        Activates the target list when a player enters the target sphere.
    */
    void OnTriggerEnter(Collider col)
    {
		if (col.gameObject.tag == "PlayerGround")
			{
				Debug.Log("ENTER");
				setToggle();
			}
    }

    /**
       Deactivates the target list when a player leaves the target sphere.
    */
    void OnTriggerExit(Collider col)
    {
		if (col.gameObject.tag == "PlayerGround")
		{
			Debug.Log("EXIT");
			setToggle();
		}
    }
	
	private void setToggle() {
		for (int i = 0; i < targetList.Length; i++)
        {
            Switchable target = targetList[i];
            target.toggle();
        }
	}
}
