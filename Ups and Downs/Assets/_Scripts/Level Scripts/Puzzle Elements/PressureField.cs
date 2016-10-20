using UnityEngine;
using System.Collections;

public class PressureField : TriggerSphere
{
    // used for testing purposes, logs whether or not player is in pressure plate collider
    public PressurePlate plate;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == GameController.PLAYER_TAG)
        {
            Debug.Log("Enter -  Child");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == GameController.PLAYER_TAG)
        {
            Debug.Log("Exit - Child");
        }
    }
}
