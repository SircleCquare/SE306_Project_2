using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {
    
    private Vector3 originPosition;
    private Vector3 originRotation;
    private Vector3 originScale;
    // To handle reset of collectibles on death
    public bool isResetOnDeath = true;
    private bool pickedUp = false;

    protected virtual void Start()
    {
        originPosition = transform.position;
        originRotation = transform.eulerAngles;
        originScale = transform.localScale;
    }

	
	protected void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == GameController.PLAYER_TAG && !pickedUp)
        {
            onPickup();
        }
		
    }
	
    public virtual void onPickup() {
        if (!isResetOnDeath)
        {
            Destroy(this.gameObject);
        }
        GetComponent<Renderer>().enabled = false;
        pickedUp = true;
	}

    protected GameController getGameController()
    {
        return GameController.Singleton;
    }

    public virtual void ResetBehaviour()
    {
        transform.position = originPosition;
        transform.eulerAngles = originRotation;
        transform.localScale = originScale;
        GetComponent<Renderer>().enabled = true;
        pickedUp = false;
    }
}
