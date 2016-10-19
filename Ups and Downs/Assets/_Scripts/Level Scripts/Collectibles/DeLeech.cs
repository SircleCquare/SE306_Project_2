using UnityEngine;
using System.Collections;

/// <summary>
/// DeLeech is a collectible that removes Leeches from the Dark player when it is picked up.
/// </summary>
public class DeLeech : Collectible {
	public override void onPickup() {
		base.onPickup();
		var players = GameController.Singleton.getAllPlayers();
		// Remove all leeches iteratively.
		foreach(PlayerController player in players)
		{
			player.deLeech();
		}
	}
}
