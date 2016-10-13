using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class PlayerController : MonoBehaviour {
    /** Player State */
	private List<LeechEnemy> leeches;


    /** The side of the player assigned this Controller */
	public Side PlayerSide;

    //accessed via Singleton now.
    private GameController inputControl;

    // The check which the player will respawn back to if they die.
    private Checkpoint currentCheckpoint;

	public float speed = 10.0F;
    public float jumpSpeed = 20.0F;
    public float gravity = 20.0F;
    public float gravityForce = 3.0f;
    public float airTime = 1f;
    public float leechSpeedMultiplier = 3.5f;
    public float leechJumpMultiplier = 1.5f;

    /** How far away switchs can be activated from */
	public float switchSearchRadius = 5.0f;
	public float darkSideZ = -2.5f;
	public float lightSideZ = 2.5f;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private float forceY = 0;
    private float invertGrav;
	public bool carryingObject { get; set; }
	private Color32 normalColour;
	public Color32 flashColour = Color.white;
	public float invulnerabilityTime = 2.0f;
	private Animator animator;

    // Whether the player is on a platform.
    private bool groundContact;


    void Awake() {
		GameController.Singleton.RegisterPlayer (this);
        
    }

    void Start() {
        invertGrav = gravity + airTime;
		controller = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();

		//normalColour = GetComponent<Renderer>().material.color;
		invulnerabilityTime = Mathf.Round(invulnerabilityTime / 0.2f) * 0.2f;
        leeches = new List<LeechEnemy>();
        inputControl = GameController.Singleton;

    

        // Get the initial Checkpoint for the scene.
        
        
    }

    void Update()
    {
		if (inputControl == null) {
			Debug.Log ("NULL");
		}
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

    /// <summary>
    /// Updates the users horizontal and vertical movement based on input 
    /// </summary>
    private void updateMovement()
    {
        float horizontalMag;
        bool jump;
        if (inputControl.getSide() != PlayerSide)
        {
            horizontalMag = 0f;
            jump = false;
        }
        else
        {
            horizontalMag = inputControl.getHorizontalMagnitude();
            jump = inputControl.isJump();
        }

        //animator.SetBool("Moving", ((horizontalMag == 0) ? false : true));
        AdjustFacing(horizontalMag);

        moveDirection = new Vector3(horizontalMag, 0, 0);
//        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= leechMultiplier(speed, leechSpeedMultiplier);
        if (controller.isGrounded)
        {
            forceY = 0;
            invertGrav = gravity * airTime;
            if (jump)
            {
                forceY = leechMultiplier(jumpSpeed, leechJumpMultiplier);
                groundContact = false;
            }
        }

        if (jump && forceY != 0)
        {
            invertGrav -= Time.deltaTime;
            forceY += invertGrav * Time.deltaTime;
        }

        forceY -= gravity * Time.deltaTime * gravityForce;
        forceY = Mathf.Clamp(forceY, -jumpSpeed, jumpSpeed);
        moveDirection.y = forceY;
        controller.Move(moveDirection * Time.deltaTime);
    }

    /// <summary>
    /// Everytime the player lands on a ground object, its jump count is reset.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            groundContact = true;
        }
    }

    public void addHeart()
    {
        Debug.Log("Add Heart");
        inputControl.addHeart();
    }

    /// <summary>
    /// Attempts to add an item to the players inventory. If the players inventory is empty
    /// the item will be added and the result will be true.
    /// 
    /// Otherwise the item will not be added and false will be returned.
    /// </summary>
    /// <param name="specialItem"></param>
    /// <returns></returns>
    public bool addToInventory(SpecialCollectible specialItem)
    {
        if (getInventoryItemType() == SpecialItem.None)
        {
            Debug.Log("Added");
            inputControl.setInventoryItem(specialItem);
            return true;
        } else
        {
            Debug.Log("Inventory Full");
            return false;
        }
	}


    public SpecialItem getInventoryItemType()
    {
        return inputControl.getInventoryItemType();
    }
    
    public int getInventoryItemIndex()
    {
        return inputControl.getInventoryItemIndex();
    }

    /// <summary>
    /// Removes what ever item was stored in the players inventory.
    /// </summary>
    public void consumeInventoryItem()
    {
        inputControl.setInventoryItem(null);
    }

	public void addLeech(LeechEnemy newLeech)
	{
		Debug.Log("Added Leech");
        leeches.Add(newLeech);
	}

	public void deLeech()
	{
        foreach(LeechEnemy leech in leeches)
        {
            leech.Destroy();
        }
        leeches = new List<LeechEnemy>();
	}

    private float leechMultiplier(float value, float multiplier)
    {
        return (leeches.Count > 0) ? value / (multiplier * Mathf.Log(leeches.Count + 1)) : value;
    }

    private void AdjustFacing(float horizontalDirection)
    {
        if (horizontalDirection > 0)
        {
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        if ( horizontalDirection < 0 )
        {
            transform.eulerAngles = new Vector3(0, 270, 0);
        }
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
            if (checkPoint.isActive() && checkPoint.order >= currentCheckpoint.order)
            {
                currentCheckpoint = checkPoint;
                checkPoint.activate();
            }
        }
    }

   /// <summary>
   /// Kills the player. The player returns to their last checkpoint and loses one heart.
   /// </summary>
    public void kill()
    {
        Debug.Log("Killing");
        if (currentCheckpoint == null)
        {
            currentCheckpoint = inputControl.getCheckpoint(PlayerSide, 0);
        }
        transform.position = currentCheckpoint.getPosition();
        inputControl.removeHeart();
		Invoke("Unlock", invulnerabilityTime);
		StopCoroutine("DamageFlash");
		StartCoroutine("DamageFlash");

        foreach (Enemy e in FindObjectsOfType<Enemy>()) {
            e.ResetBehaviour();
        }
    }


	IEnumerator DamageFlash(){
//		int numLoops = (int)(invulnerabilityTime / 0.2f);
		for (int i = 0; i < invulnerabilityTime; i++) {
			GetComponent<Renderer>().material.color = flashColour;
			yield return new WaitForSeconds(.1f);
			GetComponent<Renderer>().material.color = normalColour;
			yield return new WaitForSeconds(.1f);
		}
	}
}
