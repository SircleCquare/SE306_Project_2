using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {
	
	void OnTriggerEnter(Collider other) {
		onPickup();
    }
	
	public virtual void onPickup() {
		Destroy(this.gameObject);
	}

    protected GameController getGameController()
    {
        GameObject[] gameControllerList;
        gameControllerList = GameObject.FindGameObjectsWithTag("GameController");
        GameController controller = null;
        foreach (GameObject obj in gameControllerList)
        {
            controller = obj.GetComponent<GameController>();
            if (controller != null)
            {
                break;
            }
        }
        return controller;
    }
}
