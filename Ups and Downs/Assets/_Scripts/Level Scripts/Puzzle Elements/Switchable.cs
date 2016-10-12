using UnityEngine;
using System.Collections;

/**
	All implementations of Switchable must implement an activate
	and deactivate procedure.
	
	These are trigged by the player when they press the Activate Key (default E).
	
	Switchables are intended to have two states, active and deactive.

*/
public abstract class Switchable : MonoBehaviour {
	abstract public void toggle();
	abstract public void activate();
	abstract public void deactivate();
}
