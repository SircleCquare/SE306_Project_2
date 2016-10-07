using UnityEngine;
using System.Collections;

/**
	An implementation of the Switchable abstract class.
	A toggleable block changes transparency and collision when activated.
*/
public class ToggleBlock : Switchable {
	
	/** The material which will be rendered when this object is active */
	public Material activeMaterial;
	/** The material which will be rendered when this object is deactive */
	public Material inactiveMaterial;

    public bool locked = true;
	
	private Renderer rend;
	private Collider doorCollider;
	

	// Use this for initialization
	void Start () {
		doorCollider = GetComponent<Collider>();
		rend = GetComponent<Renderer>();
        doorCollider.enabled = locked;
		if (locked) {
			rend.material = activeMaterial;
		} else {
			rend.material = inactiveMaterial;
		}
	}
	
	public override void toggle() {
		if (locked) {
			rend.material = inactiveMaterial;
			doorCollider.enabled = false;
			locked = false;
		} else {
			rend.material = activeMaterial;
			doorCollider.enabled = true;
			locked = true;
		}
	}
}
