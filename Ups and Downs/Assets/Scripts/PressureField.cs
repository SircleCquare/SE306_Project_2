using UnityEngine;
using System.Collections;

public class PressureField : TriggerSphere
{

    public PressurePlate plate;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "PlayerGround")
        {
            Debug.Log("Enter -  Child");
            plate.setPlayerStandingOn(true);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "PlayerGround")
        {
            Debug.Log("Exit - Child");
            plate.setPlayerStandingOn(false);
        }
    }
}
