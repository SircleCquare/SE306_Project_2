using UnityEngine;
using System.Collections;

/**
 * Attach this script to an object that you want to be pushed by a character.
 * Press 'E' (default) to push the block.
 * */
public class PushableObjectScript : Switch {
		
	public bool heavyObject;
	private GameObject pushingPlayer;
	
		
	/*public override void setActive(){
		// forces on character also affect the block
		// makes the block a child of the character.
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5.0f);
		for (int i = 0; i < hitColliders.Length; i++) {
			// check if object == player
			if (hitColliders [i].gameObject.tag == "Player") {
				// TODO: check if player is carrying anything
				// if player, get the transform and make this parent to transform.
				pushingPlayer = hitColliders [i].gameObject;
				this.transform.SetParent (pushingPlayer.transform);
				// TODO: set character's carrying object boolean value
				// TODO: reduce character speed and jump height if object is heavy
			}
		}
	}

	public override void setDeactive(){
		//forces on character now do not affect the block
	}*/
		// forces on character now do not affect the block
		this.transform.SetParent(null);
		pushingPlayer = null;
		// TODO: change back all variables that were set in setActive()
	}
}
