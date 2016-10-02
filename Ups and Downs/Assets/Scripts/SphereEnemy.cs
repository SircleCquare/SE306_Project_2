using UnityEngine;
using System.Collections;

public class SphereEnemy : MonoBehaviour {
	
	public float rate = 5.0f;
	public Transform bubble;
	public Transform player;
	public float minDist = 5.0f;
	public float maxDist = 15.0f;
	public float speed = 10.0f;

	// Use this for initialization
	void Start() {
		InvokeRepeating("SpawnSphere", 0.5f , 1.0f / rate);
	}

	void update(){
		Debug.Log("updating");
		transform.LookAt(player);
		if (Vector3.Distance(transform.position, player.position) >= minDist) {
			transform.position += transform.forward * speed * Time.deltaTime;
			Debug.Log("moving");
			if (Vector3.Distance(transform.position, player.position) <= maxDist) {
				// range limit to do action
			}
		}
	}
	
	// Update is called once per frame
	void SpawnSphere() {
		Instantiate(bubble, Random.insideUnitSphere + transform.position, Quaternion.identity);
		update();
	}
}
