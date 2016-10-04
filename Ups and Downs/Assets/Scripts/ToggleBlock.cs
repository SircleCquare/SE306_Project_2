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
	public Material deactiveMaterial;

    public bool lockedState = true;
	
	private Renderer rend;
	private Collider doorCollider;
	

	// Use this for initialization
	void Start () {
		doorCollider = GetComponent<Collider>();
		rend = GetComponent<Renderer>();
        if (lockedState)
        {
            activate();
        } else
        {
            deactivate();
        }
	}
	
	public override void activate() {
		rend.material = activeMaterial;
		doorCollider.enabled = true;
	}
	public override void deactivate() {
		rend.material = deactiveMaterial;
		doorCollider.enabled = false;
	}
}
