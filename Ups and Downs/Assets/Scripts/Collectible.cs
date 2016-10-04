using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		onPickup();
    }
	
	public virtual void onPickup() {
		Destroy(this.gameObject);
	}
}
