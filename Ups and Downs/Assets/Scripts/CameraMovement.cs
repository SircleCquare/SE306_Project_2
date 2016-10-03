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
			keepOnScreen(player1);
			keepOnScreen(player2);
	}
	
	/*
	*	Ensures both players are visible on the screen at all times
	*/
	private void keepOnScreen(Transform trans) {
		Vector3 pos = Camera.main.WorldToViewportPoint (trans.position);
        pos.x = Mathf.Clamp(pos.x, 0 + boundsPadding, 1- boundsPadding);
        //pos.y = Mathf.Clamp(pos.y, 0 + boundsPadding, 1- boundsPadding);
        if (pos.y < 0)
        {
            PlayerController controller = trans.gameObject.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.kill();
            }
        } else
        {
            trans.position = Camera.main.ViewportToWorldPoint(pos);
        }
       
	}
	
}
