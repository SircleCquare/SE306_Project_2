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
	public bool moveY;
	private float runTime;
	private float spawnTime;
	private float forwardY;
	private Vector3 homePosition;

	// Use this for initialization
	void Start() {
		runTime = 0.0f;
		spawnTime = 1.0f / rate;
		homePosition = transform.position;
		forwardY = 0.0f;
	}

	void Update(){
		Debug.Log("started");
		runTime += Time.deltaTime;

		while (runTime >= spawnTime) {
			runTime -= spawnTime;
			SpawnSphere();
		}

		transform.LookAt(player);

		bool triggered = Vector3.Distance (transform.position, player.position) <= triggerRadius;
		bool returnHome = Vector3.Distance (transform.position, homePosition) >= movementRadius;

		if (inputControl.getSide() == Side.Dark) {
			if (moveY) {
				forwardY = transform.forward.y;
			}
			if (triggered && !returnHome && CanSeePlayer(transform.forward)) {
				transform.position += (new Vector3 (transform.forward.x, forwardY, 0) * speed * Time.deltaTime);
			} else if (returnHome) {
				transform.position = homePosition;
			}

			// TODO: update home position to current location when player is hit?
		}
	}
	
	// Update is called once per frame
	void SpawnSphere() {
		Instantiate(bubble, Random.insideUnitSphere + transform.position, Quaternion.identity);
	}

	bool CanSeePlayer(Vector3 rayDirection) {
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(transform.position, rayDirection, out hit)) {
			if (hit.transform == player) {
				Debug.Log ("can see");
				return true;
			} 
		}
		Debug.Log ("cannot see");
		return false;
	}
}
