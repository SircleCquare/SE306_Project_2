using UnityEngine;
using System.Collections;

public class DeLeech : Collectible {
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
