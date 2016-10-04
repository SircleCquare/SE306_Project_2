using UnityEngine;
using System.Collections;

public class PressurePlate : Switchable {

    /* How long the pressure pad takes to compress */
	public float buttonCompressionTime = 1.0f;
    /* Public to be visible by GUI only, DO NOT CHANGE */
    public bool standingOn = false;
    /** Use this switch if you want the pressure pad to deactive targets when completely compressed */
    //public bool inverseSwitch = false;

    private Vector3 initialPos;
    private float compressionDistance;
	
	private bool isActive = false;
	
	private bool isInactive = false;


	// Use this for initialization
	void Start () {
        initialPos = transform.position;
        Debug.Log(initialPos);
        compressionDistance = transform.localScale.y;
	}
	
	// Update is called once per frame
	void Update () {
        //if (standingOn) {
            //Debug.Log("Lowering");
            lowerPlate();
        //} else {
            //Debug.Log("Raising");
            raisePlate();
        //}
        Vector3 currentPos = transform.position;
        currentPos.y = Mathf.Clamp(currentPos.y, initialPos.y - compressionDistance, initialPos.y);
        transform.position = currentPos;
	}
	
	public override void toggle() {
		return;
		if (isActive) {
			raisePlate();
		} else if (isInactive) {
			lowerPlate();
		}
	}

    private void lowerPlate(){
		if (!isActive) {
			transform.Translate(compressionDistance * Vector3.down * Time.deltaTime / buttonCompressionTime);
			if (transform.position.y >= (initialPos.y - compressionDistance)) {
				isActive = true;
				isInactive = false;
			} else {
				isActive = false;
			}
		}
    }

    private void raisePlate()
    {
		if (!isInactive) {
			transform.Translate(compressionDistance * Vector3.up * Time.deltaTime / buttonCompressionTime);
			if (transform.position.y <= (initialPos.y - compressionDistance)) {
				isInactive = true;
				isActive = true;
			} else {
				isInactive = false;
			}
		}
    }

}
