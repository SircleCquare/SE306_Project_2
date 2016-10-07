using UnityEngine;
using System.Collections;

public class BubbleSpawner : MonoBehaviour {

	private float life;
	public float lifeMin = 3.0f;
	public float lifeMax = 10.0f;
	public float growth = 0.5f;
	// Use this for initialization
	void Start() {
		life = Random.Range(lifeMin, lifeMax);
	}
	
	// Update is called once per frame
	void Update() {
		life -= Time.deltaTime;
		if (life < 0) {
			Destroy (gameObject);
			return;
		} else {
			transform.localScale += growth * Time.deltaTime * Vector3.one;
		}
	}
}
