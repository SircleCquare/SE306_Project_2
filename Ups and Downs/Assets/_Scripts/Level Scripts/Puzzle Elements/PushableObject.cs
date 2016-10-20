using UnityEngine;

/**
 * Attach this script to an object that you want to be pushed by a character.
 * Press 'E' (default) to push the block.
 * */

public class PushableObject : MonoBehaviour  {
    private const float paddingFactor = 0.1f;

    private Rigidbody rb;
    private bool attached;
    private Vector3 startPosition;
    public float attachedHeight = 0.5f;
    private Side gameSide;
    private float zValue;

    public float attachDistance = 3f;

	void Start() {
		attached = false;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;

        gameSide = (transform.position.z > 0) ? Side.LIGHT : Side.DARK;
        startPosition = transform.position;

        /* Ensure box is aligned with the player */
        startPosition.z = GameController.Singleton.getZValueForSide(gameSide);
        transform.position = startPosition;
	}
    
    void Update()
    {
        float yPosition = Camera.main.WorldToViewportPoint(transform.position).y;

        if (yPosition < (0 - paddingFactor) || yPosition > (1 + paddingFactor))
        {
            transform.position = startPosition;
        }
    }

    public void attach(PlayerController pushingPlayer) {
        if (pushingPlayer.PlayerSide != gameSide)
        {
            return;
        }

		// TODO: check if player is carrying anything
		// if player, get the transform and make this parent to transform.
		this.transform.SetParent (pushingPlayer.transform);
		attached = true;
        rb.isKinematic = true;

        // Ensures that the attach distance between the pushable block and the player
        // is fixed.
        Vector3 directionToObject = rb.position - pushingPlayer.transform.position;
        directionToObject = Vector3.Normalize(directionToObject) * attachDistance;

        Vector3 newPosition =  pushingPlayer.transform.position + directionToObject;
        newPosition.y = attachedHeight;
        transform.position = newPosition;


        // TODO: set character's carrying object boolean value
        // TODO: reduce character speed and jump height if object is heavy

    }

    public void detach()
    {
        Debug.Log("DETACHED");
        this.transform.SetParent(null);
        attached = false;
        rb.isKinematic = false;

        /* Ensure box is re-aligned with player */
        var alignedPosition = transform.position;
        alignedPosition.z = GameController.Singleton.getZValueForSide(gameSide);
        transform.position = alignedPosition;
    }


    public bool isAttached()
    {
        return attached;
    }
}
