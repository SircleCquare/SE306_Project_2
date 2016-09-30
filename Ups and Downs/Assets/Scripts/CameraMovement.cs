using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	
	/** Both of these fields need to be configured within the Unity scene builder */
	public Transform player1;
	public Transform player2;
	
	/** Increasing this value will decrease playable area within the camera view port */
	public float boundsPadding = 0.05f;

	// Update is called once per frame
	void Update() {
		setCameraPosition();
		keepOnScreen(player1);
		keepOnScreen(player2);
	}
	
	/*
		Centers the camera between the two players.
	*/
	private void setCameraPosition() {
		Vector3 middle = (player1.position + player2.position) * 0.5f;
		Camera.main.transform.position = new Vector3(
			middle.x,
			middle.y,
			Camera.main.transform.position.z
		);
	}
	
	/*
	*	Ensures both players are visible on the screen at all times
	*	TODO: Stop one player from pulling the other
	*/
	private void keepOnScreen(Transform trans) {
		Vector3 pos = Camera.main.WorldToViewportPoint (trans.position);
        pos.x = Mathf.Clamp(pos.x, 0 + boundsPadding, 1- boundsPadding);
        pos.y = Mathf.Clamp(pos.y, 0 + boundsPadding, 1- boundsPadding);
        trans.position = Camera.main.ViewportToWorldPoint(pos);
	}
	
}
