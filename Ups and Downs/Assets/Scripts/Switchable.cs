using UnityEngine;
using System.Collections;

public abstract class Switchable : MonoBehaviour {
	abstract public void activate();
	abstract public void deactivate();
}
