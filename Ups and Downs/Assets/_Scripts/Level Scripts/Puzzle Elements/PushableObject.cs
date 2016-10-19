using UnityEngine;
using System.Collections;

/**
 * Attach this script to an object that you want to be pushed by a character.
 * Press 'E' (default) to push the block.
 * */

public class PushableObject : MonoBehaviour  {

    private Rigidbody rb;
    private bool attached;
    private Side gameSide;

	void Start() {
		attached = false;
        rb = GetComponent<Rigidbody>();

        Vector3 startPosition = transform.position;
        if (transform.position.z > 0)
        {
            gameSide = Side.LIGHT;
            startPosition.z = GameController.Singleton.lightSideZ;
        }
        else
        {
            gameSide = Side.DARK;
            startPosition.z = GameController.Singleton.darkSideZ;
        }
        transform.position = startPosition;
    }

	public void attach(PlayerController pushingPlayer) {
		// TODO: check if player is carrying anything
		// if player, get the transform and make this parent to transform.
        if (pushingPlayer.PlayerSide != gameSide)
        {
            return;
        }
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

    public bool isAttached()
    {
        return attached;
    }
}
