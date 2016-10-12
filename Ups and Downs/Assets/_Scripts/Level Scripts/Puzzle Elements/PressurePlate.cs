using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour
{

    /* How long the pressure pad takes to compress */
	public float compressTime = 0.5f;
	public float compressMultiplier = 0.5f;
    public float detectionRadius = 2f;
    public Switchable[] targetList;
    
    private Vector3 initialPos, finalPos;
    private float compressionDistance;
    public PlateState state = PlateState.IDLE;

    public enum PlateState { IDLE, MOVING, COMPRESSED };


	// Use this for initialization
	void Start () {
        initialPos = transform.position;
        compressionDistance = transform.localScale.y;
    }



    private IEnumerator CompressPlate() {
        state = PlateState.MOVING;
        initialPos = transform.position;
        finalPos = initialPos;
        finalPos.y = initialPos.y - 0.5f;
        float time = 0f;
        while (time < compressTime) {
            transform.position = Vector3.Lerp(initialPos, finalPos, time);;
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
        initialPos = transform.position;
        finalPos = initialPos;
        finalPos.y = initialPos.y + 0.5f;
        float time = 0f;
        while (time < compressTime) {
            transform.position = Vector3.Lerp(initialPos, finalPos, time);;
            time += Time.deltaTime / compressTime;
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
        if (col.gameObject.tag == GameController.PLAYER_TAG && state == PlateState.IDLE)
        {
            StartCoroutine(CompressPlate());
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == GameController.PLAYER_TAG && state == PlateState.COMPRESSED)
        {
            StartCoroutine(RaisePlate());
        }
    }
}
