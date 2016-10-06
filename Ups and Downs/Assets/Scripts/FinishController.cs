using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FinishController : MonoBehaviour
{
    public Text scoreText;
    public Text timeText;


	// Use this for initialization
	void Start ()
	{
	    scoreText.text = ApplicationModel.score.ToString();
	    timeText.text = ApplicationModel.time.ToString("0.0") + " seconds";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
