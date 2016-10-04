using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    /** Game State */
    /** The number of Coins the player has found in this play through */
    private int coinsFound = 0;
    private int totalNumberOfCoins;
    public float time = 0.0f;
    private int health;
    public int maxHealth;

    /** The number of seconds a player has to wait between flips */
    public float flipCoolDown = 2.0f;
	public Side currentSide = Side.Dark;
	public KeyCode flipAction = KeyCode.F;
	public KeyCode activateAction = KeyCode.E;
	
	
	public CameraPinController cameraPinController;
    private bool disableInput = false;
    private float coolDownCount;

    void Start()
    {
        coolDownCount = flipCoolDown;

        //Calculates the number of coins in this level based on the number of objects tagged as Coin.
        GameObject[] coinObjectList;
        coinObjectList = GameObject.FindGameObjectsWithTag("Coin");
        totalNumberOfCoins = coinObjectList.Length;
    }

    void Update() {
        time += Time.deltaTime;
        if (coolDownCount < 0)
        {
            if (isFlipDown())
            {
                Debug.Log("Flipping");
                flipWorld();
                coolDownCount = flipCoolDown;
                return;
            }
        }
        else
        {
            coolDownCount -= Time.deltaTime;
            if (isFlipDown())
            {
				//Logging on every frame is bad, mkay?
               // Debug.Log("Cannot Flip. Cooldown remaining: " + coolDownCount);
            }
        }
	}
    public void foundCoin()
    {
        coinsFound++;
    }

    public int getCoinsFound()
    {
        return coinsFound;
    }

    public int getTotalCoins()
    {
        return totalNumberOfCoins;
    }

    public int getTime()
    {
        return (int)time;
    }

    public void setHealth(int newHealth)
    {
        health = newHealth;
    }

    public int getHealth()
    {
        return health;
    }

    public PlayerController getActivePlayer()
    {
        GameObject[] playerObjectList;
        playerObjectList = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in playerObjectList)
        {
            PlayerController player = obj.GetComponent<PlayerController>();
            if (player != null)
            {
                if (player.PlayerSide == getSide())
                {
                    return player;
                }
            }
        }
        return null;
    }
	
	/*
		Called by the Game Controller to flip the world.
	*/
	private void flipWorld() {
		Debug.Log("Side: " + currentSide);
		cameraPinController.doFlip();
		if (currentSide == Side.Dark) {
			currentSide = Side.Light;
		} else {
			currentSide = Side.Dark;
		}
	}
	
	/*
		Gets the current side which is active
	*/
	public Side getSide() {
		return currentSide;
	}
	
	/*
		Only returns true for the first frame this key is pressed down.
	*/
	public bool isFlipDown() {
		if (!disableInput) {
			return Input.GetKeyDown(flipAction);
		} else {
			return false;
		}
	}
	
	
	public bool isActivate() {
		if (!disableInput) {
			return Input.GetKeyDown(activateAction);
		} else {
			return false;
		}
	}
	/*
	*	Will return true as long as the move right action key is being held down.
	*	
	*	If input is disabled for the GameController, this method will always return false.
	*/
	public bool isMoveRight() {
		if (!disableInput) {
			return Input.GetAxis("Horizontal") > 0f;
		} else {
			return false;
		}
	}
	
	/*
	*	Will return true as long as the move left action key is being held down.
	*	
	*	If input is disabled for the GameController, this method will always return false.
	*/
	public bool isMoveLeft() {
		if (!disableInput) {
			return Input.GetAxis("Horizontal") < 0f;
		} else {
			return false;
		}
	}
	
	/*
	*	Returns a value between -1 and 1 which represents the horizontal 
	*	magnitude of the users input.
	*
	*	If the user is pressing the left action key (Default A), this value will be negative.
	*	If the user is pressing the right action key (Default D), this value will be positive.
	*/
	public float getHorizontalMagnitude() {
		float adjust = 1f;
		if (currentSide == Side.Light) {
			adjust = -1f;
		}
		if (!disableInput) {
			return adjust * Input.GetAxis("Horizontal");
		} else {
			return 0f;
		}
	}
	
	/*
	*	Will return true as long as the jump action key is being held down.
	*	
	*	If input is disabled for the GameController, this method will always
	*	return false.
	*/
	public bool isJump() {
		if (!disableInput) {
			return Input.GetButton("Jump");
		} else {
			return false;
		}
	}

}
