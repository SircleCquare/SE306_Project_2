using UnityEngine;
using System.Collections;

public class Coin : Collectible {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void onPickup() {
		base.onPickup();
		Debug.Log("Coin picked up");
		//Play a coin-specific sound?
	}
}
