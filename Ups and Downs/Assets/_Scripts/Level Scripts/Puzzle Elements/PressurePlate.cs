using UnityEngine;
using System;
using System.Collections;

public class PressurePlate : Switch
{
    /* How long the pressure pad takes to compress */
	public float compressTime = 0.5f;

    
    private Vector3 uncompressedPosition, compressedPosition;
    public float compressionDistance = 0.5f;
    private Transform plate;
    public PlateState state = PlateState.IDLE;

    public enum PlateState { IDLE, MOVING, COMPRESSED };

    // Fixes issue where plate will raise if player leaves trigger even if block is still there
    private int numObjectsOnPlate = 0;


	// Use this for initialization
    protected override void Start () {
        base.Start();

        // Get actual plate object that moves up and down
        plate = Array.Find(GetComponentsInChildren<Transform>(), child => child.name.Equals("Plate"));

        uncompressedPosition = plate.position;
        compressionDistance = plate.localScale.y - 0.6f;

        compressedPosition = uncompressedPosition;
        compressedPosition.y -= compressionDistance;
    }


    /**
     * Coroutine to compress the plate, moves it between
     * the initial and compressed position over the compress time
     **/
    private IEnumerator CompressPlate() {
        state = PlateState.MOVING;
        Vector3 initialPosition = plate.position;
        float time = 0f;
        while (time <= compressTime) {
            plate.position = Vector3.Lerp(initialPosition, compressedPosition, time/compressTime);
            time += Time.deltaTime;
            yield return 0;   
        }

        foreach (Switchable target in targetList)
        {
            target.activate();
        }
        state = PlateState.COMPRESSED;
    }


    /**
     * Coroutine to compress the plate, moves it between
     * the initial and uncompressed position over the compress time
     **/
    private IEnumerator RaisePlate() {
        state = PlateState.MOVING;
        Vector3 initialPosition = plate.position;
        float time = 0f;
        while (time < compressTime) {
            plate.position = Vector3.Lerp(initialPosition, uncompressedPosition, time/compressTime);
            time += Time.deltaTime;
            yield return 0;   
        }
        foreach (Switchable target in targetList)
        {
            target.deactivate();
        }

        state = PlateState.IDLE;
    }

    void OnTriggerEnter(Collider col)
    {
        if (IsPlayerOrBlock(col.tag))
        {
            numObjectsOnPlate++;
            // Stop any current co-routines before starting a new one
            StopCoroutine(CompressPlate());
            StopCoroutine(RaisePlate());
            StartCoroutine(CompressPlate());
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (IsPlayerOrBlock(col.tag))
        {
            numObjectsOnPlate--;
            if (numObjectsOnPlate != 0)
            {
                return;
            }
            // Stop any current co-routines before starting a new one
			StopCoroutine(CompressPlate());
			StopCoroutine(RaisePlate());
			StartCoroutine (RaisePlate ());
        }
    }

    private bool IsPlayerOrBlock(string colliderTag)
    {
        return (colliderTag == GameController.PLAYER_TAG) || (colliderTag == GameController.WEIGHTED_TAG);
    }
}
