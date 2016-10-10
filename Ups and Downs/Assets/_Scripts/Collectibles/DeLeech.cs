using UnityEngine;
using System.Collections;

public class DeLeech : Collectible {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public override void onPickup() {
		base.onPickup();
		var players = getGameController().getAllPlayers();
		foreach(PlayerController player in players)
		{
			player.deLeech();
		}
		//Play a coin-specific sound?
	}
}
