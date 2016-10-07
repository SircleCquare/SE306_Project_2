using UnityEngine;
using System.Collections;


public class LeechEnemy : MonoBehaviour
{
	public float rate = 5.0f;
	public Transform player;
	public float hitRadius = 1.5f;
	public float attachTime = 1f;

	private float currentTime;
	private bool attached = false;
	private bool moving = false;
	private Vector3 randomPosition;

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

		if (moving)
		{
			currentTime += Time.deltaTime;
			transform.position = Vector3.Lerp(transform.position, getNewPosition(), currentTime / attachTime);

			if (currentTime >= attachTime) {
				attached = true;
				transform.parent = player.transform;
			}
			return;
		}

		bool hit = Vector3.Distance (transform.position, player.position) <= hitRadius;
		if (hit)
		{
			attachToPlayer();
			return;
		}
	}

	void attachToPlayer()
	{
		getGameController().getActivePlayer().addLeech(this);
		transform.Rotate(0, 0, 90);

		moving = true;
		currentTime = 0f;
		randomPosition = Random.onUnitSphere * 1.5f;
	}

	Vector3 getNewPosition()
	{
		var interimPos = getInterimPosition();
		RaycastHit ray = new RaycastHit();
		// Slightly randomised y value to vary the height of the leeches (Doesn't work at the moment)
		Vector3 updatedY = player.position;
		// updatedY.y = Random.Range(updatedY.y - 0.8f, updatedY.y + 0.8f);
		Physics.Raycast(interimPos, updatedY - interimPos, out ray);
		return ray.point;
	}

	Vector3 getInterimPosition()
	{
		var result = new Vector3();
		result.x = player.position.x + randomPosition.x;
		result.y = player.position.y + Mathf.Abs(randomPosition.y);
		result.z = player.position.z + randomPosition.z;
		return result;
	}

	public void Destroy()
	{
		Destroy(this.gameObject);
	}
}
