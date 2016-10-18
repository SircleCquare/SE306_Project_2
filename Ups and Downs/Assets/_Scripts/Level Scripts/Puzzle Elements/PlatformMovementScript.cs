using UnityEngine;
using System.Collections;

/**
 * A script to attach to a platform for regular vertical movement.
 * */
public class PlatformMovementScript : Switchable {

	/* liftHeight is what the final height of the vertically moving platform will be.*/
	public float liftHeight;
	/* userSpeed is a float value that can be altered to give the final speed of the platform. */
	public float userSpeed;

	/* The amount of time that should pass before this platform begins to move.*/
	public float startTimeOffset;

	/* The amount of time platforms should pause at the zenith and nadir of the movement*/
	public float pauseTime;

	private float systemSpeed;
	private float finalSpeed;

	private float lerpValue;
	private Vector3 startHeight;
	private float currentHeight;
	private bool isAscending;
	private bool canMove;

    /* Code to convert to Switchable */
    private bool isMoving;
    public MovingState defaultState = MovingState.MOVING;
    private MovingState currentState;
    public enum MovingState { MOVING, STATIONARY };


	// Use this for initialization
	void Start () {
		startHeight = transform.position;
		currentHeight = 0.0f;
		systemSpeed = 0.001f;
		finalSpeed = userSpeed * systemSpeed;

        currentState = defaultState;
		canMove = false;
        if (defaultState == MovingState.MOVING)
        {
            Invoke("setMoveFlag", startTimeOffset);
        }
	}

	// needed for Invoke
	void setMoveFlag(){
		canMove = true;
	}
	
	// Update is called once per frame
	// needs to go from current height to liftHeight
	// the lerp value needs to increase slightly but be clamped between 0 and 1.
	void Update()
	{
		if (canMove) {
			MovePlatform ();
		}
	}

	private void MovePlatform(){
		// set the y transform
		transform.position = startHeight + (Vector3.up * currentHeight);
		// lerp the currentHeight
		currentHeight = Mathf.Lerp (0.0f, liftHeight, lerpValue);

        // change the lerpValue (increase if ascending, decrease otherwise)
        var lerpChange = (isAscending) ? (lerpValue + finalSpeed) : (lerpValue - finalSpeed);
        lerpValue = Mathf.Clamp (lerpChange, 0.0f, 1.0f);

		// check if lerpValue at max/min
		if (lerpValue >= 1.0f) {
			isAscending = false;
            Pause();
		}
		if (lerpValue <= 0.0f) {
			isAscending = true;
            Pause();
		}
	}

    private void Pause()
    {
        if (pauseTime > 0f) {
            canMove = false;
            Invoke ("setMoveFlag", pauseTime);
        }
    }


    public override void toggle()
    {
        // Toggle the current state -> No attention paid to default State
        if (currentState == MovingState.MOVING)
        {
            currentState = MovingState.STATIONARY;
            canMove = false;
        }
        else
        {
            currentState = MovingState.MOVING;
            setMoveFlag();
        }
    }
    public override void activate()
    {
        // Set currentState to opposite of defaultState
        if (defaultState == MovingState.MOVING)
        {
            currentState = MovingState.STATIONARY;
            canMove = false;
        }
        else
        {
            currentState = MovingState.MOVING;
            setMoveFlag();
        }
    }
    public override void deactivate()
    {
        // Set currentState to equal defaultState
        if (defaultState == MovingState.MOVING)
        {
            currentState = MovingState.MOVING;
            setMoveFlag();
        }
        else
        {
            currentState = MovingState.STATIONARY;
            canMove = false;
        }
    }


}
