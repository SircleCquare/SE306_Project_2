using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// This class keeps both player characters within view on the screen.
/// It also flashes the borders of the window as the players approach the edge.
/// </summary>
public class CameraMovement : MonoBehaviour {

	/** Both of these fields need to be configured within the Unity scene builder */
	public Transform lightPlayer;
	public Transform darkPlayer;

	/** Increasing this value will decrease playable area within the camera view port */
	public float boundsPadding = 0.05f;

    /** Used to flash a box around the screen to indicate that the players can't go further*/
    public Image CameraBoundImage;
    private bool flash = false;
    private bool allowedOff = false;

    void Start()
    {
        lightPlayer = GameController.Singleton.getLightPlayer().gameObject.transform;
        darkPlayer = GameController.Singleton.getDarkPlayer().gameObject.transform;
    }

    // Update is called once per frame
    void Update() {
        if (allowedOff == true)
        {
            Debug.Log("allowedOff");
        }
        if (!allowedOff)
        {
            keepOnScreen(lightPlayer);
            keepOnScreen(darkPlayer);
        }

        // Flash bounds on screen if edge reached
        // TODO ensure that this image is set for all scenes
	    if (CameraBoundImage != null)
	    {
            CameraBoundImage.color = flash ? Color.white : Color.Lerp(CameraBoundImage.color, Color.clear, 5 * Time.deltaTime);
            //Debug.Log(flash + " " + CameraBoundImage.color);
        }
        flash = false;
    }

    public void allowOffScreen(bool allowedOff)
    {
        this.allowedOff = allowedOff;
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
