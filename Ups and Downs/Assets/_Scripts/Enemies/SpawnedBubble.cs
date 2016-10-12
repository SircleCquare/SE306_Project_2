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

    private float life;

    void Start()
    {
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
        if (other.gameObject.tag == GameController.PLAYER_TAG)
        {
            if (growth > 0)
            {
                growth = -growth;
                transform.position += Vector3.Normalize(Vector3.MoveTowards(transform.position, other.gameObject.transform.position, 1f)) * growth;
            }
        }
        
    }
}