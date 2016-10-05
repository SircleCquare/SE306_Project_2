using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    /** Player State */
    
    
    

    /** The side of the player assigned this Controller */
	public Side PlayerSide;
    /** Must be set to an active game controller object */
    public GameController inputControl;
    /** The checkpoint this player has initially. */
    public Checkpoint initialCheckpoint;
	public float speed = 10.0F;
    public float jumpSpeed = 20.0F;
    public float gravity = 20.0F;
    public float gravityForce = 3.0f;
    public float airTime = 1f;
	
    /** How far away switchs can be activated from */
	public float switchSearchRadius = 5.0f;
	public float darkSideZ = -2.5f;
	public float lightSideZ = 2.5f;

    // The most recent check point this player has.
    private Vector3 mostRecentCheckpoint;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private float forceY = 0;
    private float invertGrav;
	private bool locked = false;
	public bool carryingObject { get; set; }
	
    void Start() {
        invertGrav = gravity + airTime;
        controller = GetComponent<CharacterController>();
        mostRecentCheckpoint = initialCheckpoint.getPosition();
    }

    void Update()
    {
		updateMovement();
		if (inputControl.getSide() == PlayerSide) {
			if (inputControl.isActivate()) {
				activateSwitchs();
			}

			Vector3 currentPosition = transform.position;
			currentPosition.z = (inputControl.getSide () == Side.Dark) ? darkSideZ : lightSideZ;
			currentPosition.x = Mathf.Round(transform.position.x * 1000f)/1000f;
			currentPosition.y = Mathf.Round(transform.position.y * 1000f)/1000f;
			transform.position = currentPosition;
		}
    }

  

    /** Updates the users horizontal and vertical movement based on input */
    private void updateMovement() {
		float horizontalMag;
		bool jump;
		if (inputControl.getSide() != PlayerSide) {
			horizontalMag = 0f;
			jump = false;
		} else {
			horizontalMag = inputControl.getHorizontalMagnitude();
			jump = inputControl.isJump();
		}
		
        moveDirection = new Vector3(horizontalMag, 0, 0);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
        if (controller.isGrounded) {
            forceY = 0;
            invertGrav = gravity * airTime;
            if (jump)
            {
                forceY = jumpSpeed;
            }
        }

        if (jump && forceY != 0) {
            invertGrav -= Time.deltaTime;
            forceY += invertGrav * Time.deltaTime;
        }

        forceY -= gravity * Time.deltaTime * gravityForce;
        moveDirection.y = forceY;
        controller.Move(moveDirection * Time.deltaTime);		
	}
	
    /**
        Called every frame by Update() to activate nearby Switchs. Only called if the Activate action key is pressed.
    */
	private void activateSwitchs() {
		Switch closeSwitch = getNearbySwitch();
		if (closeSwitch != null) {
			closeSwitch.toggle();
		}
		
	}
	
	/**
        A helper method which searchs for switchs that are nearby to the player.
    */
	private Switch getNearbySwitch() {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, switchSearchRadius);
		for (int i = 0; i < hitColliders.Length; i++) {
			Switch switchObj = hitColliders[i].gameObject.GetComponent<Switch>();
			if (switchObj != null) {
				Debug.Log("Switch found and returning");
				return switchObj;
			}
		}
		return null;
	}

    /** Updates the players checkpoint */
    public void addCheckPoint(Checkpoint checkPoint)
    {
        if (checkPoint.checkpointSide == PlayerSide)
        {
            if (checkPoint.isActive())
            {
                mostRecentCheckpoint = checkPoint.getPosition();
                checkPoint.activate();
            }
        }
    }

    /**
        Called to kill the player and return them to their last respawn point.
    */
    public void kill()
    {
        Debug.Log("Killing");
        transform.position = mostRecentCheckpoint;
		inputControl.resetHealth();
		Invoke("Unlock", 2);
		StartCoroutine(DamageFlash());
    }

	void OnControllerColliderHit(ControllerColliderHit hit){
		if(hit.gameObject.tag == "enemy" && !locked){
			DecreaseHealth(hit.gameObject.GetComponent<Damage>().damage);
		}
	}

	void DecreaseHealth(int damage){
		Lock();
		int health = inputControl.getHealth();
		if (health <= damage) {
			kill();
			return;
		}
		inputControl.setHealth((health - damage));
		Debug.Log("health: " + (health - damage).ToString());
		Invoke("Unlock", 2);
		StartCoroutine(DamageFlash());
	}

	void Unlock(){
		locked = false;
	}

	void Lock(){
		locked = true;
	}

	IEnumerator DamageFlash(){
		for (int i = 0; i < 10; i++) {
			Color32 normalColour = GetComponent<Renderer>().material.color;
			GetComponent<Renderer>().material.color = Color.white;
			yield return new WaitForSeconds(.1f);
			GetComponent<Renderer>().material.color = normalColour;
			yield return new WaitForSeconds(.1f);
		}
	}
}