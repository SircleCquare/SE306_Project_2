using UnityEngine;
using System.Collections;

/// <summary>
/// The abstract super classes of all enemy NPC's.
/// </summary>
public abstract class Enemy : MonoBehaviour {

  /*
  * The position, rotation and scale used for resetting this enemy on level reset.
  */
  private Vector3 originPosition;
  private Vector3 originRotation;
  private Vector3 originScale;

  /// <summary>
  /// Set position, rotation and scale at runtime from the enemy's world position
  /// (Used to replace the enemy).
  /// </summary>
  protected virtual void Start()
  {
    originPosition = transform.position;
    originRotation = transform.eulerAngles;
    originScale = transform.localScale;
  }

	protected virtual void Update ()
	{
        // If the currently active side is the dark side
        if (GameController.Singleton.getSide() == Side.DARK)
        {
          // Call dark-side specific update methods.
          UpdateActive();
          // Enable rendering of the enemy.
          GetComponent<Renderer>().enabled = true;
        }
        else
        {
          // Disable rendering of the enemy.
          GetComponent<Renderer>().enabled = false;
        }
	}

  /// <summary>
  /// An abstract Update method which is only processed when the Dark Side is active.
  /// </summary>
  protected abstract void UpdateActive();

  /// <summary>
  /// Reset the enemy's position, state and visiblility on level reset.
  /// </summary>
  public virtual void ResetBehaviour()
  {
    transform.position = originPosition;
    transform.eulerAngles = originRotation;
    transform.localScale = originScale;
  }
}
