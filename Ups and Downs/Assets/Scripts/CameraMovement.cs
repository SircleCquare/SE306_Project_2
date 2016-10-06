using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	
	/** Both of these fields need to be configured within the Unity scene builder */
	public Transform player1;
	public Transform player2;

	/** Increasing this value will decrease playable area within the camera view port */
	public float boundsPadding = 0.05f;

    /** Used to flash a box around the screen to indicate that the players can't go further*/
    public Image CameraBoundImage;
    private bool flash = false; 

	// Update is called once per frame
	void Update() {
		keepOnScreen(player1);
		keepOnScreen(player2);

        // Flash bounds on screen if edge reached
        CameraBoundImage.color = flash ? Color.white : Color.Lerp(CameraBoundImage.color, Color.clear, 5 * Time.deltaTime);
        Debug.Log(flash + " " + CameraBoundImage.color);
        flash = false;
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
            if (pos.x.Equals(0 + boundsPadding) || pos.x.Equals(1 - boundsPadding))
            {
                flash = true; 
            } 
            trans.position = Camera.main.ViewportToWorldPoint(pos);
        }
       
	}
	
}
