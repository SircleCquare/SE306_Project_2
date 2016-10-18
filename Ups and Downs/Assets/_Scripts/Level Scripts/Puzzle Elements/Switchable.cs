using UnityEngine;
using System.Collections;

/**
	Switchables are intended to have two states, active and deactive.

    Switch objects can interact with Switchables and change their state and/or behaviour
*/
public abstract class Switchable : MonoBehaviour {
	abstract public void toggle();
	abstract public void activate();
	abstract public void deactivate();
}
