using UnityEngine;
using System.Collections;

public class CameraPinController : MonoBehaviour {
	
	/** Both of these fields need to be configured within the Unity scene builder */
	private Transform lightPlayer;
    private Transform darkPlayer;
	
//	public Camera camera;
	
	private Vector3 middle;
	
	private bool isFlipping = false;
	
	private int flipStep = 0;

    void Start()
    {
        lightPlayer = GameController.Singleton.getLightPlayer().gameObject.transform;
        darkPlayer = GameController.Singleton.getDarkPlayer().gameObject.transform;
    }

	// Update is called once per frame
	void Update () {
		middle = (lightPlayer.position + darkPlayer.position) * 0.5f;
		if (isFlipping) {
			flip();
		}
		setPosition();
	}
	
	public void doFlip() {
		isFlipping = true;
	}
	
	void flip() {
		if (flipStep < 30) {
			transform.Rotate(0, 6, 0);
			flipStep++;
		} else {
			isFlipping = false;
			flipStep = 0;
		}
	}
	
	void setPosition() {
		transform.position = new Vector3(
			middle.x,
			middle.y,
			transform.position.z
		);
	}
}
