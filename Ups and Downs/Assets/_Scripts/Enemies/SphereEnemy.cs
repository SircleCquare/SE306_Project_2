using UnityEngine;
using System.Collections;

/// <summary>
/// This script controlls the movement control of the Sphere Enemy as well as the spawning of
/// the bubbles that make up its cloud.
/// 
/// NOTE: Behaviour regarding the Bubbles growth and death is located on their individual prefab.
/// </summary>
public class SphereEnemy : Enemy {
	// How many bubbles per second are spawned. Influences the density of the cloud.
	public float rate = 5.0f;
	public Transform bubble;


	private Transform darkPlayer;

    // How far a Sphere can see a player from
    public float visionDistance = 15.0f;
    // How far the Sphere will chase a player before returning home.
    public float chaseDistance = 15.0f;
    // How far away from the player a sphere will stop
    public float refrainRadius = 5.0f;
    // How far away from the play bubbles will spawn
    public float spawnDistance = 1.5f;
    // The radius of the space around the sphere enemy in which bubbles will be spawned.
    public float cloudSize = 1.5f;

    public float speed = 10.0f;
	public bool moveY;

	private float runTime;
	private float spawnTime;
	private float forwardY;
	private Vector3 homePosition;

	void Start() {
		runTime = 0.0f;
		spawnTime = 1.0f / rate;
		homePosition = transform.position;
		forwardY = 0.0f;
        darkPlayer = GameController.Singleton.getDarkPlayer().gameObject.transform;
	}

    protected override void UpdateActive()
    {
        runTime += Time.deltaTime;

		while (runTime >= spawnTime) {
			runTime -= spawnTime;
			SpawnBubble();
		}
        // Point at player so we can apply forward velocities and not worry about angle.
        transform.LookAt(darkPlayer);


        float distToPlayer = Vector3.Distance(transform.position, darkPlayer.position);

        bool triggered = distToPlayer <= visionDistance;
		bool returnHome = Vector3.Distance(transform.position, homePosition) >= chaseDistance;

        if (moveY) {
            forwardY = transform.forward.y;
        }

        if (triggered && !returnHome && CanSeePlayer(transform.forward)) {
            if (distToPlayer >= refrainRadius)
            {
                transform.position += (new Vector3(transform.forward.x, forwardY, 0) * speed * Time.deltaTime);
            }
            
        } else if (returnHome) {
            transform.position = homePosition;
        }
    }

	private void SpawnBubble() {
        // Ensures that a Bubble is not spawned overtop of a player.
        float dist = Vector3.Distance(darkPlayer.transform.position, transform.position) - spawnDistance;
        dist = Mathf.Min(dist, cloudSize);
		Instantiate(bubble, dist * Random.onUnitSphere + transform.position, Quaternion.identity);
	}

	bool CanSeePlayer(Vector3 rayDirection) {
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(transform.position, rayDirection, out hit)) {
			if (hit.transform == darkPlayer) {
				return true;
			} 
		}
		return false;
	}
}
