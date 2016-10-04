using UnityEngine;
using System.Collections;

public class RotateScript : MonoBehaviour {

	float frameCount = 0;
	
	void Update () {
		frameCount+= Time.deltaTime;
		frameCount = frameCount % (2 * Mathf.PI);
		transform.Rotate (Vector3.up*5);
		transform.Translate(Vector3.up * Mathf.Sin(frameCount * 4) * 0.007f);
	}
}
