using UnityEngine;
using System.Collections;

/**
 * A script to attach to a platform for regular vertical movement.
 * */
public class PlatformMovementVerticalScript : MonoBehaviour {

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


	// Use this for initialization
	void Start () {
		startHeight = transform.position;
		currentHeight = 0.0f;
		systemSpeed = 0.001f;
		finalSpeed = userSpeed * systemSpeed;
		canMove = false;

		Invoke ("setMoveFlag", startTimeOffset);
	}

	// needed for Invoke
	void setMoveFlag(){
		canMove = true;
	}
	
	// Update is called once per frame
	// needs to go from current height to liftHeight
	// the lerp value needs to increase slightly
	// but also be clamped between 0 and 1.

	void Update()
	{
		if (canMove) {
			MovePlatform ();
		} else {
			return;
		}
	}

	void MovePlatform(){
		// set the y transform
		transform.position = startHeight + (Vector3.up * currentHeight);
		// lerp the currentHeight
		currentHeight = Mathf.Lerp (0.0f, liftHeight, lerpValue);
		// change the lerpValue
		if (isAscending) {
			lerpValue = Mathf.Clamp ((lerpValue + finalSpeed), 0.0f, 1.0f);
		} else {
			lerpValue = Mathf.Clamp ((lerpValue - finalSpeed), 0.0f, 1.0f);
		}
		// check if lerpValue at max/min
		if (lerpValue >= 1.0f) {
			isAscending = false;
			if (pauseTime > 0f) {
				canMove = false;
				Invoke ("setMoveFlag", pauseTime);
			}
		}
		if (lerpValue <= 0.0f) {
			isAscending = true;
			if (pauseTime > 0f) {
				canMove = false;
				Invoke ("setMoveFlag", pauseTime);
			}
		}
	}
}
