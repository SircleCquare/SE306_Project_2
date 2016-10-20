using UnityEngine;
using System.Collections;

/// <summary>
/// The Camera Pin Controller implements the core 'flip' feature of the game
/// by rotating the parent object of the camera instance around its y axis by 180 degrees.
/// It also implements the 'shaky camera' effect applied when a leech is attached to the player.
/// </summary>
public class CameraPinController : MonoBehaviour {

  /** Both of these fields need to be configured within the Unity scene builder */
  private Transform lightPlayer;
  private Transform darkPlayer;

  /// <summary>
  /// The initial side that the camera view starts on.
  /// </summary>
  public Side initialSide;

  /// <summary>
  /// The mid-point between the two player charcters. This is used to centre
  /// the camera pin and by extension the main camera.
  /// </summary>
	private Vector3 middle;

  /// <summary>
  /// Whether or not the view is currently flipping.
  /// </summary>
	private bool isFlipping = false;

  /// <summary>
  /// The iteration of the current flip progression. Cycles between 0 and 30.
  /// </summary>
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

    // Defaults to dark side so need to change if initial side is light side
    if (initialSide == Side.LIGHT)
    {
      transform.rotation = defaultRotation;
    }
    GameController.Singleton.setCurrentSide(initialSide);
  }

  public void resetShakyCam()
  {
    camera.fieldOfView = defaultFOV;
    Quaternion currentRotation = transform.rotation;
    currentRotation.z = 0;
    transform.rotation = currentRotation;
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
      //Enable or the fog if the camera is on the dark side.
      RenderSettings.fog = (GameController.Singleton.getSide() == Side.DARK);
			isFlipping = false;
			flipStep = 0;
		}
	}

  /// <summary>
  /// Update the position of the camera pin and by extension the camera
  /// to be the mid point of the two player characters.
  /// </summary>
	void setPosition() {
		transform.position = new Vector3(
			middle.x,
			middle.y,
			transform.position.z
		);
	}

  /// <summary>
  /// Applies a 'shaky camera' effect to the camera when a leech is attached to the currently
  /// active player character. The camera lerps between a set of randomly-generated angles and FOV. (Field )
  /// </summary>
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


    public Vector3 getGravityDirection()
    {
        return Vector3.Normalize(Quaternion.Euler(0f, 0f, -transform.eulerAngles.z) * Vector3.down);
    }
}
