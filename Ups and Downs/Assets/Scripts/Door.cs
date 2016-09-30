using UnityEngine;
using System.Collections;

public class Door : Switchable {
	
	/** The material which will be rendered when this object is active */
	public Material activeMaterial;
	/** The material which will be rendered when this object is deactive */
	public Material deactiveMaterial;
	
	private Renderer rend;
	private Collider doorCollider;
	

	// Use this for initialization
	void Start () {
		doorCollider = GetComponent<Collider>();
		rend = GetComponent<Renderer>();
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
