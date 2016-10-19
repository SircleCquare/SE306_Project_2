using UnityEngine;

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
    public float visionDistance;
    // How far the Sphere will chase a player before returning home.
    public float chaseDistanceX;
    public float chaseDistanceY;
    // How far away from the player a sphere will stop
    public float refrainRadius;
    // How far away from the play bubbles will spawn
    public float spawnDistance;
    // The radius of the space around the sphere enemy in which bubbles will be spawned.
    public float cloudSize;

    // How long the sphere enemy will wait away from it's home position after it can't see you
    public float resetTime;

    public float speed;
    public bool moveY;

    private float playerLastSeenTime;
    private float runTime;
    private float spawnTime;
    private float forwardY;
    private Vector3 homePosition;

    protected override void Start() {
        base.Start();

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
        
        Vector3 position = darkPlayer.position;
        position.y += 1.5f; // Aim at player's torso - without this, it aims at feet

        // Point at player so we can apply forward velocities and not worry about angle.
        transform.LookAt(position);


        float distToPlayer = Vector3.Distance(transform.position, darkPlayer.position);

        bool triggered = distToPlayer <= visionDistance;

        Vector3 positionFromHome = transform.position - homePosition;
        bool returnHome = Mathf.Abs(positionFromHome.y) > chaseDistanceY || // Limit how far it'll chase in the X direction
                          Mathf.Abs(positionFromHome.x) > chaseDistanceX || // Limit how far it'll chase in the Y direction
                          (Time.time - playerLastSeenTime) > resetTime; // Limit how long it'll wait after you leave its sight

        if (moveY) {
            forwardY = transform.forward.y;
        }

        if (triggered && !returnHome && CanSeePlayer(transform.forward))
        {
            playerLastSeenTime = Time.time;

            if (distToPlayer >= refrainRadius)
            {
                transform.position += (new Vector3(transform.forward.x, forwardY, 0) * speed * Time.deltaTime);
            }

        } else if (returnHome)
        {
            playerLastSeenTime = Time.time;
            transform.position = homePosition;
        }
    }

    private void SpawnBubble() {
        Instantiate(bubble, cloudSize * Random.onUnitSphere + transform.position, Quaternion.identity);
    }

    bool CanSeePlayer(Vector3 rayDirection) {
        RaycastHit hit;

        if (!Physics.Raycast(transform.position, rayDirection, out hit)) return false;
        return (hit.transform == darkPlayer) ;
    }
}