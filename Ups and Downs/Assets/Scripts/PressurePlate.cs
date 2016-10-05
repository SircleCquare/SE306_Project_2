using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour
{

    public PressureField field;

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

    private bool stationary = true;

	// Use this for initialization
	void Start () {
        initialPos = transform.position;
        Debug.Log(initialPos);
        compressionDistance = transform.localScale.y;
	}

    // Update is called once per frame
    void Update()
    {
        if (!stationary)
        {
            if (standingOn)
            {
                Debug.Log("Lowering");
                lowerPlate();
            }
            else {
                Debug.Log("Raising");
                raisePlate();
            }
            Vector3 currentPos = transform.position;
            currentPos.y = Mathf.Clamp(currentPos.y, initialPos.y - compressionDistance, initialPos.y);
            transform.position = currentPos;
            checkMovementDone();
        }
    }

    public void setPlayerStandingOn(bool standing)
    {
        standingOn = standing;
        // Toggle switch as leaving
        if (!standingOn)
        {
            field.setToggle();
        }
        stationary = false;
    }
	
    private void checkMovementDone()
    {
        if (transform.position.y <= (initialPos.y - compressionDistance))
        {
            stationary = true;
            field.setToggle();
        } else if (transform.position.y >= initialPos.y)
        {
            stationary = true;
        }
    }

    private void lowerPlate(){
		transform.Translate(compressionDistance * Vector3.down * Time.deltaTime / buttonCompressionTime);
    }

    private void raisePlate()
    {
		transform.Translate(compressionDistance * Vector3.up * Time.deltaTime / buttonCompressionTime);
    }

}
