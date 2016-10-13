using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FinishController : MonoBehaviour
{
	public Text levelNameText;
	public Text coinsText;
    public Text timeText;
	public Text deathsText;
	public Text scoreText;
    


	// Use this for initialization
	void Start ()
	{
		levelNameText.text = "Completed " + ApplicationModel.levelName.ToString() + "!";
		coinsText.text = ApplicationModel.coinsFound + "/" + ApplicationModel.totalCoins;
		//deathsText.text = ApplicationModel.
	    scoreText.text = ApplicationModel.score.ToString();
	    timeText.text = ApplicationModel.time.ToString("0.0") + " seconds";
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
