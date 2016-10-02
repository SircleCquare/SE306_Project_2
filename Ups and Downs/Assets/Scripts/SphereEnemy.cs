using UnityEngine;
using System.Collections;

public class SphereEnemy : MonoBehaviour {
	
	public float rate = 5.0f;
	public Transform bubble;

	// Use this for initialization
	void Start() {
		InvokeRepeating("SpawnSphere", 0.5f , 1.0f / rate);
	}
	
	// Update is called once per frame
	void SpawnSphere() {
		Instantiate(bubble, Random.insideUnitSphere + transform.position, Quaternion.identity);
		Debug.Log("this is going");
	}
}
