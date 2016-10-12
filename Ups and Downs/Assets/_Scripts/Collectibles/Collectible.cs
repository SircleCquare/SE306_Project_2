using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {
	
	void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == GameController.PLAYER_TAG)
        {
            Debug.Log("Collectible Pickup");
            onPickup();
        }
		
    }
	
	public virtual void onPickup() {
		Destroy(this.gameObject);
	}

    protected GameController getGameController()
    {
        return GameController.Singleton;
    }
}
