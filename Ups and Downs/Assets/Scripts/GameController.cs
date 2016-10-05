using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
    /** Game State */
    private const int MAX_HEALTH = 100;

    GameData gameData; 

    /** The number of seconds a player has to wait between flips */
    public float flipCoolDown = 2.0f;
	public Side currentSide = Side.Dark;
	public KeyCode flipAction = KeyCode.F;
	public KeyCode activateAction = KeyCode.E;
	
	public CameraPinController cameraPinController;
    private bool disableInput = false;
    private float coolDownCount;
    private bool coolDownActive; 

    /* UI components */
    public Slider healthBar;
    public Text timeText;
    public Text characterName;
    public Image characterAvatar;
    public Text flipText;

    // Used to color flip text to show flipping is disabled
    private readonly Color nearlyTransparentWhite = new Color(1, 1, 1, 0.1f);

    void Start()
    {
        gameData = new GameData(); 
        coolDownCount = flipCoolDown;

        //Calculates the number of coins in this level based on the number of objects tagged as Coin.
        GameObject[] coinObjectList;
        coinObjectList = GameObject.FindGameObjectsWithTag("Coin");
        gameData.totalNumberOfCoins = coinObjectList.Length;

        // Set limit for healthbar to allow proper proportion highlighted
        healthBar.maxValue = MAX_HEALTH;

        // Update current character selected
        updateCurrentCharacterDisplay();

        // Update whether flip is active
        coolDownActive = (coolDownCount > 0) ? true : false;
        updateFlipText(); 

    }

    void Update() {
        gameData.time += Time.deltaTime;
        updateTimeDisplay();

        if (coolDownCount < 0)
        {
            // Only updates text on first detection of cool down ending, rather than every frame
            if (coolDownActive)
            {
                coolDownActive = false;
                updateFlipText(); 
            }

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
        gameData.coinsFound++;
    }

    public int getCoinsFound()
    {
        return gameData.coinsFound;
    }

    public int getTotalCoins()
    {
        return gameData.totalNumberOfCoins;
    }

    public int getTime()
    {
        return (int)gameData.time;
    }

    /*
     * Updates the player's current health and adjusts their health bar accordingly
     */
    public void setHealth(int newHealth)
    {
        gameData.health = newHealth;
        healthBar.value = newHealth; 
    }

    public int getHealth()
    {
        return gameData.health;
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

        // Since a flip has occurred, set cool down as active
        coolDownActive = true;

        cameraPinController.doFlip();
		if (currentSide == Side.Dark) {
			currentSide = Side.Light;
		} else {
			currentSide = Side.Dark;
		}
        updateCurrentCharacterDisplay();
        updateFlipText(); 
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

    /*
     * Updates the time displayed in the UI 
     */
    void updateTimeDisplay()
    {
        timeText.text = string.Format("Time: {0:#0.00} seconds", gameData.time); 
    }

    /*
     * Updates the character name and avatar displayed in the UI, based on the current world side
     */ 
    void updateCurrentCharacterDisplay()
    {
        //TODO character avatar once a suitable image available
        //TODO need to update character names
        if(currentSide == Side.Dark)
        {
            characterName.text = "Older brother";
        }else
        {
            characterName.text = "Younger brother";
        }
    }

    /*
     * Update the color of the flip text to show if flipping is enabled or not
     */ 
    void updateFlipText()
    {
        flipText.color = (coolDownActive) ? nearlyTransparentWhite : flipText.color = Color.white;
    }

}
