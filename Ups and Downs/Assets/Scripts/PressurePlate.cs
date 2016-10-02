using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour {
	

	public float buttonActivateTime = 1.0f;
	public float compressionRatio = 0.5f;
	public bool initialState = false;
	
	private bool standingOn;
	private float standingTime;
	private Vector3 activeSize;
	private Vector3 deactiveSize;


	// Use this for initialization
	void Start () {
		activeSize = transform.localScale;
		deactiveSize = activeSize;
		deactiveSize.y = activeSize.y * compressionRatio;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Player") {
			if (!standingOn) {
				standingOn = true;
				standingTime = buttonActivateTime;
			}
		}
		
	}
	// On the assumption that no 2 players can stand on one plate
	void OnCollisionStay(Collision collision) {
		if (collision.gameObject.tag == "Player") {
			Debug.Log("Player on block");
			standingTime -= Time.deltaTime;
			if (standingTime < 0) {
				transform.localScale = deactiveSize;
			} else {
				float frac = standingTime / buttonActivateTime;
				transform.localScale = Vector3.Lerp(deactiveSize, activeSize, frac);
			}
		}
		Debug.Log("Not player on block");
		
	}
	
	
	void OnCollisionExit(Collision collision) {
		if (collision.gameObject.tag == "Player") {
			standingOn = false;
			standingTime = buttonActivateTime;
		}
	}
}
