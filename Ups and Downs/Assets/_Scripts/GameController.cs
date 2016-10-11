using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : SingletonObject<GameController> {

    public bool renderSwitchPaths = false;

    private GameData gameData;

    private const string SAVE_FOLDER_PATH = "save_data";
    private const string SAVE_FILE_PATH = SAVE_FOLDER_PATH + "/save.ser";

    /** The number of seconds a player has to wait between flips */
    public float flipCoolDown = 2.0f;
	public Side currentSide = Side.Dark;
	public KeyCode flipAction = KeyCode.F;
	public KeyCode activateAction = KeyCode.E;

    private const int MAX_HEALTH = 100;

    public CameraPinController cameraPinController;
    private bool disableInput = false;
    private float coolDownCount;
    private bool coolDownActive; 

	private bool finishedLevelDark;
	private bool finishedLevelLight;

    /* UI components */
    public Slider healthBar;
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

	private PlayerController lightPlayer, darkPlayer;

    // Used to color flip text to show flipping is disabled
    private readonly Color nearlyTransparentWhite = new Color(1, 1, 1, 0.1f);

    void Start()
    {
        Debug.Log("Loading save");

        LoadGame();
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

        // Try to hide the dialog box if it is visible
        if (dialogBox.activeSelf)
        {
            // Hides the dialog box if space is pressed
            tryHideDialogBox(); 
        }

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

		score.text = "Score: " + getScore();
	}

	public void RegisterPlayer(PlayerController controller) {
		if (controller.PlayerSide == Side.Light) {
			lightPlayer = controller;
		} else {
			darkPlayer = controller;
		}
	}

    /** 
     * Save the game data to a save file
     */
    public void SaveGame()
    {
        // Make sure save folder exists
        if (!Directory.Exists(SAVE_FOLDER_PATH))
        {
            Debug.Log("Save folder does not exist, creating");

            Directory.CreateDirectory(SAVE_FOLDER_PATH);
        }

        // Serialise data and save to save file
        using (FileStream saveFile = File.Create(SAVE_FILE_PATH))
        {
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(saveFile, gameData);

            saveFile.Close();  
        }
        Debug.Log("Game saved");
    }

    /** 
     * Load the game data from a save file
     */
    public void LoadGame()
    {
        if (!File.Exists(SAVE_FILE_PATH))
        {
            // If no save exists, create new game data and save
            gameData = new GameData();
            Debug.Log("No save file found");
            SaveGame();
        }
        else
        {
            // Load file
            using (FileStream saveFile = File.Open(SAVE_FILE_PATH, FileMode.Open))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                gameData = serializer.Deserialize(saveFile) as GameData;
                saveFile.Close();
                Debug.Log("Save data loaded");
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

	public int getScore()
	{
		return gameData.coinsFound * 10;
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

	public void resetHealth(){
		gameData.health = MAX_HEALTH;
		healthBar.value = MAX_HEALTH;
	}

    public PlayerController getActivePlayer()
    {
		if (getSide () == Side.Light) {
			return lightPlayer;
		} else {
			return darkPlayer;
		}
	}

    public PlayerController getDarkPlayer()
    {
        return darkPlayer;
    }

	public List<PlayerController> getAllPlayers()
	{
		return new List<PlayerController> { lightPlayer, darkPlayer };
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

    /*
     * Record an achievement as unlocked and display it, if this is the first time it was unlocked
     */
    void unlockAchievement(string achievementName)
    {
        // Only pop achievement description when achievement first awarded
        if (!gameData.awardedAchievements.Contains(achievementName))
        {
            // Record achievement
            gameData.awardedAchievements.Add(achievementName);

            // Display achievement pop up 
            achievementText.text = Achievements.achievementList[achievementName];
            achievementPopUp.SetActive(true);
        }
    }

    /*
     * Hide the achievement pop up
     */
    void hideAchivementPopup()
    {
        achievementPopUp.SetActive(false);
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
		if (getSide () == Side.Dark) {
			finishedLevelDark = finished;
		}
		if (getSide () == Side.Light) {
			finishedLevelLight = finished;
		}

		if (finishedLevelDark && finishedLevelLight) {
			finishTheGame ();
		}
	}

	void finishTheGame(){
		//send score and time to ApplicationModel
		ApplicationModel.score = gameData.coinsFound * 10;
		ApplicationModel.time = gameData.time;
	    ApplicationModel.levelName = "Tutorial"; // TODO

	    // Trigger finish scene
	    SceneManager.LoadScene("Finish Scene");
	}

}
