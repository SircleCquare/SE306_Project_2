﻿using System;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; 
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine.SceneManagement;

public class GameController : SingletonObject<GameController> {
    public const string PLAYER_TAG = "Player";
	public const string WEIGHTED_TAG = "Weighted";

    public bool renderSwitchPaths = false;

    private GameData gameData;
	private const int TUTORIAL_SCENE_INDEX = 2;

    /** The number of seconds a player has to wait between flips */
    public float flipCoolDown = 0.8f;

    // If you want to change the initial side, update the camera pin controller.
    private Side currentSide = Side.DARK;
	public KeyCode flipAction = KeyCode.F;
	public KeyCode activateAction = KeyCode.E;

    public const int MAX_HEALTH = 5;

    public CameraPinController cameraPinController;
    public CameraMovement cameraMovement;
    private bool disableInput = false;
    private float coolDownCount;
    private bool coolDownActive; 

	private bool finishedLevelDark;
	private bool finishedLevelLight;

	private bool inMainMenu;

    // Sound played when achievement pops
    public AudioClip achievementSound; 

    /* UI components */
    public HealthController healthBar;
    public Text timeText;
    public Text characterName;
    public Image characterAvatar;
    public Text flipText;
	public Text score;
    public GameObject achievementPopUp;
    public Text achievementText;
    public GameObject dialogBox;
    public Text dialogBoxCharacterName;
    public Text dialogBoxMessage;

    public float darkSideZ = -2.5f;
    public float lightSideZ = 2.5f;

    private bool achievementDisplayed = false;
    private float achievementPopUpCountdown = 2.0f;

    private PlayerController lightPlayer, darkPlayer;

    public Sprite darkCharacter;
    public Sprite lightCharacter;


    // Checkpoint list
    private List<Checkpoint> lightSideCheckpoints = new List<Checkpoint>();
    private List<Checkpoint> darkSideCheckpoints = new List<Checkpoint>();

    // Used to color flip text to show flipping is disabled
    private readonly Color nearlyTransparentWhite = new Color(1, 1, 1, 0.1f);

    void Start()
    {
        // Ensures transparent objects are renderered in the correct order.
        Camera.main.transparencySortMode = TransparencySortMode.Orthographic;

        gameData = GameData.LoadInstance();
		gameData.clearLevelState ();

        coolDownCount = flipCoolDown;

        //Calculates the number of coins in this level based on the number of objects tagged as Coin.
        GameObject[] coinObjectList;
        coinObjectList = GameObject.FindGameObjectsWithTag("Coin");
        gameData.TotalNumberOfCoins = coinObjectList.Length;

        // Set limit for healthbar to allow proper proportion highlighted

        // Update current character selected
        UpdateCurrentCharacterDisplay();

        // Update whether flip is active
        coolDownActive = (coolDownCount > 0) ? true : false;
        updateFlipText();

		//updates level number based on scene number.
		gameData.LevelNumber = SceneManager.GetActiveScene().buildIndex - TUTORIAL_SCENE_INDEX;
    }

    void Update() {
        // Try to hide the dialog box if it is visible
        if (dialogBox.activeSelf)
        {
            // Hides the dialog box if space is pressed
            tryHideDialogBox(); 
        }

        gameData.Time += Time.deltaTime;
        updateTimeDisplay();

        // Handle hiding an achievement if it is visible
        if (achievementDisplayed)
        {
            tryHideAchivementPopup();
        }

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
                flipWorld();
                coolDownCount = flipCoolDown;
                return;
            }
        }
        else
        {
            coolDownCount -= Time.deltaTime;
        }

		score.text = "Coins: " + getCoinsFound();
	}

	public void RegisterPlayer(PlayerController controller) {
		if (controller.PlayerSide == Side.LIGHT) {
			lightPlayer = controller;
		} else {
			darkPlayer = controller;
		}
	}

   
    /// <summary>
    /// Should be called in the Start method of each Checkpoint.
    /// 
    /// Avoids the need for drag and drop references.
    /// </summary>
    /// <param name="checkpoint"></param>
    public void RegisterCheckpoint(Checkpoint checkpoint)
    {
        if (checkpoint.checkpointSide == Side.DARK)
        {
            darkSideCheckpoints.Add(checkpoint);
        } else
        {
            lightSideCheckpoints.Add(checkpoint);
        }
    }

    /// <summary>
    /// Retrieves a checkpoint of a specific order.
    /// 
    /// NOTE: Assumes the level designer has configured checkpoints in ascending 'order' from 1.
    /// </summary>
    /// <param name="playerSide"></param>
    /// <param name="order"></param>
    /// <returns></returns>
    public Checkpoint getCheckpoint(Side playerSide, int order)
    {
        List<Checkpoint> checkpoints = (playerSide == Side.LIGHT) ? lightSideCheckpoints : darkSideCheckpoints;
 
        foreach (Checkpoint check in checkpoints)
        {
            if (check.order == order)
            {
                return check;
            }
        }
        return null;
    }

    /// <summary>
    /// 
    /// This method should be called when the player reached a lose state.
    /// 
    /// </summary>
    public void gameOver()
    {
		ApplicationModel.time = gameData.Time;
		ApplicationModel.coinsFound = gameData.CoinsFound;
		ApplicationModel.totalCoins = getTotalCoins();
		ApplicationModel.deathCount = gameData.Deaths;
		ApplicationModel.levelNumber = gameData.LevelNumber;
		ApplicationModel.score = calculateScore (gameData.CoinScore, gameData.Time, gameData.Deaths);
		if (gameData.LevelNumber == 0) {
			ApplicationModel.levelName = "Tutorial";
		} else {
			ApplicationModel.levelName = "Level "+ gameData.LevelNumber;
		}

		SceneManager.LoadScene (GameOverController.GAME_OVER_SCENE_NAME);

    }


    public void foundCoin(int score)
    {
        gameData.CoinsFound++;
        gameData.CoinScore += score;
    }

    public int getCoinsFound()
    {
        return gameData.CoinsFound;
    }

    public int getTotalCoins()
    {
        return gameData.TotalNumberOfCoins;
    }

    public int getTime()
    {
        return (int)gameData.Time;
    }

	public int getScore()
	{
		return gameData.CoinScore;
	}

    public void setInventoryItem(SpecialCollectible specialItem)
    {
        gameData.setInventoryItem(specialItem);
    }

    public SpecialItem getInventoryItemType()
    {
        return gameData.getItemType();
    }

    public int getInventoryItemIndex()
    {
        return gameData.getItemIndex();
    }

    public void setCurrentSide(Side newSide)
    {
        currentSide = newSide;
    }
  

    /// <summary>
    /// Updates the player's current health and adjusts their health bar accordingly
    /// </summary>
    public bool addHeart()
    {
        if (getCurrentHealth() < MAX_HEALTH)
        {
            gameData.Heart++;
			healthBar.showLastHeart ();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Removes one of the brothers shared hearts.
    /// </summary>
    private void removeHeart()
    {
        if (gameData.Heart > 1)
        {
            gameData.Heart--;
			healthBar.hideLastHeart();
           
        } else
        {
            gameData.Heart--;
			healthBar.hideLastHeart();
            gameOver();
        }
        
    }

    private IEnumerator allowPlayerOffScreen()
    {
        cameraMovement.allowOffScreen(true);
        yield return 0;
        yield return 0;
        cameraMovement.allowOffScreen(false);
    }


    // Handle level behaviour on player death
    public void playerDeath()
    {
        if (cameraMovement != null)
        {
            StartCoroutine(allowPlayerOffScreen());
        }
        gameData.Deaths++;
        removeHeart();

        int lightCheckpoint = lightPlayer.getCheckpointNumber(),
            darkCheckpoint = darkPlayer.getCheckpointNumber();
        int resetTo = Math.Min(lightCheckpoint, darkCheckpoint);

        // Move the camera to the midpoint between the two players checkpoint.
        Vector3 lightCheckPoint = getCheckpoint(Side.LIGHT, resetTo).getPosition();
        Vector3 darkCheckPoint = getCheckpoint(Side.DARK, resetTo).getPosition();

        Vector3 newCameraPosition = (lightCheckPoint + darkCheckPoint) / 2.0f;
        newCameraPosition.z = cameraPinController.transform.position.z;
        cameraPinController.transform.position = newCameraPosition;

        cameraPinController.resetShakyCam();

        darkPlayer.resetToCheckpoint(resetTo);
        lightPlayer.resetToCheckpoint(resetTo);
        //not running


        // Reset all enemies and collectables in level.
        foreach (Enemy e in FindObjectsOfType<Enemy>()) {
            e.ResetBehaviour();
        }
        foreach (Collectible c in FindObjectsOfType<Collectible>()) {
            c.ResetBehaviour();
        }
    }

    public int getCurrentHealth()
    {
        return gameData.Heart;
    }

	public void resetHealth(){
		gameData.Heart = MAX_HEALTH;
		healthBar.showAllHearts ();
	}

    public PlayerController getActivePlayer()
    {
		if (getSide () == Side.LIGHT) {
			return lightPlayer;
		} else {
			return darkPlayer;
		}
	}

    public PlayerController getDarkPlayer()
    {
        return darkPlayer;
    }

    public PlayerController getLightPlayer()
    {
        return lightPlayer;
    }

    public List<PlayerController> getAllPlayers()
	{
		return new List<PlayerController> { lightPlayer, darkPlayer };
	}
	
	/*
		Called by the Game Controller to flip the world.
	*/
	private void flipWorld() {
        // Since a flip has occurred, set cool down as active
        coolDownActive = true;

        cameraPinController.doFlip();
		if (currentSide == Side.DARK) {
			currentSide = Side.LIGHT;
		} else {
			currentSide = Side.DARK;
        }
        UpdateCurrentCharacterDisplay();
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
	
	
	public bool isActivate()
	{
	    return !disableInput && Input.GetKeyDown(activateAction);
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
		if (currentSide == Side.LIGHT) {
			adjust = -1f;
		}
		if (!disableInput) {
			return adjust * Input.GetAxis("Horizontal");
		}
	    return 0f;
	}
	
	/*
	*	Will return true as long as the jump action key is being held down.
	*	
	*	If input is disabled for the GameController, this method will always
	*	return false.
	*/
	public bool isJump()
	{
	    if (!disableInput) {
            return Input.GetButton("Jump") || Input.GetButton("Vertical");
		}
	    return false;
	}

    /*
     * Updates the time displayed in the UI 
     */
    void updateTimeDisplay()
    {
        timeText.text = string.Format("Time: {0:#0.00} seconds", gameData.Time); 
    }

    /*
     * Updates the character name and avatar displayed in the UI, based on the current world side
     */ 
    void UpdateCurrentCharacterDisplay()
    {
        //TODO need to update character names
        if(currentSide == Side.DARK)
        {
            characterName.text = "Older brother";
            characterAvatar.sprite = darkCharacter; 
        }else
        {
            characterName.text = "Younger brother";
            characterAvatar.sprite = lightCharacter; 
        }
    }

    /*
     * Update the color of the flip text to show if flipping is enabled or not
     */ 
    void updateFlipText()
    {
        flipText.color = (coolDownActive) ? nearlyTransparentWhite : flipText.color = Color.white;
    }

    /*
     * Record an achievement as unlocked and display it, if this is the first time it was unlocked
     */
    void unlockAchievement(string achievementName)
    {
        Achievements.UnlockAchievement(achievementName, achievementPopUp, achievementText, gameData, achievementSound);
        achievementPopUpCountdown = 2.0f;
        achievementDisplayed = true; 
    }

    /*
     * Hides the achievement popup when the achievement countdown ends. 
     */
    void tryHideAchivementPopup()
    {
        achievementPopUpCountdown -= Time.deltaTime;

        // Don't hide popup if countdown time still remains
        if (achievementPopUpCountdown > 0)
        {
            return;
        }

        Achievements.HideAchivementPopup(achievementPopUp);
        achievementDisplayed = false; 
    }

    /*
     * Show dialog on screen for a character, as described by their name, until space is pressed. 
     */
    void showDialogbox(string characterName, string message)
    {
        dialogBoxCharacterName.text = characterName;
        dialogBoxMessage.text = message;
        dialogBox.SetActive(true);
        disableInput = true; 
    }

    /*
     * Hide the dialog box if the correct key is being pressed (space) 
     */
    void tryHideDialogBox()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogBox.SetActive(false);
            disableInput = false; 
        }
    }

	public void setFinishedLevel(bool finished){
		// check side
		if (getSide () == Side.DARK) {
			finishedLevelDark = finished;
		}
		if (getSide () == Side.LIGHT) {
			finishedLevelLight = finished;
		}

		if (finishedLevelDark && finishedLevelLight) {
			finishTheGame ();
		}
	}

	void finishTheGame(){
		//send score and time to ApplicationModel
		ApplicationModel.time = gameData.Time;
	    ApplicationModel.coinsFound = gameData.CoinsFound;
	    ApplicationModel.totalCoins = getTotalCoins();
	    ApplicationModel.deathCount = gameData.Deaths;
		ApplicationModel.levelNumber = gameData.LevelNumber;
		ApplicationModel.score = calculateScore (gameData.CoinScore, gameData.Time, gameData.Deaths);
		if (gameData.LevelNumber == 0) {
			ApplicationModel.levelName = "Tutorial";
		} else {
			ApplicationModel.levelName = "Level "+ gameData.LevelNumber;
		}

	    // Trigger finish scene
	    SceneManager.LoadScene("Finish Scene");
    }

	public int calculateScore(int CoinScore, float time, int deaths)
	{
        // Work our multipliers
	    ApplicationModel.timeMultiplier =Math.Round(10/Mathf.Log10(time + 5f), 2);
        ApplicationModel.deathMultiplier = Math.Round(1/(deaths + 1f), 2);
        
        // Work out final score
		return (int)(CoinScore * 100 * ApplicationModel.timeMultiplier * ApplicationModel.deathMultiplier);
	}

    public GameData GetGameData()
    {
        return gameData; 
    }

    public void enableShakyCam()
    {
        cameraPinController.enableShakyCam = true;
    }

    public void disableShakyCam()
    {
        cameraPinController.enableShakyCam = false;
    }

	public void setInMainMenu(bool state) {
		inMainMenu = state;
	}

	public bool getInMainMenu() {
		return inMainMenu;
	}

    public float getZValueForSide(Side side)
    {
        return (side == Side.LIGHT) ? lightSideZ : darkSideZ;
    }
}
