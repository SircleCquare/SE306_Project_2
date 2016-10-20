using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// The Leech enemy attaches onto the player and causes a disorienting
/// camera distortion effect. It can be removed from the player by collecting a
/// DeLeech collectible.
/// </summary>
public class LeechEnemy : Enemy
{
  /// <summary>
  /// The player instance that this leech will interact with.
  /// </summary>
  private PlayerController player;

  /// <summary>
  /// The distance from which the leech will notice the player.
  /// </summary>
  public float hitRadius = 1.5f;

  /// <summary>
  /// The time (in seconds) the leech will take to play its attach animation.
  /// </summary>
  public float attachTime = 1f;

  /// <summary>
  /// Offset to the z-position of the leech so that it is not hidden behind the player when attached.
  /// </summary>
  public float forwardDistance = 0.2f;

	private float currentTime;

  /// <summary>
  /// A semi-random vector so that the leech's attach position on the player has some variation.
  /// </summary>
	private Vector3 randomPosition;

  /// <summary>
  /// The current state of the leech. The state can be IDLE, MOVING or ATTACHED
  /// </summary>
	public LeechState state = LeechState.IDLE;

  /// <summary>
  /// The enum governing leech state.
  /// </summary>
	public enum LeechState { IDLE, MOVING, ATTACHED };

  /// <summary>
  /// Animation curve to control the leech's animation onto the player.
  /// </summary>
	public AnimationCurve jumpHeight;

  /// <summary>
  /// Animation curve for pulsing the leech animation when attached to the player.
  /// </summary>
  public AnimationCurve jumpSizePulse;

  /// <summary>
  /// Multiplier for the leech's jump height.
  /// </summary>
	public float heightMultiple = 3f;

  /// <summary>
  /// The number of times the leech has tried to attach the player. Used to prevent
  /// endless attach animations.
  /// </summary>
  private int tries = 0;

  /// <summary>
  /// Coroutine for handling the leech's animations.
  /// </summary>
  private Coroutine animationRoutine;

  protected override void Start()
  {

    //Get the game controller singleton
    var gameController = GameController.Singleton;

    // Ensure the leech is aligned with the nearest player in the z-axis
    var initPosition = transform.position;
    if (initPosition.z > 0)
    {
      player = gameController.getLightPlayer();
      initPosition.z = gameController.lightSideZ;
    }
    else
    {
      player = gameController.getDarkPlayer();
      initPosition.z = gameController.darkSideZ;
    }
    transform.position = initPosition;
    base.Start();
  }

  /// <summary>
  /// Attach the leech to the player, and set it as the child of the player object.
  /// </summary>
	private IEnumerator AttachToPlayer() {

    // Animation setup
		state = LeechState.MOVING;
		Vector3 attachPosition = GetAttachPosition();
		attachPosition.z -= forwardDistance;
    Vector3 origPosition = transform.position;
    Vector3 initialScale = transform.localScale;
		float time = 0f;

    /*
     * Animates the leech between its initial position and the chosen attach point
     * on the player. If the player moves, leech will move faster.
     * Animation lasts for the attach time.
     */
		while (time < 1f) {
			Vector3 targetPosition = Vector3.Lerp(origPosition, player.transform.TransformPoint(attachPosition), time);

      // extraHeight is used to animate the leech in a curve rather than a straight line.
			float extraHeight = jumpHeight.Evaluate(time) * heightMultiple;
			targetPosition.y += extraHeight;
			transform.position = targetPosition;
			transform.localScale = initialScale * jumpSizePulse.Evaluate (time);
			time += Time.deltaTime / attachTime;
			yield return 0;
		}

    /* Handle behaviour for attaching leech to player */
		transform.parent = player.transform;
		transform.localPosition = attachPosition;
		state = LeechState.ATTACHED;
		player.addLeech(this);
    GameController.Singleton.enableShakyCam();
    /* Remaining code runs the entire time the leech is attached.
     * Animates the leech pulsing .
     */
		float randomTimeScale = UnityEngine.Random.Range(0.3f, 1f);
    float randomAngle = UnityEngine.Random.Range(0,360);
		transform.eulerAngles = new Vector3(0, 0, randomAngle);
		while ( true ) {
			time += Time.deltaTime * randomTimeScale;
			yield return 0f;
			transform.localScale = initialScale * jumpSizePulse.Evaluate (time) * 0.5f;
		}
	}

  /*
   * This function finds the position on the player the leech will attach to.
   * It randomly generates a position on a sphere around the player
   * Raycasts from that position to the player
   * The hit point of the ray is the attach position
   */
  private Vector3 GetAttachPosition() {
    // Find random point around player
    Vector3 startPoint = player.transform.TransformPoint(UnityEngine.Random.insideUnitCircle * 5f);
    // Find the players chest -> this is where we will raycast towards
    var chest = Array.Find(player.GetComponentsInChildren<Transform>(), transform => transform.name.Equals("chest"));
    Vector3 endPoint = chest.position;

    Collider collider = chest.GetComponent<BoxCollider> ();
    RaycastHit hit;
    // collider.Raycast ensures we will only hit the chest
    if (collider.Raycast(new Ray(startPoint, (endPoint - startPoint).normalized), out hit, 6f)) {
			return player.transform.InverseTransformPoint (hit.point);
		} else {
			tries++;
			if (tries > 5) {
				Destroy (gameObject);
				return Vector3.zero;
			} else {
				return GetAttachPosition ();
			}
		}
	}

  /// <summary>
  /// Called once per frame when the dark side is active.
  /// </summary>
	protected override void UpdateActive()
	{
		switch (state) {
			case LeechState.ATTACHED:
				return;
			case LeechState.IDLE:
				bool hit = Vector3.Distance (transform.position, player.transform.position) <= hitRadius;
				if (hit)
				{
          animationRoutine = StartCoroutine(AttachToPlayer());
				}
				break;
		}
	}

  /// <summary>
  /// Reset leech state on player death.
  /// </summary>
  public override void ResetBehaviour()
  {
    if (animationRoutine != null)
    {
      // Reset all leech specific behaviour
      GameController.Singleton.disableShakyCam();
      StopCoroutine(animationRoutine);
      transform.parent = null;
      state = LeechState.IDLE;

      // Reset leech position
      base.ResetBehaviour();
    }
  }

    // Destroy leech object on de-leech collectible pickup
  public void Destroy()
  {
    GameController.Singleton.disableShakyCam();
    Destroy(this.gameObject);
  }
}
