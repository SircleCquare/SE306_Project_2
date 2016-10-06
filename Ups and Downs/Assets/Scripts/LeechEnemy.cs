using UnityEngine;
using System.Collections;


public class LeechEnemy : MonoBehaviour
{
	public float rate = 5.0f;
	public Transform player;
	public float hitRadius = 0.5f;
	private bool attached = false;

	// Use this for initialization
	// void Start() {
	//
	// }

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

	// Update is called once per frame
	void Update()
	{
		if (attached) {
			return;
		}

		bool hit = Vector3.Distance (transform.position, player.position) <= hitRadius;
		if (!hit)
		{
			return;
		}

        Debug.Log("Hit Leech");
		attached = true;
		getGameController().getActivePlayer().addLeech(this);
		this.transform.parent = player.transform;
	}

	public void Destroy()
	{
		Destroy(this.gameObject);
	}
}
