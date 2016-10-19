using UnityEngine;
using System.Collections;

public class CameraPinController : MonoBehaviour {

    /** Both of these fields need to be configured within the Unity scene builder */
    private Transform lightPlayer;
    private Transform darkPlayer;
	
	private Vector3 middle;
	
	private bool isFlipping = false;
	
	private int flipStep = 0;

    public float fovRange, defaultFOVSpeed, fovSpeedRange,
        rotationRange, defaultRotationSpeed, rotationSpeedRange;

    private float defaultFOV, toFOV, fromFOV, fovSpeed,
        rotationSpeed;
    private Quaternion defaultRotation, toRotation, fromRotation;
    private float fovLerpProgress = 0f,
        rotationLerpProgress = 0f;
    private new Camera camera;

    public bool enableShakyCam { get; set; }

    void Start()
    {
        lightPlayer = GameController.Singleton.getLightPlayer().gameObject.transform;
        darkPlayer = GameController.Singleton.getDarkPlayer().gameObject.transform;
        enableShakyCam = false;

        camera = GetComponentInChildren<Camera>();
        defaultFOV = camera.fieldOfView;
        toFOV = defaultFOV;

        defaultRotation = transform.rotation;
        defaultRotation.y = 180;

        toRotation = defaultRotation;
    }

    // Update is called once per frame
    void Update() {
        middle = (lightPlayer.position + darkPlayer.position) * 0.5f;
        if (isFlipping) {
            flip();
        }
        setPosition();
        applyShakyCam();
	}
	
	public void doFlip() {
		isFlipping = true;
	}
	
	void flip() {
		if (flipStep < 30) {
            // Reset Z rotation, in case changed by shaky cam
            Quaternion currentRotation = transform.rotation;
            currentRotation.z = 0;

            transform.rotation = currentRotation;
			transform.Rotate(0, 6, 0); // Flip 6 degrees per frame

			flipStep++;
		} else {
            RenderSettings.fog = (GameController.Singleton.getSide() == Side.DARK);
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

    void applyShakyCam()
    {
        if (GameController.Singleton.getSide() == Side.LIGHT || isFlipping || !enableShakyCam)
        {
            // Reset FOV if shaky cam not active
            camera.fieldOfView = defaultFOV;
            return;
        }

        if (camera.fieldOfView == toFOV)
        {
            // Generate new FOV parameters when FOV movement complete
            fovLerpProgress = 0f;
            fromFOV = camera.fieldOfView;
            toFOV = (Random.value * 2 - 1) * fovRange + defaultFOV; // Value in range (defaultFOV +- fovRange)
            fovSpeed = (Random.value * 2 - 1) * fovSpeedRange + defaultFOVSpeed;
        }

        if (transform.rotation == toRotation)
        {
            // Generate new rotation parameters when rotation movement complete
            rotationLerpProgress = 0f;
            fromRotation = transform.rotation;
            float toRotationZ = (Random.value * 2 - 1) * rotationRange + defaultRotation.z; // Value in range (defaultRotation.z +- rotationRange)
            toRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, toRotationZ);
            rotationSpeed = (Random.value * 2 - 1) * rotationSpeedRange + defaultRotationSpeed;
        }

        // Interpolate FOV
        fovLerpProgress += Time.deltaTime * fovSpeed/30;
        camera.fieldOfView = Mathf.Lerp(fromFOV, toFOV, fovLerpProgress);

        // Interpolate camera shake
        rotationLerpProgress += Time.deltaTime * rotationSpeed / 30;
        transform.rotation = Quaternion.Slerp(fromRotation, toRotation, rotationLerpProgress);
    }
}
