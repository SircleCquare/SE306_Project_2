using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	
	
	public Side currentSide = Side.Dark;
	public KeyCode flipAction = KeyCode.F;
	private bool disableInput = false;
	
	
	void Update() {
		if (isFlipDown()) {
			Debug.Log("Flipping");
			flipWorld();
			//TODO: Implement some sort of Coroutine to trigger camera pan + animation before
			// 	programatically switching sides.
			return;
		}
	}
	
	/*
		Called by the Game Controller to flip the world.
	*/
	private void flipWorld() {
		Debug.Log("Side: " + currentSide);
		if (currentSide == Side.Dark) {
			currentSide = Side.Light;
		} else {
			currentSide = Side.Dark;
		}
	}
	
	/*
		Gets the current side which is active
	*/
	public Side getSide() {
		return currentSide;
	}
	
	/*
		Only returns true for the first frame this key is pressed down.
	*/
	public bool isFlipDown() {
		if (!disableInput) {
			return Input.GetKeyDown(flipAction);
		} else {
			return false;
		}
	}
	
	/*
	*	Will return true as long as the move right action key is being held down.
	*	
	*	If input is disabled for the GameController, this method will always return false.
	*/
	public bool isMoveRight() {
		if (!disableInput) {
			return Input.GetAxis("Horizontal") > 0f;
		} else {
			return false;
		}
	}
	
	/*
	*	Will return true as long as the move left action key is being held down.
	*	
	*	If input is disabled for the GameController, this method will always return false.
	*/
	public bool isMoveLeft() {
		if (!disableInput) {
			return Input.GetAxis("Horizontal") < 0f;
		} else {
			return false;
		}
	}
	
	/*
	*	Returns a value between -1 and 1 which represents the horizontal 
	*	magnitude of the users input.
	*
	*	If the user is pressing the left action key (Default A), this value will be negative.
	*	If the user is pressing the right action key (Default D), this value will be positive.
	*/
	public float getHorizontalMagnitude() {
		if (!disableInput) {
			return Input.GetAxis("Horizontal");
		} else {
			return 0f;
		}
	}
	
	/*
	*	Will return true as long as the jump action key is being held down.
	*	
	*	If input is disabled for the GameController, this method will always
	*	return false.
	*/
	public bool isJump() {
		if (!disableInput) {
			return Input.GetButton("Jump");
		} else {
			return false;
		}
	}

}
