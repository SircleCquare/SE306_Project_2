﻿using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour
{

    /* How long the pressure pad takes to compress */
	public float compressTime = 0.5f;
	public float compressMultiplier = 0.5f;
    public float detectionRadius = 2f;
    public Switchable[] targetList;
    
    private Vector3 uncompressedPosition, compressedPosition;
    private float compressionDistance;
    public PlateState state = PlateState.IDLE;

    public enum PlateState { IDLE, MOVING, COMPRESSED };


	// Use this for initialization
	void Start () {
        uncompressedPosition = transform.position;
        compressionDistance = transform.localScale.y;

        compressedPosition = uncompressedPosition;
        compressedPosition.y -= compressionDistance;
    }



    private IEnumerator CompressPlate() {
        state = PlateState.MOVING;
        Vector3 initialPosition = transform.position;
        float time = 0f;
        while (time < compressTime) {
            transform.position = Vector3.Lerp(initialPosition, compressedPosition, time);
            time += Time.deltaTime / compressTime;
            yield return 0;   
        }

        foreach (Switchable target in targetList)
        {
            target.activate();
        }
        state = PlateState.COMPRESSED;
    }

    private IEnumerator RaisePlate() {
        state = PlateState.MOVING;
        Vector3 initialPosition = transform.position;
        float time = 0f;
        while (time < compressTime) {
            transform.position = Vector3.Lerp(initialPosition, uncompressedPosition, time);;
            time += Time.deltaTime / compressTime;
            yield return 0;   
        }

        foreach (Switchable target in targetList)
        {
            target.deactivate();
        }

        state = PlateState.IDLE;
    }

    private Coroutine raiseRoutine, compressRoutine;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == GameController.PLAYER_TAG)
        {
            StopCoroutine(CompressPlate());
            StopCoroutine(RaisePlate());

            compressRoutine = StartCoroutine(CompressPlate());
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == GameController.PLAYER_TAG)
        {
            StopCoroutine(CompressPlate());
            StopCoroutine(RaisePlate());

            raiseRoutine = StartCoroutine(RaisePlate());
        }
    }
}
