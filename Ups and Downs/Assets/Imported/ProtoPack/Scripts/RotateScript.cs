using UnityEngine;
using System.Collections;

public class RotateScript : MonoBehaviour {

	private float frameCount = 0;
	
	public bool doFloat = true;
	
	public float spinFactor = 1; 
	void Update () {	
		if (doFloat) {
			frameCount+= Time.deltaTime;
			frameCount = frameCount % (2 * Mathf.PI);
			transform.Translate(Vector3.up * Mathf.Sin(frameCount * 4) * 0.007f);
		}
		transform.Rotate (Vector3.up * 5 * spinFactor);

	}
}
