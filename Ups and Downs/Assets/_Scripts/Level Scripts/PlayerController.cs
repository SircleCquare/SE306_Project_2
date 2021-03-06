﻿using UnityEngine;
using System.Collections.Generic;


public class PlayerController : MonoBehaviour {
    /* Interactable Components */
    private GameController gameController;
    private Animator animator;
    private Rigidbody rb;
    // The check which the player will respawn back to if they die.
    private Checkpoint currentCheckpoint;

    /** Configurable Player Behaviour Variables */
    public Side PlayerSide;
    /* How far away switchs can be activated from */
	public float switchSearchRadius = 1.0f;

    /** Configurable Player Movement Variables */
    public float maxSpeed = 10f;
    public float maxFallSpeed = 30f;
    public float runningForce = 13f;
    public float jumpForce = 10f;
    public float gravity = 5f;
    public float airtime = 1f;
    public float deadzone = 0.01f;

    /** private, persistent, movement variables */
    private float distToGround;
    private float airTimeCount;

    /** Enemy Effects */
    private List<LeechEnemy> leeches;
    public float leechPullFactor = 10f;

    /** direction in terms of x axis*/
    private const int FORWARD = 1;


    void Awake() {
		GameController.Singleton.RegisterPlayer (this);
    }
    
    void Start() {
       rb = GetComponent<Rigidbody>();
       distToGround = GetComponent<Collider>().bounds.extents.y;
       animator = GetComponent<Animator>();
       leeches = new List<LeechEnemy>();
       gameController = GameController.Singleton;
    }

    /// <summary>
    /// Takes the users input and transforms it into a vector which is affected by gravity.
    /// 
    /// This is primarily used to apply the effects of leechs to movement.
    /// </summary>
    /// <returns></returns>
    private Vector3 getModifiedRunningForce()
    {
        float rawInput = gameController.getHorizontalMagnitude();
        if (rawInput < deadzone && -deadzone < rawInput)
        {
            animator.SetBool("RunningFwd", false);
            rawInput = 0f;
        }
        else
        {
            animator.SetBool("RunningFwd", true);
            rawInput = (rawInput > 0) ? 1f : -1f;
        }
        return new Vector3(rawInput * runningForce - getGravityHorizontalComponent(), 0f, 0f);

    }

    void Update()
    {
        // If the player is holding down the interact (E) button.
        if (gameController.isActivate())
        {
            activateSwitchs();
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal;
        // If the player is on the inactive side, kill its horizontal velocity.
        if (gameController.getSide() != PlayerSide)
        {
			animator.SetBool ("RunningFwd", false);
			animator.SetBool ("isJumping", false);
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            return;
        }

        moveHorizontal = gameController.getHorizontalMagnitude();
        AdjustFacing(moveHorizontal);

        Vector3 movement = getModifiedRunningForce();

        // Preserve vertical velocity and assign to rigidbody
        movement.y = rb.velocity.y;
        rb.velocity = movement;

        // Enforce terminal velocity.
        Vector3 clampedVelocity = rb.velocity;
        clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -maxSpeed, maxSpeed);
        clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, -maxFallSpeed, maxFallSpeed);
        clampedVelocity.z = 0;
        rb.velocity = clampedVelocity;

        if (IsGrounded())
        {
            // If the player is on the ground and jumping, apply jump force.
			if (gameController.isJump ()) {
				animator.SetBool ("isJumping", true);
				rb.AddForce (Vector3.up * jumpForce, ForceMode.VelocityChange);
				airTimeCount = airtime;
			} else {
				animator.SetBool ("isJumping", false);
			}
        }
        else
        {
            if (gameController.isJump() && airTimeCount > 0)
            {
				animator.SetBool ("isJumping", true);
                // If the player holds down the jump key, gravity is not applied
                // This gives the appearance of a longer jump.
                airTimeCount -= Time.fixedDeltaTime;
            }
            else
            {
                rb.AddForce(Vector3.down * gravity * Time.fixedDeltaTime, ForceMode.VelocityChange);

            }
        }
    }
    
    private float getGravityHorizontalComponent()
    {
        Vector3 gravityDirection = Vector3.down;
        if (PlayerSide != Side.DARK) return 0f;
        
        CameraPinController cameraPin = GameObject.FindObjectOfType<CameraPinController>();
        if (cameraPin == null)
        {
            Debug.LogError("Could not locate Camera Pin Object");
            return gravityDirection.x;
        }
        return (cameraPin.getGravityDirection() * leechPullFactor).x;
    }

    // Determines whether the player is on the ground.
    // Dist to ground is determined by the height of the capsule collider, 1.5 is applied due to the offset.
    // an extra 0.2 is added to give some margin.
    private bool IsGrounded()
    {
        RaycastHit hit = new RaycastHit();
        var result = Physics.Raycast(transform.position, -Vector3.up, out hit, distToGround + 0.2f - 1.5f);
        return (result) ? (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground")) : false;
    }

    public void addHeart()
    {
        gameController.addHeart();
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
        if (getInventoryItemType() != SpecialItem.None) return false;
        gameController.setInventoryItem(specialItem);
        return true;
    }


    public SpecialItem getInventoryItemType()
    {
        return gameController.getInventoryItemType();
    }
    
    public int getInventoryItemIndex()
    {
        return gameController.getInventoryItemIndex();
    }

    /// <summary>
    /// Removes what ever item was stored in the players inventory.
    /// </summary>
    public void consumeInventoryItem()
    {
        gameController.setInventoryItem(null);
    }

	public void addLeech(LeechEnemy newLeech)
	{
		//Debug.Log("Added Leech");
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
    
     /*
      * Turns the player to face the way they are moving
      */
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
		ToggleSwitch closeSwitch = getNearbySwitch();
		if (closeSwitch != null) {
			closeSwitch.toggle();
		}
        //First check if the player has a pushable block attached.
        PushableObject attachedBlock = GetComponentInChildren<PushableObject>();
        if (attachedBlock != null)
        {
            if (attachedBlock.isAttached())
            {
                attachedBlock.detach();
            }
        }
        else
        {
            // Check for a nearby block to pick up.
            PushableObject nearbyBlock = getNearbyPushable();
            if (IsGrounded() && nearbyBlock != null)
            {
                nearbyBlock.attach(this);
            }
        }
	}

	/**
        A helper method which searchs for switchs that are nearby to the player.
    */
	private ToggleSwitch getNearbySwitch() {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, switchSearchRadius);
		for (int i = 0; i < hitColliders.Length; i++) {
			ToggleSwitch switchObj = hitColliders[i].gameObject.GetComponent<ToggleSwitch>();
			if (switchObj != null) {
				//Debug.Log("Switch found and returning");
				return switchObj;
			}
		}
		return null;
	}

	/**
	 *  (RIP DRY) Helper method to search for nearby pushable blocks.
	 */
	private PushableObject getNearbyPushable() {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, switchSearchRadius);
		for (int i = 0; i < hitColliders.Length; i++) {
			PushableObject pushblock = hitColliders[i].gameObject.GetComponent<PushableObject>();
			if (pushblock != null) {
                Debug.Log("Pushblock found and returning");
                var relativePoint = transform.InverseTransformPoint(pushblock.transform.position);
                // relative point is the position of block relative to the player, when z < 0 block is behind player
                if (relativePoint.z > 0)
                {
                    return pushblock;
                }
			}
		}
		return null;
	}

    /** Updates the players checkpoint */
    public void addCheckPoint(Checkpoint checkPoint)
    {
        if (checkPoint.checkpointSide == PlayerSide)
        {
            if ((currentCheckpoint == null) || checkPoint.isActive() && checkPoint.order >= currentCheckpoint.order)
            {
                currentCheckpoint = checkPoint;
                checkPoint.activate();
            }
        }
    }

    // reset the player to the given checkpoint
    public void resetToCheckpoint(int checkpoint)
    {
        currentCheckpoint = gameController.getCheckpoint(PlayerSide, checkpoint);
        transform.position = currentCheckpoint.getPosition();
		AdjustFacing(FORWARD); 
		PushableObject attachedBlock = GetComponentInChildren<PushableObject>();

        if (attachedBlock != null && attachedBlock.isAttached()) attachedBlock.detach();
    }

    public int getCheckpointNumber()
    {
        if (currentCheckpoint == null)
        {
            currentCheckpoint = gameController.getCheckpoint(PlayerSide, 0);
        }

        return currentCheckpoint.order;
    }

    public bool markedAsDead = false;
   /// <summary>
   /// Kills the player. The player returns to their last checkpoint and loses one heart.
   /// </summary>
    public void kill()
    {
        gameController.playerDeath();
        // detatch all leeches from the player.
        leeches = new List<LeechEnemy>();
    }
}
