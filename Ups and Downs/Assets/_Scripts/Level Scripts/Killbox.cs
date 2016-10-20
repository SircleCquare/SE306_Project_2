using UnityEngine;
using System.Collections;

public class Killbox : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		
		GameObject collided = other.gameObject;

		if (collided.tag != GameController.PLAYER_TAG) return;

		GameController.Singleton.playerDeath();
	}
}