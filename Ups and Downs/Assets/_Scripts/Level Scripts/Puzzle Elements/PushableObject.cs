using UnityEngine;
using System.Collections;

/**
 * Attach this script to an object that you want to be pushed by a character.
 * Press 'E' (default) to push the block.
 * */

public class PushableObject : MonoBehaviour  {
    private const float paddingFactor = 0.1f;

    private Rigidbody rb;
    public bool attached;
    private Vector3 startPosition;
    private float distToGround;

    public float attachDistance = 3f;

	void Start() {
		attached = false;
        rb = GetComponent<Rigidbody>();
	    startPosition = transform.position;
        distToGround = GetComponent<Collider>().bounds.extents.y;
	}

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround);
    }
    
    void Update()
    {
        float yPosition = Camera.main.WorldToViewportPoint(transform.position).y;

        if (yPosition < (0 - paddingFactor) || yPosition > (1 + paddingFactor))
        {
            transform.position = startPosition;
        }
    }

	public void attach(GameObject pushingPlayer) {
		// TODO: check if player is carrying anything
		// if player, get the transform and make this parent to transform.
		this.transform.SetParent (pushingPlayer.transform);
		attached = true;
        rb.isKinematic = true;

        Vector3 directionToObject = rb.position - pushingPlayer.transform.position;
        directionToObject = Vector3.Normalize(directionToObject) * attachDistance;

        rb.position = pushingPlayer.transform.position + directionToObject;


        // TODO: set character's carrying object boolean value
        // TODO: reduce character speed and jump height if object is heavy

    }

    public void detach()
    {
        Debug.Log("DETACHED");
        this.transform.SetParent(null);
        attached = false;
        rb.isKinematic = false;
    }
	
}
