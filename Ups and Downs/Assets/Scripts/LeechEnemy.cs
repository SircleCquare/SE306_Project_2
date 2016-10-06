using UnityEngine;
using System.Collections;


public class LeechEnemy : MonoBehaviour
{
	public float rate = 5.0f;
	public Transform player;
	public float hitRadius = 1.5f;
	private bool attached = false;

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

		attached = true;
		getGameController().getActivePlayer().addLeech(this);
		transform.position = getNewPosition();
		transform.parent = player.transform;
	}

	Vector3 getNewPosition()
	{
		transform.Rotate(0, 0, 90);
		Debug.Log(transform.rotation);
		transform.position = GeneratedPosition();
		RaycastHit ray = new RaycastHit();
		Physics.Raycast(transform.position, getDirection(), out ray);
		return ray.point;
	}

	Vector3 getDirection()
	{
		// Slightly randomised y value to vary the height of the leeches
		Vector3 newPos = player.position;
		newPos.y = Random.Range(newPos.y - 0.8f, newPos.y + 0.8f);
		return newPos - transform.position;
	}

	Vector3 GeneratedPosition()
	{
		Vector3 sphere = Random.onUnitSphere;
        Vector3 pos;
        pos.x = player.position.x + sphere.x;
        pos.y = player.position.y + Mathf.Abs(sphere.y);
        pos.z = player.position.z + sphere.z;
        return pos;
	}

	public void Destroy()
	{
		Destroy(this.gameObject);
	}
}
