using UnityEngine;
using System.Collections;

public class PressurePlate : Switchable {

    /** A list of switchs which this pressure pad should trigger when completely compressed */
    public Switchable[] targetList;
    /* How long the pressure pad takes to compress */
	public float buttonCompressionTime = 1.0f;
    /* Public to be visible by GUI only, DO NOT CHANGE */
    public bool standingOn = false;
    /** Use this switch if you want the pressure pad to deactive targets when completely compressed */
    public bool inverseSwitch = false;

    private Vector3 initialPos;
    private float compressionDistance;


	// Use this for initialization
	void Start () {
        initialPos = transform.position;
        Debug.Log(initialPos);
        compressionDistance = transform.localScale.y;
	}
	
	// Update is called once per frame
	void Update () {
        if (standingOn) {
            Debug.Log("Lowering");
            lowerPlate();
        } else {
            Debug.Log("Raising");
            raisePlate();
        }
        Vector3 currentPos = transform.position;
        currentPos.y = Mathf.Clamp(currentPos.y, initialPos.y - compressionDistance, initialPos.y);
        transform.position = currentPos;
        if (isActive())
        {
            activateAllTargets();
        }
        else
        {
            deactivateAllTargets();
        }
	}

    /**
       Returns true if the pressure pad is fully compressed. Else returns false.
    */
    private bool isActive()
    {   
        if (transform.position.y <= (initialPos.y - compressionDistance))
        {   
            if (inverseSwitch)
            {
                return false;
            }
            return true;
        }
        if (inverseSwitch)
        {
            return true;
        }
        return false;
    }

    /**
       Activates all targets attached to this pressure pad.
    */
    private void activateAllTargets()
    {
        for (int i = 0; i < targetList.Length; i++)
        {
            Switchable target = targetList[i];
            target.activate();
        }
    }

    /**
        Deactivates all targets attached to this pressure pad.
    */
    private void deactivateAllTargets()
    {
        for (int i = 0; i < targetList.Length; i++)
        {
            Switchable target = targetList[i];
            target.deactivate();
        }
    }

    public override void activate()
    {

        standingOn = true;
    }

    public override void deactivate()
    {
        standingOn = false;
    }

    private void lowerPlate(){
        transform.Translate(compressionDistance * Vector3.down * Time.deltaTime / buttonCompressionTime);
    }

    private void raisePlate()
    {
        transform.Translate(compressionDistance * Vector3.up * Time.deltaTime / buttonCompressionTime);
    }

}
