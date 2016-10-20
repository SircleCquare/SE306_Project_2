using UnityEngine;
using System.Collections;

/// <summary>
/// A bubble spawned by the Sphere Enemy.
///
/// This script controls the life and death of the bubble independly from its parent (Sphere Enemy).
/// </summary>
public class SpawnedBubble : Enemy
{

  /// <summary>
  /// The minimum life span of a bubble in seconds.
  /// </summary>
  public float lifeMin;

  /// <summary>
  /// The maximum life span of a bubble in seconds.
  /// </summary>
  public float lifeMax;

  /// <summary>
  /// The growth rate of a bubble.
  /// </summary>
  public float growth = 0.5f;

  /// <summary>
  /// Force mode to apply to player on collision.
  /// </summary>
  public ForceMode forceMode = ForceMode.VelocityChange;

  /// <summary>
  /// Force to apply to player on collision.
  /// </summary>
  public float pushForce = 20f;

  private float life;

  protected override void Start()
  {
    base.Start();
    life = Random.Range(lifeMin, lifeMax);
  }

  /// <summary>
  /// Called once per frame when the dark side is active.
  /// </summary>
  protected override void UpdateActive()
  {
    life -= Time.deltaTime;
    if (life < 0)
    {
      Destroy(gameObject);
      return;
    }
    else
    {
      transform.localScale += growth * Time.deltaTime * Vector3.one;
    }
  }

  /// <summary>
  /// When a bubble is touched by the player it starts growing smaller and steps a small distance away.
  /// </summary>
  /// <param name="other"></param>
  void OnTriggerEnter(Collider other)
  {
    GameObject collided = other.gameObject;

    if (collided.tag != GameController.PLAYER_TAG) return;

    Rigidbody playerRb = collided.GetComponent<Rigidbody>();
    Vector3 forceDirection = Vector3.Normalize(collided.transform.position - transform.position);
    playerRb.AddForce(forceDirection * pushForce, forceMode);
  }
}
