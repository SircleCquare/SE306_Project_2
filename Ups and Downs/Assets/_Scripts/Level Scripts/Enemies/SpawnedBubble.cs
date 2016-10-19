using UnityEngine;
using System.Collections;

/// <summary>
/// A bubble spawned by the Sphere Enemy.
/// 
/// This script controls the life and death of the bubble independly from its parent (Sphere Enemy).
/// </summary>
public class SpawnedBubble : Enemy
{

    // The minimum life span of a bubble in seconds
    public float lifeMin;
    // The maximum life span of a bubble in seconds
    public float lifeMax;
    // An artibitary growth rate of a bubble.
    public float growth = 0.5f;
    // Forcemode to apply to player on collision
    public ForceMode forceMode = ForceMode.VelocityChange;
    // Force to apply to player on collision
    public Vector3 force = new Vector3(10.0f,0.0f,0.0f);

    private float life;

    protected override void Start()
    {
        base.Start();
        life = Random.Range(lifeMin, lifeMax);
    }

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
    /// 
    /// TODO: Consider Lerping away for smoother effect.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        GameObject collided = other.gameObject;
        if (collided.tag == GameController.PLAYER_TAG)
        {
            Rigidbody playerRb = collided.GetComponent<Rigidbody>();
            playerRb.AddForce(force, forceMode);
        }
        
    }
}