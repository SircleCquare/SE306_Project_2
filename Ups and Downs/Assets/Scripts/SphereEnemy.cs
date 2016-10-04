using UnityEngine;
using System.Collections;

public class SphereEnemy : MonoBehaviour {
	
	public float rate = 5.0f;
	public Transform bubble;
	public Transform player;
	public float movementRadius = 15.0f;
	public float triggerRadius = 15.0f;
	public float speed = 10.0f;
	public GameController inputControl;

	private Vector3 homePosition;

	// Use this for initialization
	void Start() {
		InvokeRepeating("SpawnSphere", 0.5f , 1.0f / rate);
		homePosition = transform.position;
	}

	void update(){
		Debug.Log("updating");
		transform.LookAt(player);

		bool triggered = Vector3.Distance (transform.position, player.position) <= triggerRadius;
		bool returnHome = Vector3.Distance (transform.position, homePosition) >= movementRadius;

		if (inputControl.getSide() == Side.Dark) {
			if (triggered && !returnHome) {
				transform.position += (new Vector3 (transform.forward.x, 0, 0) * speed * Time.deltaTime);
				Debug.Log ("moving");
			} else if (returnHome) {
				transform.position = homePosition;
			}

			// TODO: update home position to current location when player is hit?
		}
	}
	
	// Update is called once per frame
	void SpawnSphere() {
		Instantiate(bubble, Random.insideUnitSphere + transform.position, Quaternion.identity);
		update();
	}
}
