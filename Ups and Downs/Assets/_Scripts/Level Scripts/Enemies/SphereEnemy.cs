using UnityEngine;

/// <summary>
/// This script controlls the movement control of the Sphere Enemy as well as the spawning of
/// the bubbles that make up its cloud.
///
/// NOTE: Behaviour regarding the Bubbles growth and death is located on their individual prefab.
/// </summary>
public class SphereEnemy : Enemy {

  /// <summary>
  /// How many bubbles per second are spawned. Influences the density of the cloud.
  /// </summary>
  public float rate = 5.0f;

  /// <summary>
  /// How far a Sphere can see a player from
  /// </summary>
  public float visionDistance;

  /// <summary>
  /// How far the Sphere will chase a player (x distance) before returning home.
  /// </summary>
  public float chaseDistanceX;

  /// <summary>
  /// How far the Sphere will chase a player (y distance) before returning home.
  /// </summary>
  public float chaseDistanceY;

  /// <summary>
  /// How far away from the player a sphere will stop
  /// </summary>
  public float refrainRadius;

  /// <summary>
  /// How far away from the play bubbles will spawn
  /// </summary>
  public float spawnDistance;

  /// <summary>
  /// The radius of the space around the sphere enemy in which bubbles will be spawned.
  /// </summary>
  public float cloudSize;

  /// <summary>
  /// How long the sphere enemy will wait away from it's home position after it can't see you
  /// </summary>
  public float resetTime;

  /// <summary>
  /// The enemy's speed
  /// </summary>
  public float speed;

  //Variables for storing AI state.
  public bool moveY;
  private float playerLastSeenTime;
  private float runTime;
  private float spawnTime;
  private float forwardY;
  private Vector3 homePosition;

  public Transform bubble;

  private Transform darkPlayer;

  protected override void Start() {
      base.Start();

      runTime = 0.0f;
      spawnTime = 1.0f / rate;
      homePosition = transform.position;
      forwardY = 0.0f;
      darkPlayer = GameController.Singleton.getDarkPlayer().gameObject.transform;
  }

  /// <summary>
  /// Called once per frame when the dark side is active.
  /// </summary>
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

    // If the player is within range and the sphere has a clear line of sight to the player
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

  /// <summary>
  /// Create a bubble object at a random position around the core sphere object.
  /// </summary>
  private void SpawnBubble() {
    Instantiate(bubble, cloudSize * Random.onUnitSphere + transform.position, Quaternion.identity);
  }

  /// <summary>
  /// Used to test if the enemy has a clear line of sight towards the player
  /// </summary>
  bool CanSeePlayer(Vector3 rayDirection) {
    RaycastHit hit;

    if (!Physics.Raycast(transform.position, rayDirection, out hit)) return false;
    return (hit.transform == darkPlayer) ;
  }
}
