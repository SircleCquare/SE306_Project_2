using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class PlayerController : MonoBehaviour {
    /* Interactable Components */
    private GameController gameController;
    // The check which the player will respawn back to if they die.
    private Checkpoint currentCheckpoint;

    /** Configurable Player Variables */
    public Side PlayerSide;

    /** How far away switchs can be activated from */
	public float switchSearchRadius = 1.0f;
	public float darkSideZ = -2.5f;
	public float lightSideZ = 2.5f;
    public float terminalVelocity = 200.0f;

    /** private, persistent, movement variables */
    private float forceY = 0;
    private float invertGrav;
    public float maxSpeed = 10f;
    public float speed;

    public float jumpForce = 10f;
    public float gravity = 5f;
    public float airtime = 1f;

    public float deadzone = 0.2f;
    private Rigidbody rb;
    private float distToGround;

    private float airTimeCount;

    /** Enemy Effects */
    private List<LeechEnemy> leeches;
    private bool invisible = false;


    void Awake() {
		GameController.Singleton.RegisterPlayer (this);
    }

    void Start() {
       rb = GetComponent<Rigidbody>();
       distToGround = GetComponent<Collider>().bounds.extents.y;

       leeches = new List<LeechEnemy>();
       gameController = GameController.Singleton;
    }

    void FixedUpdate()
    {
        if (gameController.getSide() != PlayerSide)
        {
            return;
        }

        if (gameController.isActivate())
        {
            activateSwitchs();
        }

        float moveHorizontal = gameController.getHorizontalMagnitude();
        AdjustFacing(moveHorizontal);

        if (moveHorizontal < deadzone && -deadzone < moveHorizontal)
        {
            moveHorizontal = 0f;
        }
        else
        {
            moveHorizontal = (moveHorizontal > 0) ? 1 : -1;
        }

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f) * speed;

        movement.y = rb.velocity.y;
        rb.velocity = movement;

        Vector3 clampedVelocity = rb.velocity;
        clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -maxSpeed, maxSpeed);
        clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, -jumpForce, jumpForce);
        clampedVelocity.z = 0;
        rb.velocity = clampedVelocity;

        if (IsGrounded())
        {
            if (gameController.isJump())
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                airTimeCount = airtime;
            }
        }
        else
        {
            if (gameController.isJump() && airTimeCount > 0)
            {
                airTimeCount -= Time.fixedDeltaTime;
            }
            else
            {
                rb.AddForce(Vector3.down * gravity * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }

        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.2f - 1.5f);
    }

    public void addHeart()
    {
        Debug.Log("Add Heart");
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
        if (getInventoryItemType() == SpecialItem.None)
        {
            Debug.Log("Added");
            gameController.setInventoryItem(specialItem);
            return true;
        } else
        {
            Debug.Log("Inventory Full");
            return false;
        }
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

//    public IEnumerator HandleInvisiblity(float invisiblityTime)
//    {
//        invisible = true;
//        float time = 0f;
//
//        while (time < invisiblityTime)
//        {
//        }
//        yield return 0;
//        invisible = false;
//    }

    public void MakeInvisible(float invisiblityTime)
    {
//        StartCoroutine(HandleInvisiblity(invisiblityTime));
    }

    public bool IsInvisible()
    {
        return invisible;
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
		ToggleSwitch closeSwitch = getNearbySwitch();
		if (closeSwitch != null) {
			closeSwitch.toggle();
		}
		PushableObject pushblock = getNearbyPushable ();
		if (pushblock != null){
			if (pushblock.attached) {
				pushblock.detach ();
			} else {
				pushblock.attach (gameObject);
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
				Debug.Log("Switch found and returning");
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
				return pushblock;
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

    public void resetToCheckpoint(int checkpoint)
    {
        currentCheckpoint = gameController.getCheckpoint(PlayerSide, checkpoint);
        transform.position = currentCheckpoint.getPosition();

    }

    public int getCheckpointNumber()
    {
        if (currentCheckpoint == null)
        {
            currentCheckpoint = gameController.getCheckpoint(PlayerSide, 0);
        }

        return currentCheckpoint.order;
    }

   /// <summary>
   /// Kills the player. The player returns to their last checkpoint and loses one heart.
   /// </summary>
    public void kill()
    {
        Debug.Log("Killing");

        deLeech();
        foreach (Enemy e in FindObjectsOfType<Enemy>()) {
            e.ResetBehaviour();
        }

        gameController.playerDeath();
        // detatch all leeches from the player.
        leeches = new List<LeechEnemy>();
    }
}
