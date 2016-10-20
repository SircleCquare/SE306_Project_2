using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// This class keeps both player characters within view on the screen.
/// It also flashes the borders of the window as the players approach the edge.
/// </summary>
public class CameraMovement : MonoBehaviour {

	/** Both of these fields need to be configured within the Unity scene builder */
	public PlayerController lightPlayer;
	public PlayerController darkPlayer;

	/** Increasing this value will decrease playable area within the camera view port */
	public float boundsPadding = 0.05f;

    public bool active { get; set; }

    /** Used to flash a box around the screen to indicate that the players can't go further*/
    public Image CameraBoundImage;
    private bool flash = false;

    void Start()
    {
        active = true;
        lightPlayer = GameController.Singleton.getLightPlayer();
        darkPlayer = GameController.Singleton.getDarkPlayer();
    }

    // Update is called once per frame
    void Update() {
        if (active)
        {
            keepOnScreen(lightPlayer);
            keepOnScreen(darkPlayer);

            // Flash bounds on screen if edge reached
            // TODO ensure that this image is set for all scenes
            if (CameraBoundImage != null)
            {
                CameraBoundImage.color = flash ? Color.white : Color.Lerp(CameraBoundImage.color, Color.clear, 5 * Time.deltaTime);
                //Debug.Log(flash + " " + CameraBoundImage.color);
            }
            flash = false;
        }
		
    }

	/*
	*	Ensures both players are visible on the screen at all times
	*/
	private void keepOnScreen(PlayerController player) {
		Vector3 pos = Camera.main.WorldToViewportPoint (player.transform.position);
        pos.x = Mathf.Clamp(pos.x, 0 + boundsPadding, 1- boundsPadding);
        if (pos.y < 0)
        {
            GameController.Singleton.playerDeath();
        } else
        {
            if (pos.x.Equals(0 + boundsPadding) || pos.x.Equals(1 - boundsPadding))
            {
                flash = true;
            }
            player.transform.position = Camera.main.ViewportToWorldPoint(pos);
        }

	}

}
