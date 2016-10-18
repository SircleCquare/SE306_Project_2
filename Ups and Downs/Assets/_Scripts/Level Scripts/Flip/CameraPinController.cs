using UnityEngine;
using System.Collections;

public class CameraPinController : MonoBehaviour {

    /** Both of these fields need to be configured within the Unity scene builder */
    private Transform lightPlayer;
    private Transform darkPlayer;
	
	private Vector3 middle;
	
	private bool isFlipping = false;
	
	private int flipStep = 0;

    public float fovRange, defaultFOVSpeed, fovSpeedRange;

    private float defaultFOV, toFOV, fromFOV, fovSpeed;
    private float fovLerpProgress = 0f;
    private new Camera camera;

    public bool enableLSDCam { get; set; }

    void Start()
    {
        lightPlayer = GameController.Singleton.getLightPlayer().gameObject.transform;
        darkPlayer = GameController.Singleton.getDarkPlayer().gameObject.transform;
        enableLSDCam = false;

        camera = GetComponentInChildren<Camera>();
        defaultFOV = camera.fieldOfView;
        toFOV = defaultFOV;
    }

	// Update is called once per frame
	void Update () {
		middle = (lightPlayer.position + darkPlayer.position) * 0.5f;
		if (isFlipping) {
			flip();
		}

        setPosition();
        applyLSDCam();
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

    void applyLSDCam()
    {
        if (GameController.Singleton.getSide() == Side.Light || isFlipping || !enableLSDCam)
        {
            camera.fieldOfView = defaultFOV;
            return;
        }

        if (camera.fieldOfView == toFOV)
        {
            fovLerpProgress = 0f;
            fromFOV = camera.fieldOfView;
            toFOV = (Random.value * 2 - 1) * fovRange + defaultFOV;
            fovSpeed = (Random.value * 2 - 1) * fovSpeedRange + defaultFOVSpeed;
        }

        fovLerpProgress += Time.deltaTime * fovSpeed/30;
        camera.fieldOfView = Mathf.Lerp(fromFOV, toFOV, fovLerpProgress);
    }
}
