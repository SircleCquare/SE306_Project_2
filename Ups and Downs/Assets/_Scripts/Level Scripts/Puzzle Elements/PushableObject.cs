﻿using UnityEngine;
using System.Collections;

/**
 * Attach this script to an object that you want to be pushed by a character.
 * Press 'E' (default) to push the block.
 * */

public class PushableObject : MonoBehaviour  {

    private Rigidbody rb;
    public bool attached;

	void Start() {
		attached = false;
        rb = GetComponent<Rigidbody>();
    }

	public void attach(GameObject pushingPlayer) {
		// TODO: check if player is carrying anything
		// if player, get the transform and make this parent to transform.
		this.transform.SetParent (pushingPlayer.transform);
		attached = true;
        // TODO: set character's carrying object boolean value
        // TODO: reduce character speed and jump height if object is heavy
        rb.isKinematic = true;
    }

	public void detach(){
		this.transform.SetParent (null);
		attached = false;
        rb.isKinematic = false;
    }
	
}
